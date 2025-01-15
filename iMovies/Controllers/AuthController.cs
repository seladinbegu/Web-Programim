using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using iMovies.Models;
using iMovies.Data.Services;
using Microsoft.EntityFrameworkCore;

namespace iMovies.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration, ITokenService tokenService, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _tokenService = tokenService;
            _roleManager = roleManager;
        }

        // Register user
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid registration data", errors = ModelState });
            }

            var user = new User
            {
                UserName = model.Username,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(new { message = "User registration failed", errors = result.Errors });
            }

            // Ensure the role exists and assign it
            var role = model.Username.Equals("seladin", StringComparison.OrdinalIgnoreCase) ? "Admin" : "User";
            await EnsureRoleExistsAsync(role);

            var roleResult = await _userManager.AddToRoleAsync(user, role);
            if (!roleResult.Succeeded)
            {
                return BadRequest(new { message = "Role assignment failed", errors = roleResult.Errors });
            }

            return Ok(new { message = "User registered successfully." });
        }

        // Login user
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Authenticate the user
            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                // Log the error for debugging (you might use a logging framework like Serilog or NLog)
                return Unauthorized(new { Message = "Invalid login attempt." });
            }

            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                return Unauthorized(new { Message = "User not found." });
            }

            // Ensure user has appropriate roles
            if (model.Username.Equals("seladin", StringComparison.OrdinalIgnoreCase))
            {
                // If the user is 'seladin', make them only an Admin (remove User role if present)
                if (await _userManager.IsInRoleAsync(user, "User"))
                {
                    await _userManager.RemoveFromRoleAsync(user, "User");
                }
                if (!await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    await _userManager.AddToRoleAsync(user, "Admin");
                }
            }
            else
            {
                // For other users, ensure they have 'User' role (and 'Admin' if necessary)
                if (!await _userManager.IsInRoleAsync(user, "User"))
                {
                    await _userManager.AddToRoleAsync(user, "User");
                }

                // You can remove the Admin role if the user is not 'seladin'
                if (await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    await _userManager.RemoveFromRoleAsync(user, "Admin");
                }
            }

            // Generate JWT Token
            var token = await GenerateJwtToken(user);

            // Generate Refresh Token
            var refreshToken = await _tokenService.GenerateRefreshTokenAsync(user);

            // Store the JWT token in a cookie
            Response.Cookies.Append("accessToken", token, new CookieOptions
            {
                Secure = true,    // Ensure the cookie is sent over HTTPS
                SameSite = SameSiteMode.Lax, // Adjust based on your needs
                Expires = DateTimeOffset.UtcNow.AddMinutes(30) // Set expiry time for the token
            });
            Response.Cookies.Append("userName", model.Username, new CookieOptions
            {
                Secure = true,    // Ensure the cookie is sent over HTTPS
                SameSite = SameSiteMode.Lax, // Adjust based on your needs
                Expires = DateTimeOffset.UtcNow.AddMinutes(30) // Set expiry time for the token
            });

            // Store the Refresh Token in a cookie (or optionally return it to the client)
            Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,  // Prevents JavaScript from accessing the cookie
                Secure = true,    // Ensure the cookie is sent over HTTPS
                SameSite = SameSiteMode.Lax, // Adjust based on your needs
                Expires = DateTimeOffset.UtcNow.AddDays(7) // Set expiry time for the refresh token
            });

            return Ok(new
            {
                UserName = user.UserName,
                Email = user.Email
            });
        }



        // Refresh token
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
                return BadRequest(new { Message = "Refresh token is required." });

            var storedRefreshToken = await _tokenService.GetRefreshTokenAsync(refreshToken);
            if (storedRefreshToken == null)
                return Unauthorized(new { Message = "Invalid or expired refresh token." });

            var user = await _userManager.FindByIdAsync(storedRefreshToken.UserId);
            if (user == null)
                return Unauthorized(new { Message = "User not found." });

            var newJwtToken = await GenerateJwtToken(user);
            var newRefreshToken = await _tokenService.GenerateRefreshTokenAsync(user);

            return Ok(new
            {
                AccessToken = newJwtToken,
                RefreshToken = newRefreshToken
            });
        }

        // Generate JWT Token
        private async Task<string> GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["JWT:SigninKey"]);
            var expirationHours = int.Parse(_configuration["JWT:ExpirationHours"]);
            var expirationTime = DateTime.UtcNow.AddHours(expirationHours);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expirationTime,
                Audience = _configuration["JWT:Audience"],
                Issuer = _configuration["JWT:Issuer"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        // Ensure that the required role exists
        private async Task EnsureRoleExistsAsync(string role)
        {
            var roleExists = await _roleManager.RoleExistsAsync(role);
            if (!roleExists)
            {
                var newRole = new IdentityRole(role);
                var createRoleResult = await _roleManager.CreateAsync(newRole);
                if (!createRoleResult.Succeeded)
                {
                    throw new Exception("Failed to create role");
                }
            }
        }

        // Get user ID by username
        [HttpGet("get-user-id/{username}")]
        public async Task<IActionResult> GetUserIdByUsername(string username)
        {
            // Normalize the username to upper case before looking up
            var normalizedUsername = username.ToUpperInvariant();

            var user = await _userManager.Users
                                         .FirstOrDefaultAsync(u => u.NormalizedUserName == normalizedUsername);

            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            return Ok(new { userId = user.Id });
        }
    }
}

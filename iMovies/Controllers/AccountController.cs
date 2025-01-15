using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using iMovies.Data; // Make sure to import your ApplicationDbContext
using iMovies.Models;
using iMovies.Models.DTOs.Movie;
using System.Linq;
using System.Threading.Tasks;

public class AccountController : Controller
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly ApplicationDbContext _context; // Add context for fetching movies

    // Inject dependencies
    public AccountController(SignInManager<User> signInManager, UserManager<User> userManager, ApplicationDbContext context)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _context = context;
    }

    // Display the login view
    [HttpGet]
    public IActionResult Login()
    {
        return View(); // Renders Views/Account/Login.cshtml
    }

    // Handle login form submission
    [HttpPost]
    public async Task<IActionResult> Login(LoginModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model); // Return with validation errors
        }

        var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);

        if (result.Succeeded)
        {
            return RedirectToAction("Index", "Home"); // Redirect to Home or Dashboard
        }

        ModelState.AddModelError("", "Invalid login attempt.");
        return View(model); // Re-render view with errors
    }

    // Handle logout
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login");
    }

    // Display the registration view
    [HttpGet]
    public IActionResult Register()
    {
        return View(); // Renders Views/Account/Register.cshtml
    }

    // Handle registration form submission
    [HttpPost]
    public async Task<IActionResult> Register(RegisterModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model); // Return with validation errors
        }

        var user = new User { UserName = model.Username };
        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            // Optionally, automatically log the user in after registration
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home"); // Redirect to Home or Dashboard
        }

        // If registration fails, add errors to ModelState
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }

        return View(model); // Re-render view with errors
    }

    // Movies action - Fetch and display movies


}

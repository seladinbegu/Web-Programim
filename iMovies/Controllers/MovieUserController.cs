using iMovies.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using iMovies.Data;
using iMovies.DTOs.MovieUser;
using System.Security.Claims;
using iMovies.Controllers.Mappers;

namespace iMovies.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieUserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MovieUserController(ApplicationDbContext context)
        {
            _context = context;
        }




        [HttpGet]
        public IActionResult Get()
        {
            var movieuser = _context.MovieUsers.ToList().Select(s => s.toMovieUserDto());
            return Ok(movieuser);
        }


        [HttpGet("{movieId}/{userId}")]
        public IActionResult Get(int movieId, string userId)
        {
            // Fetch the data based on the provided movieId and userId
            var movieuser = _context.MovieUsers
                                    .Where(mu => mu.MovieId == movieId && mu.UserId == userId)
                                    .Select(s => s.toMovieUserDto())  // Assuming ToMovieUserDto is a method to map to the DTO
                                    .FirstOrDefault();  // Assuming you expect only one result for each combination of movieId and userId

            if (movieuser == null)
            {
                return NotFound("MovieUser not found for the provided movieId and userId.");
            }

            return Ok(movieuser);
        }

        // GET: api/MovieUser/user-favorites/{userId}
        [HttpGet("user-favorites/{userId}")]
        public async Task<IActionResult> GetUserFavorites(string userId)
        {
            try
            {
                // Fetch the list of favorite movies for the provided userId
                var userFavorites = await _context.MovieUsers
                    .Where(mu => mu.UserId == userId)
                    .Include(mu => mu.Movie)  // Include movie details
                    .Select(mu => new
                    {
                        mu.Movie.Id,
                        mu.Movie.Name,
                        mu.Movie.Genre,
                        mu.Movie.Duration,
                        mu.Movie.ImgPath
                    })
                    .ToListAsync();

                if (userFavorites == null || !userFavorites.Any())
                {
                    return NotFound(new { message = "No favorite movies found for this user" });
                }

                return Ok(userFavorites);
            }
            catch (Exception ex)
            {
                // Log the error
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }






        [HttpPost("add-to-favorites")]
        public async Task<IActionResult> AddToFavorites([FromBody] MovieUserDto movieUserDto)
        {
            try
            {
                if (movieUserDto == null)
                {
                    return BadRequest(new { message = "Invalid request body" });
                }

                // Check if the combination of MovieId and UserId already exists
                var existingMovieUser = await _context.MovieUsers
                                                       .FirstOrDefaultAsync(mu => mu.MovieId == movieUserDto.MovieId && mu.UserId == movieUserDto.UserId);

                if (existingMovieUser != null)
                {
                    // If the movie is already in the favorites, return a conflict response
                    return Ok(new { message = "Movie is already in favorites" });
                }

                // If no existing record found, proceed to add the new movie to favorites
                var movieUser = new MovieUser
                {
                    MovieId = movieUserDto.MovieId,
                    UserId = movieUserDto.UserId
                };

                _context.MovieUsers.Add(movieUser);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Movie added to favorites" });
            }
            catch (Exception ex)
            {
                // Log the error (you can use a logger here)
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, new { message = "An error occurred while adding to favorites." });
            }
        }




        // DELETE: api/MovieUser/remove-favorite
        [HttpDelete("remove-favorite/{movieId}/{userId}")]
        public async Task<IActionResult> RemoveFavorite(int movieId, string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                // Log or handle error when userId is missing or empty
                return Unauthorized("User ID is required.");
            }

            // Log the values for debugging
            Console.WriteLine($"Received movieId: {movieId}, userId: {userId}");

            var existingFavorite = await _context.MovieUsers
                .FirstOrDefaultAsync(m => m.UserId == userId && m.MovieId == movieId);

            if (existingFavorite == null)
            {
                return NotFound("Movie not found in favorites.");
            }

            _context.MovieUsers.Remove(existingFavorite);
            await _context.SaveChangesAsync();

            return Ok("Movie removed from favorites.");
        }



        // GET: api/MovieUser/user-favorites/{userId}


    }
}

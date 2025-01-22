using System.Linq;
using Microsoft.AspNetCore.Mvc;
using iMovies.Data;
using iMovies.Models.DTOs.Movie;
using iMovies.Controllers.Mappers;
using Microsoft.AspNetCore.Authorization;

namespace iMovies.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MovieController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET all movies
        [HttpGet]

        public IActionResult Get()
        {
            var movie = _context.Movies.ToList().Select(s => s.toMovieDto());
            return Ok(movie);
        }

        // GET movie by ID
        [HttpGet("{id}")]

        public IActionResult GetById(int id)
        {
            var movie = _context.Movies.FirstOrDefault(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }
            return Ok(movie.toMovieDto());
        }



        [HttpGet("title/{title}")]

        public IActionResult GetByTitle(string title)
        {
            var movie = _context.Movies.FirstOrDefault(m => m.Name.Equals(title, System.StringComparison.OrdinalIgnoreCase));
            if (movie == null)
            {
                return NotFound();
            }
            return Ok(new { movieId = movie.Id });
        }


        // POST create a new movie
        [HttpPost]
        [Authorize(Roles = "Admin")]  // Restrict this action to admin users
        public IActionResult Create([FromBody] MovieCreateDto movieDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);  // Return detailed error messages if model is invalid
            }

            // Map DTO to model entity
            var movieModel = movieDto.toMovieFromCreateDto();
            _context.Movies.Add(movieModel);
            _context.SaveChanges();

            // Return 201 Created response with the newly created movie data
            return CreatedAtAction(nameof(GetById), new { id = movieModel.Id }, movieModel.toMovieDto());
        }

        // PUT update movie by ID
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]  // Restrict this action to admin users

        public IActionResult Update([FromRoute] int id, [FromBody] MovieUpdateDto updateDto)
        {
            var movieModel = _context.Movies.FirstOrDefault(m => m.Id == id);
            if (movieModel == null)
            {
                return NotFound();
            }

            movieModel.Name = updateDto.Name;
            movieModel.Duration = updateDto.Duration;
            movieModel.ImgPath = updateDto.ImgPath;
            movieModel.Genre = updateDto.Genre;
            movieModel.ShowDate = updateDto.ShowDate;

            _context.SaveChanges();

            return Ok(movieModel.toMovieDto());
        }









        // DELETE movie by ID
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]  // Restrict this action to admin users
        public IActionResult Delete([FromRoute] int id)
        {
            var movieModel = _context.Movies.FirstOrDefault(m => m.Id == id);
            if (movieModel == null)
            {
                return NotFound();
            }
            _context.Movies.Remove(movieModel);
            _context.SaveChanges();

            return NoContent();
        }
    }
}

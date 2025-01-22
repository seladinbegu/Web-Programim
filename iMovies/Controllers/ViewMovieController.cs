using Microsoft.AspNetCore.Mvc;
using iMovies.Data;
using iMovies.Models.DTOs.Movie;

public class MoviesController : Controller
{
    private readonly ApplicationDbContext _context;

    public MoviesController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var movies = _context.Movies.ToList();
        var movieDtos = movies.Select(m => new MovieDto
        {
            Id = m.Id,
            Name = m.Name,
            Genre = m.Genre,
            Duration = m.Duration,
            ImgPath = m.ImgPath
        }).ToList();

        return View(movieDtos); // Will render Views/Movies/Index.cshtml
    }



}

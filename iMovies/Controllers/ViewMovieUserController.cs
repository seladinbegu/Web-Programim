using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace iMovies.Controllers
{
    [Route("[controller]")]
    public class ViewMovieUserController : Controller
    {
        private readonly ILogger<ViewMovieUserController> _logger;

        public ViewMovieUserController(ILogger<ViewMovieUserController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View("~/Views/Favorites/Index.cshtml");
        }


        [HttpGet("error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error");
        }
    }
}

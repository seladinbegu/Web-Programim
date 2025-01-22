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
    public class ViewAdminMovieController : Controller
    {
        private readonly ILogger<ViewActorController> _logger;

        public ViewAdminMovieController(ILogger<ViewActorController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View("~/Views/MovieAdmin/Index.cshtml");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpGet("error")]

        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
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
    public class ViewActorController : Controller
    {
        private readonly ILogger<ViewActorController> _logger;

        public ViewActorController(ILogger<ViewActorController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View("~/Views/Actor/Index.cshtml");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpGet("error")]

        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
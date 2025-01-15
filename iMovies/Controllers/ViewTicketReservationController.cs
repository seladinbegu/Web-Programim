using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace iMovies.Controllers
{
    [Route("ViewTicketReservation")] // Custom route for this controller
    public class ViewTicketReservationController : Controller
    {
        private readonly ILogger<ViewTicketReservationController> _logger;

        public ViewTicketReservationController(ILogger<ViewTicketReservationController> logger)
        {
            _logger = logger;
        }

        // GET: ViewTicketReservation/Index
        public IActionResult Index()
        {
            // Ensure this path is correct and points to an existing view.
            return View("~/Views/TicketReservation/Index.cshtml");
        }

        // GET: ViewTicketReservation/Error
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpGet("error")]
        public IActionResult Error()
        {
            return View("Error");
        }
    }
}

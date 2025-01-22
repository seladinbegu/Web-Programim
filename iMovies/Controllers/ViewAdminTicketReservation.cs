using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace iMovies.Controllers
{
    public class ViewAdminTicketReservation : Controller
    {
        private readonly ILogger<ViewAdminTicketReservation> _logger;

        public ViewAdminTicketReservation(ILogger<ViewAdminTicketReservation> logger)
        {
            _logger = logger;
        }

        [Route("admin/ticket-reservation/index")]
        public IActionResult Index()
        {
            return View("~/Views/TicketReservationAdmin/Index.cshtml");
        }

        [Route("admin/ticket-reservation/error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iMovies.DTOs.TicketReservation
{
    public class TicketReservationCreateDto
    {
        public string? UserId { get; set; }
        public int MovieId { get; set; }
        public DateTime ReservationDate { get; set; } = DateTime.Today;
        public DateTime ShowTime { get; set; }
        public double Amount { get; set; } = 4.20;
        public string? PaymentMethod { get; set; }


    }
}
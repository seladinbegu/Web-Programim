using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace iMovies.Models
{
    public class TicketReservation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Ensures auto-increment

        [Key]
        public int Id { get; set; }

        // Foreign key to User
        public string? UserId { get; set; }
        public virtual User? User { get; set; }

        // Foreign key to Movie
        public int MovieId { get; set; }
        public virtual Movie? Movie { get; set; }

        // Additional fields for the reservation, like Date and Time
        public DateTime ReservationDate { get; set; } = DateTime.Today;
        public DateTime ShowTime { get; set; }

        // Payment information (optional, for payment handling)
        public double Amount { get; set; } = 4.2;
        public string? PaymentMethod { get; set; }
    }
}


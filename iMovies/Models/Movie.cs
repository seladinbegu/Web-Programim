using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace iMovies.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Duration { get; set; }
        public string? ImgPath { get; set; }
        public string? Genre { get; set; }
        public DateTime? ShowDate { get; set; }

        public ICollection<MovieUser>? MovieUsers { get; set; }
        public ICollection<TicketReservation>? TicketReservations { get; set; }





    }
}
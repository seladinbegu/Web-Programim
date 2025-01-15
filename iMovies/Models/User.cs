using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace iMovies.Models
{
    public class User : IdentityUser
    {
        public ICollection<MovieUser>? MovieUsers { get; set; }
        public ICollection<TicketReservation>? TicketReservations { get; set; }


    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iMovies.DTOs.MovieUser
{
    public class MovieUserCreateDto
    {
        public int MovieId { get; set; }
        public string? UserId { get; set; }
    }
}
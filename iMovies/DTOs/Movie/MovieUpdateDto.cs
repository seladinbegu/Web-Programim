using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iMovies.Models.DTOs.Movie
{
    public class MovieUpdateDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Duration { get; set; }
        public string? ImgPath { get; set; }
        public string? Genre { get; set; }
        public DateTime? ShowDate { get; set; }

    }
}
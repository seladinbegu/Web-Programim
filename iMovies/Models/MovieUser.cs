using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace iMovies.Models
{
    public class MovieUser
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Ensures auto-increment
        public int Id { get; set; }

        public int MovieId { get; set; }

        public virtual Movie? Movie { get; set; }

        public string? UserId { get; set; }

        public virtual User? User { get; set; }
    }
}
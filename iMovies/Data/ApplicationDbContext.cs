using iMovies.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace iMovies.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions)
            : base(dbContextOptions)
        { }

        // DbSets for the entities
        public DbSet<RefreshToken> RefreshToken { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieUser> MovieUsers { get; set; }
        public DbSet<TicketReservation> TicketReservations { get; set; }



        // Configure the many-to-many relationship
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // TicketReservation configuration
            modelBuilder.Entity<TicketReservation>()
                .HasKey(tr => tr.Id);  // Primary key for TicketReservation

            modelBuilder.Entity<TicketReservation>()
                .HasOne(tr => tr.User)
                .WithMany(u => u.TicketReservations)
                .HasForeignKey(tr => tr.UserId);

            modelBuilder.Entity<TicketReservation>()
                .HasOne(tr => tr.Movie)
                .WithMany(m => m.TicketReservations)
                .HasForeignKey(tr => tr.MovieId);




            modelBuilder.Entity<MovieUser>()
                .HasKey(mu => new { mu.MovieId, mu.UserId });

            modelBuilder.Entity<MovieUser>()
                .HasOne(mu => mu.Movie)
                .WithMany(m => m.MovieUsers)
                .HasForeignKey(mu => mu.MovieId);

            modelBuilder.Entity<MovieUser>()
                .HasOne(mu => mu.User)
                .WithMany(u => u.MovieUsers)
                .HasForeignKey(mu => mu.UserId);


        }
    }
}

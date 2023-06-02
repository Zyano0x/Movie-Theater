using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Movie_Theater.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        public bool? IsEnabled { get; set; }
        public DateTime RegistrationDate { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Crew> Crews { get; set; }
        public DbSet<CRole> CRoles { get; set; }
        public DbSet<MovieGenre> MovieGenres { get; set; }
        public DbSet<MovieCrew> MovieCrews { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<MovieSchedule> MovieSchedules { get; set; }
        public DbSet<MovieTicket> MovieTickets { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomSeat> RoomSeats { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<Movie_Theater.ViewModels.CrewViewModel> CrewViewModels { get; set; }

        public System.Data.Entity.DbSet<Movie_Theater.ViewModels.MovieViewModel> MovieViewModels { get; set; }
    }
}
using CinemaProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CinemaProject.Dal
{
    public class MoviesDBMapping : DbContext
    {
        public MoviesDBMapping() : base("CinemaProject") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Hall>().ToTable("Hall");
            modelBuilder.Entity<Movie>().ToTable("Movie");
            modelBuilder.Entity<MovieScreen>().ToTable("MovieScreening");
            modelBuilder.Entity<Ticket>().ToTable("Ticket");
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<SeatReserved>().ToTable("SeatReserved");
        }

        public DbSet<Hall> Hall { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieScreen> MoviesList { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<SeatReserved> SeatsTaken { get; set; }

        public System.Data.Entity.DbSet<CinemaProject.ViewModels.LogInViewModel> LogInViewModels { get; set; }
    }
}
using CityInfo.API.DTOs;
using CityInfo.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Data
{
    public class CityContext : DbContext
    {
        public DbSet<City> Cities { get; set; }
        public DbSet<PointOfInterest> PointsOfInterest { get; set; }
        public CityContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>().HasData(
                new City
                {
                    Id = 1,
                    Name = "Cairo",
                    Description = "Has Pyramids",
                },
                new City
                {
                    Id = 2,
                    Name = "New York",
                    Description = "Has statue of Liberty",
                },
                new City
                {
                    Id = 3,
                    Name = "Paris",
                    Description = "Has Eiffel tower",
                }
            );

            modelBuilder.Entity<PointOfInterest>().HasData(
                new PointOfInterest
                {
                    CityId = 1,
                    Id = 1,
                    Name = "Centeral Park",
                    Description = "The most visited Park in Egypt"
                },
                new PointOfInterest
                {
                    CityId = 1,
                    Id = 2,
                    Name = "Aqua Park",
                    Description = "The most visited Park in Egypt"
                },
                new PointOfInterest
                {
                    CityId = 2,
                    Id = 3,
                    Name = "Centeral Park",
                    Description = "The most visited Park in Egypt"
                },
                new PointOfInterest
                {
                    Id = 4,
                    CityId = 2,
                    Name = "Aqua Park",
                    Description = "The most visited Park in Egypt"
                },
                new PointOfInterest
                {
                    Id = 5,
                    CityId = 3,
                    Name = "Centeral Park",
                    Description = "The most visited Park in Egypt"
                },
                new PointOfInterest
                {
                    Id = 6,
                    CityId = 3,
                    Name = "Aqua Park",
                    Description = "The most visited Park in Egypt"
                }
            );
            base.OnModelCreating(modelBuilder);
        }
    }
}

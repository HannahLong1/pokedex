using Microsoft.EntityFrameworkCore;
using FancySignup.Models;

namespace FancySignup.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Person> People { get; set; }
        public DbSet<Country> Countries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed 5 countries
            modelBuilder.Entity<Country>().HasData(
                new Country { CountryId = 1, Name = "USA" },
                new Country { CountryId = 2, Name = "Canada" },
                new Country { CountryId = 3, Name = "UK" },
                new Country { CountryId = 4, Name = "Germany" },
                new Country { CountryId = 5, Name = "Japan" }
            );
        }
    }
}

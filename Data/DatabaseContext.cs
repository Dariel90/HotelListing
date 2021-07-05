using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options): base(options)
        {
            
        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<Hotel> Hotels { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Country>().HasData(new Country
                {
                    Id = 1,
                    Name = "Cuba",
                    ShortName = "CU"
                }, new Country
                {
                    Id = 2,
                    Name = "United States of America",
                    ShortName = "USA"
                }, new Country
                {
                    Id = 3,
                    Name = "Spain",
                    ShortName = "ES"
                },
                new Country
                {
                    Id = 4,
                    Name = "Colombia",
                    ShortName = "COL"
                }
            );

            builder.Entity<Hotel>().HasData(new Hotel
                {
                    Id = 1,
                    Name = "Melia Habana",
                    Address = "Habana",
                    CountryId = 1,
                    Rating = 4.6
                }, new Hotel
                {
                    Id = 2,
                    Name = "Hotel Cagüama",
                    Address = "Matanzas",
                    CountryId = 2,
                    Rating = 4
                }, new Hotel
                {
                    Id = 3,
                    Name = "Hotel Viñales",
                    Address = "Pinar del Rio",
                    CountryId = 3,
                    Rating = 3
                },
                new Hotel
                {
                    Id = 4,
                    Name = "Pasacaballos",
                    Address = "Cienfuegos",
                    CountryId = 4,
                    Rating = 3.5
                }
            );
        }
    }
}

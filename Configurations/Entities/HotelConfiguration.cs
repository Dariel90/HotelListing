using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelListing.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListing.Configurations.Entities
{
    public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {
            builder.HasData(new Hotel
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

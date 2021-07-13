using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelListing.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListing.Configurations.Entities
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.HasData(new Country
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
        }
    }
}

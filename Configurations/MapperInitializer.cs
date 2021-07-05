using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HotelListing.Data;
using HotelListing.DTOs;

namespace HotelListing.Configurations
{
    public class MapperInitializer : Profile
    {
        public MapperInitializer()
        {
            CreateMap<Country, CountryDTO>();
            CreateMap<Country, CreateCountryDTO>();
            CreateMap<Hotel, HotelDTO>();
            CreateMap<Hotel, CreateHotelDTO>();
        }
    }
}

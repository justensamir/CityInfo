using AutoMapper;
using CityInfo.API.DTOs;
using CityInfo.API.Models;

namespace CityInfo.API.Profiles
{
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            CreateMap<City, CityDTO>();
            CreateMap<City, CityWithPointOfInterestDTO>();
        }
    }
}

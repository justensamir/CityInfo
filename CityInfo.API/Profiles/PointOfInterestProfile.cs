using AutoMapper;
using CityInfo.API.DTOs;
using CityInfo.API.Models;

namespace CityInfo.API.Profiles
{
    public class PointOfInterestProfile : Profile
    {
        public PointOfInterestProfile()
        {
            CreateMap<PointOfInterest, PointOfInterestDTO>();
            CreateMap<PointOfInterestCreationDTO, PointOfInterest>();
            CreateMap<PointOfInterest, PointOfInterestCreationDTO>();
        }
    }
}

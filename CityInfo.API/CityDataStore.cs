using CityInfo.API.DTOs;
using CityInfo.API.Models;

namespace CityInfo.API
{
    public class CityDataStore
    {
        public List<CityDTO> Cities { get; set; }
        
        public CityDataStore()
        {
            Cities = new()
            {
                new CityDTO { 
                    Id = 1, 
                    Name="Cairo", 
                    Description="Has Pyramids", 
                    PointsOfInterest = new List<PointsOfInterestDTO>(){
                        new PointsOfInterestDTO
                        {
                            Id = 1,
                            Name = "Centeral Park",
                            Description = "The most visited Park in Egypt"
                        },
                        new PointsOfInterestDTO
                        {
                            Id = 2,
                            Name = "Aqua Park",
                            Description = "The most visited Park in Egypt"
                        }
                    }
                },
                new CityDTO { 
                    Id = 2,
                    Name="New York", 
                    Description = "Has statue of Liberty",
                    PointsOfInterest = new List<PointsOfInterestDTO>(){
                        new PointsOfInterestDTO
                        {
                            Id = 3,
                            Name = "Centeral Park",
                            Description = "The most visited Park in Egypt"
                        },
                        new PointsOfInterestDTO
                        {
                            Id = 4,
                            Name = "Aqua Park",
                            Description = "The most visited Park in Egypt"
                        }
                    }
                },
                new CityDTO { 
                    Id = 3, 
                    Name="Paris", 
                    Description = "Has Eiffel tower",
                    PointsOfInterest = new List<PointsOfInterestDTO>(){
                        new PointsOfInterestDTO
                        {
                            Id = 5,
                            Name = "Centeral Park",
                            Description = "The most visited Park in Egypt"
                        },
                        new PointsOfInterestDTO
                        {
                            Id = 6,
                            Name = "Aqua Park",
                            Description = "The most visited Park in Egypt"
                        }
                    }
                },
            };
        }
    }
}

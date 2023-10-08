using CityInfo.API.Models;

namespace CityInfo.API.DTOs
{
    public class CityWithPointOfInterestDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public ICollection<PointOfInterestDTO> pointOfInterests { get; set; }
                    = new List<PointOfInterestDTO>();
    }
}

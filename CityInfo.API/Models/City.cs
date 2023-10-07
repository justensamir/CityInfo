using System.ComponentModel.DataAnnotations;

namespace CityInfo.API.Models
{
    public class City
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(200)]
        public string? Description { get; set; }
        public ICollection<PointOfInterest> pointOfInterests { get; set; }
                    = new List<PointOfInterest>();
    }
}

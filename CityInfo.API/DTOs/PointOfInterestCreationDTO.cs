using System.ComponentModel.DataAnnotations;

namespace CityInfo.API.DTOs
{
    public class PointOfInterestCreationDTO
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(100)]
        public string? Description { get; set; }
    }
}

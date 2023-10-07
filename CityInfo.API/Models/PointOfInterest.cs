using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace CityInfo.API.Models
{
    public class PointOfInterest
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int CityId { get; set; }
        public City? City { get; set; }
    }
}

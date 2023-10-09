using CityInfo.API.Models;

namespace CityInfo.API.DTOs
{
    public class CityDTO
    {
        
        /// <summary>
        /// Id of city
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Name of the city
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Description of the city
        /// </summary>
        public string? Description { get; set; }
    }
}

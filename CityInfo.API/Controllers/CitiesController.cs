using CityInfo.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly CityDataStore cityDataStore;
        public CitiesController(CityDataStore cityDataStore)
        {
            this.cityDataStore = cityDataStore ?? throw new ArgumentNullException(nameof(cityDataStore));
        }

        public CityDataStore CityDataStore { get; }

        [HttpGet]
        public IActionResult GetCities()
        {
            var cities = cityDataStore.Cities;
            return cities.Count == 0 ? NotFound() : Ok(cityDataStore.Cities);
        }

        [HttpGet("{id}")]
        public IActionResult GetCity(int id) 
        {
            var city = cityDataStore.Cities.FirstOrDefault(c => c.Id == id);
            return city is null? NotFound() : Ok(city);
        }

        
    }
}

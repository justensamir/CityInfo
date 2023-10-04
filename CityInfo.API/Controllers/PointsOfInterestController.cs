using CityInfo.API.DTOs;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace CityInfo.API.Controllers
{
    [Route("api/cities/{cityId}/pointsofinterest")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly ILogger<PointsOfInterestController> logger;
        private readonly IMailService mailService;
        private readonly CityDataStore cityDataStore;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger, IMailService mailService, CityDataStore cityDataStore)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(this.logger));
            this.mailService = mailService ?? throw new ArgumentNullException(nameof(this.logger));
            this.cityDataStore = cityDataStore ?? throw new ArgumentNullException(nameof(this.cityDataStore));
        }

        [HttpGet]
        public IActionResult GetPointsOfInterest(int cityId)
        {
            var city = cityDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                logger.LogInformation($"City with id ({cityId}) not found when accessing points of interest");
                return NotFound();
            }
            var pointsOfInterest = city.PointsOfInterest;

            return pointsOfInterest is null ? NotFound() :
                   pointsOfInterest.Count == 0 ? NotFound() : Ok(pointsOfInterest);
        }

        [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
        public IActionResult GetPointOfInterest(int cityId, int pointOfInterestId)
        {
            var city = cityDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
            if(city is null || city.PointsOfInterest.Count == 0) return NotFound();
            var pointOfInterest = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);
            return pointOfInterest is null? NotFound() : Ok(pointOfInterest);
        }

        [HttpPost]
        public IActionResult CreatePointOfInterest(int cityId, PointOfInterestCreationDTO pointOfInterest)
        {
            if (!ModelState.IsValid) return BadRequest();
            var city = cityDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
            if(city is null) return NotFound();
            IEnumerable<PointsOfInterestDTO> po = cityDataStore.Cities.SelectMany(c => c.PointsOfInterest);
            int maxId = po.Max(p => p.Id);
            var finalInterestPoint = new PointsOfInterestDTO()
            {
                Id = ++maxId,
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description,
            };
            city.PointsOfInterest.Add(finalInterestPoint);

            return CreatedAtRoute("GetPointOfInterest", new { cityId, pointOfInterestId = finalInterestPoint.Id }, finalInterestPoint);
        }

        [HttpPut("{pointOfInterestId}")]
        public IActionResult UpdatePointOfInterest(int cityId, int pointOfInterestId, PointOfInterestCreationDTO updatedPointsOfInterest)
        {
            var city = cityDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city is null) return NotFound();
            var pointOfInterest = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);
            if (pointOfInterest is null) return NotFound();
        
            pointOfInterest.Name = updatedPointsOfInterest.Name;
            pointOfInterest.Description = updatedPointsOfInterest.Description;
            return NoContent();
        }

        [HttpDelete("{pointOfInterestId}")]
        public IActionResult DeletePointOfInterest(int cityId, int pointOfInterestId)
        {
            var city = cityDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city is null) return NotFound();
            var pointOfInterest = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);
            if (pointOfInterest is null) return NotFound();
            city.PointsOfInterest.Remove(pointOfInterest);
            mailService.Send("Interest Point Deleted", $"Interest point with id ({pointOfInterestId}) of City with id ({cityId}) has been deleted.");
            return NoContent();
        }

    }
}

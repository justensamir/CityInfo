using AutoMapper;
using CityInfo.API.DTOs;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace CityInfo.API.Controllers
{
    [Route("api/cities/{cityId}/pointsofinterest")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly ILogger<PointsOfInterestController> logger;
        private readonly IMailService mailService;
        private readonly ICityInfoRepository cityInfoRepository;
        private readonly IMapper mapper;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger, 
            IMailService mailService, 
            ICityInfoRepository cityInfoRepository,
            IMapper mapper)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(this.logger));
            this.mailService = mailService ?? throw new ArgumentNullException(nameof(this.logger));
            this.cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(this.cityInfoRepository));
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetPointsOfInterest(int cityId)
        {
            var city = await cityInfoRepository.IsCityExistsAsync(cityId);
            
            if (!city) 
            {
                logger.LogInformation($"City with id ({cityId}) wasn't found when accessing Point of interest");
                return NotFound(); 
            }

            var pointsOfInterest = await cityInfoRepository.GetPointsOfInterestForCityAsync(cityId);
            if (pointsOfInterest.Count() == 0) return NotFound();
            
            return Ok(mapper.Map<IEnumerable<PointOfInterestDTO>>(pointsOfInterest));
        }

        [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
        public async Task<IActionResult> GetPointOfInterest(int cityId, int pointOfInterestId)
        {
            var city = await cityInfoRepository.IsCityExistsAsync(cityId);
            if (!city)
            {
                logger.LogInformation($"City with id ({cityId}) wasn't found when accessing Point of interest");
                return NotFound();
            }

            var pointOfInterest = await cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);
            if(pointOfInterest == null) return NotFound();

            return Ok(mapper.Map<PointOfInterestDTO>(pointOfInterest));
        }

        [HttpPost]
        public async Task<IActionResult> CreatePointOfInterest(int cityId, PointOfInterestCreationDTO pointOfInterest)
        {
            if (!ModelState.IsValid) return BadRequest();
            
            if (!await cityInfoRepository.IsCityExistsAsync(cityId)) return NotFound();


            var finalInterestPoint = mapper.Map<PointOfInterest>(pointOfInterest);
            
            await cityInfoRepository.AddPointOfInterestForCityAsync(cityId, finalInterestPoint);

            await cityInfoRepository.SaveChangesAsync();

            var pointOfInterestDTO = mapper.Map<PointOfInterestDTO>(finalInterestPoint);

            return CreatedAtRoute("GetPointOfInterest", new { cityId, pointOfInterestId = pointOfInterestDTO.Id }, pointOfInterestDTO);
        }

        [HttpPut("{pointOfInterestId}")]
        public async Task<IActionResult> UpdatePointOfInterest(int cityId, int pointOfInterestId, PointOfInterestCreationDTO updatedPointsOfInterest)
        {
            if (!await cityInfoRepository.IsCityExistsAsync(cityId)) return NotFound();

            var pointOfInterest = await cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);
            
            if (pointOfInterest == null) return NotFound();
            
            mapper.Map(updatedPointsOfInterest, pointOfInterest);

            await cityInfoRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{pointOfInterestId}")]
        public async Task<IActionResult> PartiallyUpdatePointOfInterest(int cityId, 
            int pointOfInterestId, 
            JsonPatchDocument<PointOfInterestCreationDTO> updatedPointOfInterest)
        {
            if (!await cityInfoRepository.IsCityExistsAsync(cityId)) return NotFound();

            var pointOfInterest = await cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);
            
            if(pointOfInterest == null) return NotFound();

            var pointOfInterestToPatch = new PointOfInterestCreationDTO
            {
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description,
            };

            updatedPointOfInterest.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            mapper.Map(pointOfInterestToPatch, pointOfInterest);

            await cityInfoRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{pointOfInterestId}")]
        public async Task<IActionResult> DeletePointOfInterest(int cityId, int pointOfInterestId)
        {
            if (!await cityInfoRepository.IsCityExistsAsync(cityId)) return NotFound();

            var pointOfInterest = await cityInfoRepository.GetPointOfInterestForCityAsync (cityId, pointOfInterestId);
            if (pointOfInterest == null) return NotFound();

            cityInfoRepository.DeletePointOfInterest(pointOfInterest);
            await cityInfoRepository.SaveChangesAsync();

            mailService.Send("Interest Point Deleted", $"Interest point with id ({pointOfInterest.Id}) of City with id ({cityId}) has been deleted.");
            return NoContent();
        }

    }
}

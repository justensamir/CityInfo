using AutoMapper;
using CityInfo.API.DTOs;
using CityInfo.API.Models;
using CityInfo.API.Repositories;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
using System.Xml.Linq;

namespace CityInfo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly ICityInfoRepository cityInfoRepository;
        private readonly IMapper mapper;
        const int MAX_PAGE_SIZE = 20;
        const int MIN_PAGE_SIZE = 1;
        public CitiesController(ICityInfoRepository cityInfoRepository, IMapper mapper)
        {
            this.cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetCities(string? name, 
                    string? searchQuery,
                    int pageNumber = 1, int pageSize = 10)
        {
            if (pageSize > MAX_PAGE_SIZE) { pageSize = MAX_PAGE_SIZE; }
            if (pageSize < MIN_PAGE_SIZE) { pageSize = MIN_PAGE_SIZE; }

            var (cities, paginatioMetaData) = await cityInfoRepository.GetCitiesAsync(name, 
                                                                                      searchQuery, 
                                                                                      pageNumber, 
                                                                                      pageSize);


            if (cities.Count() == 0) return NotFound();

            Response.Headers.Add("x-Pagination", JsonSerializer.Serialize(paginatioMetaData));

            return Ok(mapper.Map<IEnumerable<CityDTO>>(cities));
        }
        
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCity(int id, bool includePointOfInterest = false) 
        {
            var city = await cityInfoRepository.GetCityAsync(id, includePointOfInterest); 
            if(city == null) return NotFound();
            if (includePointOfInterest) return Ok(mapper.Map<CityWithPointOfInterestDTO>(city));
            return Ok(mapper.Map<CityDTO>(city));
        }

        
    }
}

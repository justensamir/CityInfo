using CityInfo.API.Data;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Repositories
{
    public class CityInfoRepository : ICityInfoRepository
    {
        private readonly CityContext context;
        public CityInfoRepository(CityContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<City>> GetCitiesAsync()
        {
            return await context.Cities.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<City?> GetCityAsync(int cityId, bool includePointsOfInterest)
        {
            if (includePointsOfInterest)
            {
                return await context.Cities.Include(c => c.pointOfInterests).FirstOrDefaultAsync(c => c.Id == cityId);
            }
            return await context.Cities.FirstOrDefaultAsync(c => c.Id == cityId);
        }

        public async Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId, int pointOfInterestId)
        {
            return await context.PointsOfInterest.FirstOrDefaultAsync(p => p.CityId == cityId && p.Id == pointOfInterestId);
        }

        public async Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCityAsync(int cityId)
        {
            return await context.PointsOfInterest.Where(p => p.CityId == cityId).ToListAsync();
        }
    }
}

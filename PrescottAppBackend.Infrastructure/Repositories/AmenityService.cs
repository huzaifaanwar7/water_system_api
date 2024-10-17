using Microsoft.EntityFrameworkCore;
using PrescottAppBackend.Domain;
using PrescottAppBackend.Domain.DbModels;

namespace PrescottAppBackend.Infrastructure
{
    public class AmenityService : IAmenityService
    {
        private readonly PrescottContext _dbContext;

        public AmenityService(PrescottContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<List<AmenityVM>> GetAllAmenitiesAsync() {
            var list = await (from a in _dbContext.Amenities
                                join b in _dbContext.Buildings on a.BuildingId equals b.Id
                                join u in _dbContext.Users on a.CreatedBy equals u.Id
                                select new AmenityVM() {
                                    Id = a.Id,
                                    BuildingId = a.BuildingId,
                                    AmenityName = a.AmenityName,
                                    Description = a.Description,
                                    CreatedBy = a.CreatedBy,
                                    CreatedAt = a.CreatedAt,
                                    UpdatedBy = a.UpdatedBy,
                                    UpdatedAt = a.UpdatedAt,
                                    BuildingName = b.BuildingName,
                                    CreatedByStr = (u.FirstName + ' ' + u.LastName).ToString(),
                                }).ToListAsync();
            return list;
        }
        public async Task<Amenity> GetAmenityByIdAsync(int amenityId) {
            return await _dbContext.Amenities.FirstOrDefaultAsync(a => a.Id == amenityId);
        }
        // public async Task<Amenity> AddUpdateAmenityAsync(AmenityVM amenity) {

        // }
        // public async Task DeleteAmenityAsync(int amenityId) {

        // }

    }
}
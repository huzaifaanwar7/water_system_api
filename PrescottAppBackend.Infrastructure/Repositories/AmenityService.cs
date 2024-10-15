using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        // public async Task<List<AmenityVM>> GetAllAmenitiesAsync() {

        // }
        // public async Task<Reservation> GetAmenityByIdAsync(int amenityId) {

        // }
        // public async Task<Amenity> AddUpdateAmenityAsync(AmenityVM amenity) {

        // }
        // public async Task DeleteAmenityAsync(int amenityId) {

        // }

    }
}
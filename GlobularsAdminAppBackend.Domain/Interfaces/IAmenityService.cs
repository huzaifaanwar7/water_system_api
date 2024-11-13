using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlobularsAdminAppBackend.Domain.DbModels;
using GlobularsAdminAppBackend.Domain;


namespace GlobularsAdminAppBackend.Domain
{
    public interface IAmenityService
    {
        Task<List<AmenityVM>> GetAllAmenitiesAsync();
        Task<AmenityVM> GetAmenityByIdAsync(int amenityId);
        Task<AmenityVM> AddUpdateAmenityAsync(AmenityVM amenity);
        Task DeleteAmenityAsync(int amenityId);

        Task<List<AmenityVM>> GetAmenityByBuildingId(int buildingId);
    }
}
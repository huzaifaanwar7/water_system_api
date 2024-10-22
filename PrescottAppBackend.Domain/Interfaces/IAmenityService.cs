using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrescottAppBackend.Domain.DbModels;
using PrescottAppBackend.Domain;


namespace PrescottAppBackend.Domain
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
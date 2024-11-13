using System;
using GlobularsAdminAppBackend.Domain.DbModels;

namespace GlobularsAdminAppBackend.Domain
{

    public interface IBuildingService
    {
        Task<List<Building>> GetAllBuildingsAsync();
        Task<Building> GetBuildingByIdAsync(int buildingId);
        Task<Building> GetBuildingByBuildingnameAsync(string buildingname);
        Task<Building> AddBuildingAsync(BuildingVM building);
        Task UpdateBuildingAsync(Building building);
        Task DeleteBuildingAsync(int buildingId);
    }
}
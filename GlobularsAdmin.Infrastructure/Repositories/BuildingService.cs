
using Microsoft.EntityFrameworkCore;
using GlobularsAdmin.Domain;
using GlobularsAdmin.Domain.DbModels;

namespace GlobularsAdmin.Infrastructure
{
    public class BuildingService : IBuildingService
    {
        private readonly PrescottContext _dbContext;

        public BuildingService(PrescottContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<List<Building>> GetAllBuildingsAsync()
        {
            var bd = await _dbContext.Buildings.ToListAsync();
            return bd;
        }

        public async Task<Building> GetBuildingByIdAsync(int buildingId)
        {
            return await _dbContext.Buildings.FindAsync(buildingId);
        }

        public async Task<Building> GetBuildingByBuildingnameAsync(string buildingname)
        {
            return await _dbContext.Buildings.FirstOrDefaultAsync(u => u.BuildingName == buildingname);
        }

        public async Task<Building> AddBuildingAsync(BuildingVM buildingVM)
        {
            Building building = new Building() {
                BuildingName = buildingVM.BuildingName,
                BuildingDescription = buildingVM.BuildingDescription,
                Address = buildingVM.Address,
                CreatedBy = "",
                CreatedAt = DateTime.Now,
                IsDeleted = false
            };

            await _dbContext.Buildings.AddAsync(building);
            await _dbContext.SaveChangesAsync();
            return building;
        }

        public async Task UpdateBuildingAsync(Building building)
        {
            _dbContext.Buildings.Update(building);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteBuildingAsync(int buildingId)
        {
            var building = await _dbContext.Buildings.FindAsync(buildingId);
            if (building != null)
            {
                _dbContext.Buildings.Remove(building);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}

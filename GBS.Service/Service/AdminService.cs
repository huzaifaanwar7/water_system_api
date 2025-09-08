
using Microsoft.EntityFrameworkCore;
using GBS.Entities;
using GBS.Entities.DbModels;
using GBS.Data.Model;

namespace GBS.Service
{
    public interface IAdminService
    {
        #region All Lookups
        Task<LookupsVM> GetAllLookups();
        #endregion
        #region Status
        Task<List<Status>> GetStatusList();
        #endregion

        #region User Role
        Task<List<UserRole>> GetUserRoleList();
        #endregion

        #region Job Role
        Task<List<JobRole>> GetJobRoleList();
        #endregion

        #region Tech Stack
        Task<List<TechStack>> GetTechStackList();
        #endregion

    }
    public class AdminService(GBS_DbContext _dbContext) : IAdminService
    {
        #region All Lookups
        public async Task<LookupsVM> GetAllLookups()
        {
            return new LookupsVM
            {
                Status = await _dbContext.Statuses.Where(x => x.IsActive).ToListAsync(),
                UserRole = await _dbContext.UserRoles.Where(x => x.IsActive).ToListAsync(),
                JobRole = await _dbContext.JobRoles.Where(x => x.IsActive).ToListAsync(),
                TechStack = await _dbContext.TechStacks.Where(x => x.IsActive).ToListAsync()
            };
        }
        #endregion

        #region Status
        public async Task<List<Status>> GetStatusList()
        {
            return await _dbContext.Statuses.Where(x => x.IsActive).ToListAsync();
        }
        #endregion

        #region User Role
        public async Task<List<UserRole>> GetUserRoleList()
        {
            return await _dbContext.UserRoles.Where(x => x.IsActive).ToListAsync();
        }

        #endregion

        #region Job Role
        public async Task<List<JobRole>> GetJobRoleList()
        {
            return await _dbContext.JobRoles.Where(x => x.IsActive).ToListAsync();

        }

        #endregion

        #region Tech Stack
        public async Task<List<TechStack>> GetTechStackList()
        {
            return await _dbContext.TechStacks.Where(x => x.IsActive).ToListAsync();

        }


        #endregion

    }
}

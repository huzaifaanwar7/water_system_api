
using Microsoft.EntityFrameworkCore;
using GBS.Entities;
using GBS.Entities.DbModels;
using GBS.Data.Model;
using GBS.Service.Helpers;
using GBS.Data.Enums;

namespace GBS.Service
{
    public interface IAdminService
    {
        #region All Lookups
        Task<LookupsVM> GetAllLookups();
        #endregion
        #region Status
        Task<List<Lookup>> GetStatusList();
        #endregion

        #region User Role
        Task<List<Lookup>> GetUserRoleList();
        #endregion

        #region Job Role
        Task<List<Lookup>> GetJobRoleList();
        #endregion

        #region Tech Stack
        Task<List<Lookup>> GetTechStackList();
        #endregion

    }
    public class AdminService(GBS_DbContext _dbContext) : IAdminService
    {
        #region All Lookups
        public async Task<LookupsVM> GetAllLookups()
        {
            return new LookupsVM
            {
                EmployeeStatus = await _dbContext.Lookups.Where(x => x.IsActive && x.Type == LookupsTypeEnum.EmployeeStatus.GetStringValue()).ToListAsync(),
                UserRole = await _dbContext.Lookups.Where(x => x.IsActive && x.Type == LookupsTypeEnum.UserRole.GetStringValue()).ToListAsync(),
                JobRole = await _dbContext.Lookups.Where(x => x.IsActive && x.Type == LookupsTypeEnum.JobRole.GetStringValue()).ToListAsync(),
                TechStack = await _dbContext.Lookups.Where(x => x.IsActive && x.Type == LookupsTypeEnum.TechStack.GetStringValue()).ToListAsync(),
                LedgerStatus = await _dbContext.Lookups.Where(x => x.IsActive && x.Type == LookupsTypeEnum.LedgerStatus.GetStringValue()).ToListAsync(),
                TransactionType = await _dbContext.Lookups.Where(x => x.IsActive && x.Type == LookupsTypeEnum.TransactionType.GetStringValue()).ToListAsync(),
            };
        }
        #endregion
        #region Employee Status
        public async Task<List<Lookup>> GetStatusList()
        {
            return await _dbContext.Lookups.Where(x => x.IsActive && x.Type == LookupsTypeEnum.EmployeeStatus.GetStringValue()).ToListAsync();
        }
        #endregion

        #region User Role
        public async Task<List<Lookup>> GetUserRoleList()
        {
            return await _dbContext.Lookups.Where(x => x.IsActive && x.Type == LookupsTypeEnum.UserRole.GetStringValue()).ToListAsync();
        }
        #endregion

        #region Job Role
        public async Task<List<Lookup>> GetJobRoleList()
        {
            return await _dbContext.Lookups.Where(x => x.IsActive && x.Type == LookupsTypeEnum.JobRole.GetStringValue()).ToListAsync();
        }
        #endregion

        #region Tech Stack
        public async Task<List<Lookup>> GetTechStackList()
        {
            return await _dbContext.Lookups.Where(x => x.IsActive && x.Type == LookupsTypeEnum.TechStack.GetStringValue()).ToListAsync();
        }
        #endregion

    }
}

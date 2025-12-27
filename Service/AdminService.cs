
using Microsoft.EntityFrameworkCore;

using GBS.Api.DbModels;
using GBS.Data.Model;
using GBS.Service.Helpers;
using GBS.Data.Enums;
using Microsoft.Data.SqlClient;

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
            var lookups = await _dbContext.Lookups
         .Where(x => x.IsActive)
         .Include(x => x.ParentIdFkNavigation)
         .ToListAsync();

            var allLookups = lookups.Select(x => new LookupVM()
            {
                Id = x.Id,
                Type = x.Type,
                Name = x.Name,
                SortOrder = x.SortOrder,
                ParentIdFk = x.ParentIdFk,
            });
            return new LookupsVM
            {
                EmployeeStatus = allLookups.Where(x => x.Type == LookupsTypeEnum.EmployeeStatus.GetStringValue()).ToList(),
                UserRole = allLookups.Where(x => x.Type == LookupsTypeEnum.UserRole.GetStringValue()).ToList(),
                JobRole = allLookups.Where(x => x.Type == LookupsTypeEnum.JobRole.GetStringValue()).ToList(),
                TechStack = allLookups.Where(x => x.Type == LookupsTypeEnum.TechStack.GetStringValue()).ToList(),
                LedgerStatus = allLookups.Where(x => x.Type == LookupsTypeEnum.LedgerStatus.GetStringValue()).ToList(),
                TransactionType = allLookups.Where(x => x.Type == LookupsTypeEnum.TransactionType.GetStringValue()).ToList(),
                CostCategory = allLookups.Where(x => x.Type == LookupsTypeEnum.CostCategory.GetStringValue()).ToList(),
                MaterialType = allLookups.Where(x => x.Type == LookupsTypeEnum.MaterialType.GetStringValue()).ToList(),
                MaterialUnit = allLookups.Where(x => x.Type == LookupsTypeEnum.MaterialUnit.GetStringValue()).ToList(),
                OrderStatus = allLookups.Where(x => x.Type == LookupsTypeEnum.OrderStatus.GetStringValue()).ToList(),
                PaymentMethod = allLookups.Where(x => x.Type == LookupsTypeEnum.PaymentMethod.GetStringValue()).ToList(),
                ProductCategory = allLookups.Where(x => x.Type == LookupsTypeEnum.ProductCategory.GetStringValue()).ToList(),
                ProductSize = allLookups.Where(x => x.Type == LookupsTypeEnum.ProductSize.GetStringValue()).ToList(),
                SalaryChangeType = allLookups.Where(x => x.Type == LookupsTypeEnum.SalaryChangeType.GetStringValue()).ToList()
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

using System;
using GlobularsAdminAppBackend.Domain.DbModels;

namespace GlobularsAdminAppBackend.Domain
{

    public interface IDDLService
    {
        Task<List<Dropdownlistchild>> GetDropdownListByTypeAsync(string ddlType);
    }
}
using System;
using GlobularsAdmin.Domain.DbModels;

namespace GlobularsAdmin.Domain
{

    public interface IDDLService
    {
        Task<List<Dropdownlistchild>> GetDropdownListByTypeAsync(string ddlType);
    }
}
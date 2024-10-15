using System;
using PrescottAppBackend.Domain.DbModels;

namespace PrescottAppBackend.Domain
{

    public interface IDDLService
    {
        Task<List<Dropdownlistchild>> GetDropdownListByTypeAsync(string ddlType);
    }
}
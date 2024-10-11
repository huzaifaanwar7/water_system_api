using System;
using PrescottAppBackend.Domain.DbModels;

namespace PrescottAppBackend.Domain
{

    public interface IRoleService
    {
        Task<List<Role>> GetAllRolesAsync();
        Task<Role> GetRoleByIdAsync(string roleId);
        Task<Role> GetRoleByRolenameAsync(string rolename);
        Task<Role> AddRoleAsync(RoleVM role);
        Task UpdateRoleAsync(Role role);
        Task DeleteRoleAsync(string roleId);
    }
}
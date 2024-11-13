
using Microsoft.EntityFrameworkCore;
using GlobularsAdminAppBackend.Domain;
using GlobularsAdminAppBackend.Domain.DbModels;

namespace GlobularsAdminAppBackend.Infrastructure
{
    public class RoleService : IRoleService
    {
        private readonly PrescottContext _dbContext;

        public RoleService(PrescottContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Role> GetRoleByIdAsync(string roleId)
        {
            return await _dbContext.Roles.FindAsync(roleId);
        }

        public async Task<Role> GetRoleByRolenameAsync(string rolename)
        {
            return await _dbContext.Roles.FirstOrDefaultAsync(u => u.RoleName == rolename);
        }

        public async Task<Role> AddRoleAsync(RoleVM roleVM)
        {
            Role role = new Role(){
                Id = Guid.NewGuid().ToString(),
                RoleName = roleVM.RoleName,
                RoleDescription = roleVM.RoleDescription,
                CreatedBy = "",
                CreatedAt = DateTime.Now,
                IsDeleted = false
            };

            await _dbContext.Roles.AddAsync(role);
            await _dbContext.SaveChangesAsync();
            return role;
        }

        public async Task UpdateRoleAsync(Role role)
        {
            _dbContext.Roles.Update(role);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteRoleAsync(string roleId)
        {
            var role = await _dbContext.Roles.FindAsync(roleId);
            if (role != null)
            {
                _dbContext.Roles.Remove(role);
                await _dbContext.SaveChangesAsync();
            }
        }

        // Other methods as needed

        public async Task<List<Role>> GetAllRolesAsync()
        {
            return await _dbContext.Roles.ToListAsync();
        }
    }
}

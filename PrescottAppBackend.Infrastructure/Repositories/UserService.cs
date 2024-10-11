
using Microsoft.EntityFrameworkCore;
using PrescottAppBackend.Domain;
using PrescottAppBackend.Domain.DbModels;

namespace PrescottAppBackend.Infrastructure
{
    public class UserService : IUserService
    {
        private readonly PrescottContext _dbContext;
        private readonly IRoleService _roleService;

        public UserService(PrescottContext dbContext, IRoleService roleService)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _roleService = roleService;
        }

        public async Task<User> ValidateUserAsync(UserVM userVM)
        {
            if (userVM.UserSignUpType.Equals("Email"))
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == userVM.Email && x.Password == userVM.Password);
                return user;
            }
            else if (userVM.UserSignUpType.Equals("Google"))
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == userVM.Email && x.UserSignUpType == userVM.UserSignUpType);
                if (user == null)
                {
                    var role = await _roleService.GetRoleByRolenameAsync("Tenant");
                    userVM.Id = Guid.NewGuid().ToString();
                    userVM.RoleId = role.Id;
                    userVM.FirstName = userVM.DisplayName;
                    await this.AddUserAsync(userVM);
                    user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == userVM.Email && x.UserSignUpType == userVM.UserSignUpType);
                }
                return user;
            }
            else
            {
                return null;
            }
        }

        public async Task<User> GetUserByIdAsync(string userId)
        {
            return await _dbContext.Users.FindAsync(userId) ?? new User();
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == username) ?? new User();
        }

        public async Task AddUserAsync(UserVM user)
        {
            var newUser = new User()
            {
                Id = user.Id ?? "",
                FirstName = user.FirstName ?? "",
                LastName = "",
                RoleId = user.RoleId ?? "",
                BuildingId = user.BuildingId,
                Email = user.Email ?? "",
                EmailVerified = true,
                Password = user.Password ?? "",
                FirebaseId = user.FirebaseId ?? "",
                PhotoUrl = user.PhotoUrl ?? "",
                CreatedBy = "7bbb8f39-853d-419c-9821-4a9df220a805",
                CreatedAt = DateTime.Now,
                IsActive = true,
                IsDeleted = false,
                UserSignUpType = user.UserSignUpType ?? ""
            };

            await _dbContext.Users.AddAsync(newUser);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(string userId)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            if (user != null)
            {
                _dbContext.Users.Remove(user);
                await _dbContext.SaveChangesAsync();
            }
        }

        // Other methods as needed

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _dbContext.Users.ToListAsync();
        }
    }
}

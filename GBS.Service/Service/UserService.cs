
using Microsoft.EntityFrameworkCore;
using GBS.Entities;
using GBS.Entities.DbModels;
using GBS.Data.Model;

namespace GBS.Service
{
    public interface IUserService
    {
        Task<User> ValidateUserAsync(AuthVM auth);
    }
    public class UserService(GBS_DbContext _dbContext) : IUserService
    {


        public async Task<User> ValidateUserAsync(AuthVM auth)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserName == auth.Username && x.Password == auth.Password);
            return user;
        }

        // public async Task<User> GetUserByIdAsync(string userId)
        // {
        //     return await _dbContext.Users.FindAsync(userId) ?? new User();
        // }

        // public async Task<User> GetUserByUsernameAsync(string username)
        // {
        //     return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == username) ?? new User();
        // }

        // public async Task AddUserAsync(UserVM user)
        // {
        //     var newUser = new User()
        //     {
        //         Id = user.Id ?? "",
        //         FirstName = user.FirstName ?? "",
        //         LastName = "",
        //         RoleId = user.RoleId ?? "",
        //         BuildingId = user.BuildingId,
        //         Email = user.Email ?? "",
        //         EmailVerified = true,
        //         Password = user.Password ?? "",
        //         FirebaseId = user.FirebaseId ?? "",
        //         PhotoUrl = user.PhotoUrl ?? "",
        //         CreatedBy = "7bbb8f39-853d-419c-9821-4a9df220a805",
        //         CreatedAt = DateTime.Now,
        //         IsActive = true,
        //         IsDeleted = false,
        //         UserSignUpType = user.UserSignUpType ?? ""
        //     };

        //     await _dbContext.Users.AddAsync(newUser);
        //     await _dbContext.SaveChangesAsync();
        // }

        // public async Task UpdateUserAsync(User user)
        // {
        //     _dbContext.Users.Update(user);
        //     await _dbContext.SaveChangesAsync();
        // }

        // public async Task DeleteUserAsync(string userId)
        // {
        //     var user = await _dbContext.Users.FindAsync(userId);
        //     if (user != null)
        //     {
        //         _dbContext.Users.Remove(user);
        //         await _dbContext.SaveChangesAsync();
        //     }
        // }

        // // Other methods as needed

        // public async Task<List<User>> GetAllUsersAsync()
        // {
        //     return await _dbContext.Users.ToListAsync();
        // }
    }
}

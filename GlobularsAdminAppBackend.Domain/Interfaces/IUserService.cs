using System;
using GlobularsAdminAppBackend.Domain.DbModels;

namespace GlobularsAdminAppBackend.Domain
{

    public interface IUserService
    {
        Task<User> ValidateUserAsync(UserVM userVM);
        Task<User> GetUserByIdAsync(string userId);
        Task<User> GetUserByUsernameAsync(string username);
        Task AddUserAsync(UserVM user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(string userId);
    }
}
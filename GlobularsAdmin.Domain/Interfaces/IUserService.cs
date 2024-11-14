using System;
using GlobularsAdmin.Domain.DbModels;

namespace GlobularsAdmin.Domain
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
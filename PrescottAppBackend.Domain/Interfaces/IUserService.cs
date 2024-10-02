using System;
using PrescottAppBackend.Domain.DbModels;

namespace PrescottAppBackend.Domain
{

    public interface IUserService
    {
        Task<User> GetUserByIdAsync(string userId);
        Task<User> GetUserByUsernameAsync(string username);
        Task AddUserAsync(UserVM user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(string userId);
    }
}
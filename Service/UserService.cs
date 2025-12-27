
using Microsoft.EntityFrameworkCore;

using GBS.Api.DbModels;
using GBS.Model;

namespace GBS.Service
{
    public interface IUserService
    {
        Task<Employee> ValidateUserAsync(AuthVM auth);
    }
    public class UserService(GBS_DbContext _dbContext) : IUserService
    {


        public async Task<Employee> ValidateUserAsync(AuthVM auth)
        {
            var user = await _dbContext.Employees.FirstOrDefaultAsync(x => x.Username == auth.Username && x.Password == auth.Password);
            return user;
        }
    }
}

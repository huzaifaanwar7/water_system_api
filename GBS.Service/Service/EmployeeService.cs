
using Microsoft.EntityFrameworkCore;
using GBS.Entities;
using GBS.Entities.DbModels;
using GBS.Data.Model;

namespace GBS.Service
{
    public interface IEmployeeService
    {
        Task<List<Employee>> GetEmployeeList();
        Task<Employee> GetEmployeeById(int Id);
        Task<Employee> GetUserByUsernameAsync(string username);
        Task<int> UpdateEmployee(Employee user);
        Task<int> DeleteEmployee(string Id);

    }
    public class EmployeeService(GBS_DbContext _dbContext) : IEmployeeService
    {


        public async Task<List<Employee>> GetEmployeeList()
        {
            var users = await _dbContext.Employees.Where(e => e.IsActive).ToListAsync();
            return users;
        }

        public async Task<Employee> GetEmployeeById(int Id)
        {
            return await _dbContext.Employees.Where(u => u.Id == Id)
                .Include(e => e.EmployeeJobRoles)
                .Include(e => e.EmployeeTechStacks)
                .Include(e => e.EmployeeBankDetails)
                .FirstOrDefaultAsync();
        }

        public async Task<Employee> GetUserByUsernameAsync(string username)
        {
            return await _dbContext.Employees.FirstOrDefaultAsync(u => u.Username == username);
        }

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

        public async Task<int> UpdateEmployee(Employee user)
        {
            _dbContext.Employees.Update(user);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteEmployee(string Id)
        {
            var user = await _dbContext.Employees.FindAsync(Id);
            if (user != null)
            {
                user.IsActive = false;
                _dbContext.Employees.Update(user);

                return await _dbContext.SaveChangesAsync();
            }
            return 0;
        }


    }
}

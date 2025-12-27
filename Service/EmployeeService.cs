
using Microsoft.EntityFrameworkCore;

using GBS.Api.DbModels;
using GBS.Data.Model;

namespace GBS.Service
{
    public interface IEmployeeService
    {
        Task<List<Employee>> GetEmployeeList();
        Task<Employee> GetEmployeeById(int Id);
        Task<EmployeeBankDetail> GetEmployeeBankDetail(int Id);
        Task<Employee> GetEmployeeByUsername(string username);
        Task<bool> UsernameAlreadyExists(string username);
        Task<int> SaveEmployee(Employee user);
        Task<int> SaveUserRole(IEnumerable<EmployeeUserRole> data);

        Task<int> SaveTeckStack(IEnumerable<EmployeeTechStack> data);

        Task<int> SaveJobRole(IEnumerable<EmployeeJobRole> data);

        Task<int> SaveEmployeeBankDetail(EmployeeBankDetail data);
        Task<int> DeleteEmployee(string Id);

        Task<int> UpdateEmployee(Employee employee);
        Task UpdateUserRoles(IEnumerable<int> userRoles, int employeeId);
        Task UpdateTechStack(IEnumerable<int> techStackIds, int employeeId);
        Task UpdateJobRoles(IEnumerable<int> jobRoleIds, int employeeId);

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
            return await _dbContext.Employees
               .Where(u => u.Id == Id)
               .Include(e => e.EmployeeBankDetails)
               .Include(e => e.StatusIdFkNavigation)
               .Include(e => e.EmployeeJobRoles)
                   .ThenInclude(ej => ej.JobRoleIdFkNavigation)
               .Include(e => e.EmployeeTechStacks)
                   .ThenInclude(et => et.TechStackIdFkNavigation)
               .Include(e => e.EmployeeUserRoles)
                   .ThenInclude(eu => eu.UserRoleIdFkNavigation)
               .Include(e => e.EmployeeLedgers)
                    .ThenInclude(l => l.StatusIdFkNavigation)
               .Include(e => e.EmployeeLedgers)
                    .ThenInclude(l => l.TransactionTypeIdFkNavigation)
               .FirstOrDefaultAsync();
        }
        public async Task<EmployeeBankDetail> GetEmployeeBankDetail(int Id)
        {
            return await _dbContext.EmployeeBankDetails.Where(u => u.Id == Id).FirstOrDefaultAsync();
        }

        public async Task<Employee> GetEmployeeByUsername(string username)
        {
            return await _dbContext.Employees.FirstOrDefaultAsync(u => u.Username == username);
        }



        public async Task<bool> UsernameAlreadyExists(string username)
        {
            return await _dbContext.Employees.CountAsync(u => u.Username == username) > 0;
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

        //     await _dbContext.SaveChangesAsync();
        // }

        public async Task<int> SaveEmployee(Employee user)
        {
            if (user.Id == 0) { await _dbContext.Employees.AddAsync(user); }
            else { _dbContext.Employees.Update(user); }
            return await _dbContext.SaveChangesAsync();
        }


        public async Task<int> SaveUserRole(IEnumerable<EmployeeUserRole> data)
        {
            var existingRoles = _dbContext.EmployeeUserRoles.Where(x => x.EmployeeIdFk == data.FirstOrDefault().EmployeeIdFk);
            _dbContext.EmployeeUserRoles.RemoveRange(existingRoles);
            await _dbContext.EmployeeUserRoles.AddRangeAsync(data);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> SaveTeckStack(IEnumerable<EmployeeTechStack> data)
        {
            var existingTeckStack = _dbContext.EmployeeTechStacks.Where(t => t.EmployeeIdFk == data.FirstOrDefault().EmployeeIdFk);
            _dbContext.EmployeeTechStacks.RemoveRange(existingTeckStack);
            await _dbContext.EmployeeTechStacks.AddRangeAsync(data);
            return await _dbContext.SaveChangesAsync();
        }
        public async Task<int> SaveJobRole(IEnumerable<EmployeeJobRole> data)
        {
            var existingjobRoles = _dbContext.EmployeeJobRoles.Where(j => j.EmployeeIdFk == data.FirstOrDefault().EmployeeIdFk);
            _dbContext.EmployeeJobRoles.RemoveRange(existingjobRoles);
            await _dbContext.EmployeeJobRoles.AddRangeAsync(data);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> SaveEmployeeBankDetail(EmployeeBankDetail data)
        {
            if (data.Id == 0) { await _dbContext.EmployeeBankDetails.AddAsync(data); }
            else { _dbContext.EmployeeBankDetails.Update(data); }
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

        public async Task<int> UpdateEmployee(Employee employee)
        {
            _dbContext.Employees.Update(employee);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateUserRoles(IEnumerable<int> userRoles, int employeeId)
        {
            var existingRoles = _dbContext.EmployeeUserRoles.Where(x => x.EmployeeIdFk == employeeId);
            _dbContext.EmployeeUserRoles.RemoveRange(existingRoles);

            var newRoles = userRoles.Select(roleId => new EmployeeUserRole
            {
                EmployeeIdFk = employeeId,
                UserRoleIdFk = roleId,
                UpdatedBy = LoggedEmployee.Id,
                UpdatedDate = DateTime.Now
            });

            await _dbContext.EmployeeUserRoles.AddRangeAsync(newRoles);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateTechStack(IEnumerable<int> techStackIds, int employeeId)
        {
            var existingTechStack = _dbContext.EmployeeTechStacks.Where(x => x.EmployeeIdFk == employeeId);
            _dbContext.EmployeeTechStacks.RemoveRange(existingTechStack);

            var newTechStack = techStackIds.Select(techId => new EmployeeTechStack
            {
                EmployeeIdFk = employeeId,
                TechStackIdFk = techId,
                UpdatedBy = LoggedEmployee.Id,
                UpdatedDate = DateTime.Now
                
            });

            await _dbContext.EmployeeTechStacks.AddRangeAsync(newTechStack);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateJobRoles(IEnumerable<int> jobRoleIds, int employeeId)
        {
            var existingJobRoles = _dbContext.EmployeeJobRoles.Where(x => x.EmployeeIdFk == employeeId);
            _dbContext.EmployeeJobRoles.RemoveRange(existingJobRoles);

            var newJobRoles = jobRoleIds.Select(jobId => new EmployeeJobRole
            {
                EmployeeIdFk = employeeId,
                JobRoleIdFk = jobId,
                UpdatedBy = LoggedEmployee.Id,
                UpdatedDate = DateTime.Now
                
            });

            await _dbContext.EmployeeJobRoles.AddRangeAsync(newJobRoles);
            await _dbContext.SaveChangesAsync();
        }


    }
}

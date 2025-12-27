using Azure;
using Firebase.Auth;

using GBS.Model;
using GBS.Api.DbModels;
using GBS.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.ObjectModel;
using System.Net;
using GBS.Api.Model;

namespace GBS.Api.Controller
{
    [Route("[controller]")]
    [ApiController]
    public class EmployeeController(IEmployeeService _employeeService) : ControllerBase
    {
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            int user = Convert.ToInt32(HttpContext.Items["EmployeeId"]);
            try
            {
                // Retrieve all users using the employee service
                var users = await _employeeService.GetEmployeeList();
                var baseUrls = "https://ihs.cc";
                // Check if users list is not empty
                if (users != null && users.Any())
                {
                    // Prepare the response data
                    var response = users.Select(user => new EmployeeVM
                    {
                        Id = user.Id,
                        Username = user.Username,
                        FullName = (user.FirstName + " " + user.LastName).Trim(),
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        PersonalEmail = user.PersonalEmail,
                        PersonalPhone = user.PersonalPhone,
                        //user.EmployeeTechStacks,
                        Cnic = user.Cnic,
                        //user.EmployeeJobRoles,
                        JoiningDate = user.JoiningDate,
                        SeparationDate = user.SeparationDate,
                        ProfilePictureUrl = baseUrls + user.ProfilePictureId,
                        ProfilePictureId = user.ProfilePictureId,

                    }).ToList();

                    // Return success response
                    return Ok(new BaseResponse
                    {
                        status = HttpStatusCode.OK,
                        data = response
                    });
                }

                // Handle case where no users are found
                return NotFound(new BaseResponse
                {
                    status = HttpStatusCode.NotFound,
                    message = "No users found."
                });
            }
            catch (Exception ex)
            {
                // Handle unexpected exceptions
                return StatusCode(500, new BaseResponse
                {
                    status = HttpStatusCode.InternalServerError,
                    message = $"An error occurred: {ex.Message}"
                });
            }
        }


        [HttpGet("{EmployeeId}")]
        public async Task<IActionResult> GetDetails([FromRoute] int EmployeeId)
        {
            try
            {
                // Retrieve all users using the employee service
                var user = await _employeeService.GetEmployeeById(EmployeeId);
                var baseUrls = "https://ihs.cc";
                // Check if users list is not empty
                if (user != null)
                {
                    // Prepare the response data
                    var response = new EmployeeVM
                    {
                        Id = user.Id,
                        Username = user.Username,
                        FullName = (user.FirstName + " " + user.LastName).Trim(),
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        PersonalEmail = user.PersonalEmail,
                        PersonalPhone = user.PersonalPhone,
                        //user.EmployeeTechStacks,
                        Cnic = user.Cnic,
                        //user.EmployeeJobRoles,
                        JoiningDate = user.JoiningDate,
                        SeparationDate = user.SeparationDate,
                        ProfilePictureUrl = baseUrls + user.ProfilePictureId,
                        ProfilePictureId = user.ProfilePictureId,
                        JobRole = user.EmployeeJobRoles.Select(j => j.JobRoleIdFkNavigation.Name),
                        UserRole = user.EmployeeUserRoles.Select(j => j.UserRoleIdFkNavigation.Name),
                        TechStack = user.EmployeeTechStacks.Select(j => j.TechStackIdFkNavigation.Name),
                        BankDetail = user.EmployeeBankDetails.Select(b => new BankDetailVM
                        {
                            Id = b.Id,
                            EmployeeId = b.EmployeeIdFk,
                            BankName = b.BankName,
                            AccountTitle = b.AccountTitle,
                            BranchCode = b.BranchCode,
                            AccountNumber = b.AccountNumber,
                            Iban = b.Iban
                        }).ToList(),
                        Ledger = user.EmployeeLedgers.Select(l => new EmployeeLedgerVM
                        {
                            Id = l.Id,
                            TransactionDate = l.TransactionDate,
                            TransactionTypeIdFk = l.TransactionTypeIdFk,
                            TransactionType = l.TransactionTypeIdFkNavigation.Name,
                            Description = l.Description,
                            DebitAmount = l.DebitAmount,
                            CreditAmount = l.CreditAmount,
                            SalaryMonth = l.SalaryMonth,
                            RunningBalance = l.RunningBalance,
                            StatusIdFk = l.StatusIdFk,
                            Status = l.StatusIdFkNavigation.Name, // Assuming navigation property to Lookups table
                            IsRecovered = l.IsRecovered,
                            CreatedDate = l.CreatedDate,
                            CreatedBy = l.CreatedBy,
                            ModifiedDate = l.ModifiedDate,
                            ModifiedBy = l.ModifiedBy,
                            Remarks = l.Remarks,
                            IsActive = l.IsActive
                        }).ToList(),
                        Status = user.StatusIdFkNavigation?.Name,

                    };

                    // Return success response
                    return Ok(new BaseResponse
                    {
                        status = HttpStatusCode.OK,
                        data = response
                    });
                }

                // Handle case where no users are found
                return NotFound(new BaseResponse
                {
                    status = HttpStatusCode.NotFound,
                    message = "No users found."
                });
            }
            catch (Exception ex)
            {
                // Handle unexpected exceptions
                return StatusCode(500, new BaseResponse
                {
                    status = HttpStatusCode.InternalServerError,
                    message = $"An error occurred: {ex.Message}"
                });
            }
        }


        [HttpPost("Save")]
        public async Task<IActionResult> SaveEmployee([FromBody] EmployeePM employee)
        {
            try
            {
                // Retrieve all users using the employee service
                bool usernameAlreadyExists = false;
                usernameAlreadyExists = await _employeeService.UsernameAlreadyExists(employee.Username);
                if (usernameAlreadyExists && (employee.Id == 0 || employee.Id == null))
                {
                    return Ok(new BaseResponse
                    {
                        status = HttpStatusCode.BadRequest,
                        message = "Username already exists"
                    });
                }

                Employee user = null;
                user = await _employeeService.GetEmployeeById(employee.Id);
                if (user == null)
                {
                    user = new Employee();
                }
                user.FirstName = employee.FirstName;
                user.Username = employee.Username;
                user.LastName = employee.LastName;
                user.PersonalPhone = employee.PersonalPhone;
                user.PersonalEmail = employee.PersonalEmail;
                user.Cnic = employee.Cnic; ;
                user.JoiningDate = employee.JoiningDate;
                user.Username = employee.Username;
                user.Password = "Password@123";
                user.StatusIdFk = employee.Status;
                user.IsActive = true;
                user.ProfilePictureId = employee.ProfilePictureId;

                var saveResponse = await _employeeService.SaveEmployee(user);
                // Check if users list is not empty
                if (saveResponse > 0)
                {
                    Employee savedEmployee = await _employeeService.GetEmployeeByUsername(user.Username);
                    //Saved Userrole
                    var EmployeeUserRoles = employee.UserRole.Select(r => new EmployeeUserRole
                    {
                        EmployeeIdFk = savedEmployee.Id,
                        UserRoleIdFk = r,
                        CreatedBy = LoggedEmployee.Id,
                        CreatedDate = DateTime.Now,
                    });

                    var saveUserRole = await _employeeService.SaveUserRole(EmployeeUserRoles);
                    //Saved TechStack
                    var EmployeeTechStack = employee.TechStack.Select(t => new EmployeeTechStack
                    {
                        EmployeeIdFk = savedEmployee.Id,
                        TechStackIdFk = t,
                        CreatedBy = LoggedEmployee.Id,
                        CreatedDate = DateTime.Now,
                    });
                    var saveTechStack = await _employeeService.SaveTeckStack(EmployeeTechStack);

                    //Saved JobRole
                    var EmployeeJobRole = employee.JobRole.Select(j => new EmployeeJobRole
                    {
                        EmployeeIdFk = savedEmployee.Id,
                        JobRoleIdFk = j,
                        CreatedBy = LoggedEmployee.Id,
                        CreatedDate = DateTime.Now,
                    });
                    var saveJobRole = await _employeeService.SaveJobRole(EmployeeJobRole);

                    var savedUser = await _employeeService.GetEmployeeByUsername(user.Username);

                    var employeeDto = new EmployeeDto
                    {
                        Id = savedUser.Id,
                        Username = savedUser.Username,
                        FirstName = savedUser.FirstName,
                        LastName = savedUser.LastName,
                        PersonalEmail = savedUser.PersonalEmail,
                        PersonalPhone = savedUser.PersonalPhone
                        // Map other properties...
                    };

                    // Return success response
                    return Ok(new BaseResponse
                    {
                        status = HttpStatusCode.OK,
                        message = "Employee saved successfully",
                        data = employeeDto
                    });
                }

                // Handle case where no users are found
                return Ok(new BaseResponse
                {
                    status = HttpStatusCode.BadRequest,
                    message = "Employee not saved"
                });
            }
            catch (Exception ex)
            {
                // Handle unexpected exceptions
                return StatusCode(500, new BaseResponse
                {
                    status = HttpStatusCode.InternalServerError,
                    message = $"An error occurred: {ex.InnerException}"
                });
            }
        }


        [HttpPut("Update")]
        public async Task<IActionResult> UpdateEmployee([FromBody] EmployeeUpdatePM employee)
        {
            try
            {
                // Check if the employee exists
                var existingEmployee = await _employeeService.GetEmployeeById(employee.Id);
                if (existingEmployee == null)
                {
                    return Ok(new BaseResponse
                    {
                        status = HttpStatusCode.NotFound,
                        message = "Employee not found"
                    });
                }

                // Update employee details
                existingEmployee.FirstName = employee.FirstName;
                existingEmployee.LastName = employee.LastName;
                existingEmployee.PersonalPhone = employee.PersonalPhone;
                existingEmployee.PersonalEmail = employee.PersonalEmail;
                existingEmployee.Cnic = employee.Cnic;
                existingEmployee.JoiningDate = employee.JoiningDate;
                existingEmployee.StatusIdFk = employee.Status;
                existingEmployee.ProfilePictureId = employee.ProfilePictureId;

                // Save updated employee details
                var updateEmployeeResponse = await _employeeService.UpdateEmployee(existingEmployee);

                if (updateEmployeeResponse > 0)
                {
                    // Update related data (Roles, Tech Stack, Job Roles)
                    await _employeeService.UpdateUserRoles(employee.UserRole, existingEmployee.Id);
                    await _employeeService.UpdateTechStack(employee.TechStack, existingEmployee.Id);
                    await _employeeService.UpdateJobRoles(employee.JobRole, existingEmployee.Id);

                    // Return success response
                    return Ok(new BaseResponse
                    {
                        status = HttpStatusCode.OK,
                        message = "Employee updated successfully"
                    });
                }

                // If update fails
                return Ok(new BaseResponse
                {
                    status = HttpStatusCode.BadRequest,
                    message = "Employee update failed"
                });
            }
            catch (Exception ex)
            {
                // Handle unexpected exceptions
                return StatusCode(500, new BaseResponse
                {
                    status = HttpStatusCode.InternalServerError,
                    message = $"An error occurred: {ex.InnerException?.Message ?? ex.Message}"
                });
            }
        }


        [HttpPut("UpdatePersonalDetails")]
        public async Task<IActionResult> UpdatePersonalDetails([FromBody] PersonalDetailsPM personalInfo)
        {
            try
            {
                // Check if the user exists
                var existingUser = await _employeeService.GetEmployeeById(personalInfo.Id);
                if (existingUser == null)
                {
                    return Ok(new BaseResponse
                    {
                        status = HttpStatusCode.NotFound,
                        message = "User not found"
                    });
                }

                // Update user personal details
                existingUser.FirstName = personalInfo.FirstName;
                existingUser.LastName = personalInfo.LastName;
                existingUser.PersonalPhone = personalInfo.PersonalPhone;
                existingUser.PersonalEmail = personalInfo.PersonalEmail;
                existingUser.Cnic = personalInfo.Cnic;
                existingUser.ProfilePictureId = personalInfo.ProfilePictureId;

                // Save updated personal details
                var updateResponse = await _employeeService.UpdateEmployee(existingUser);

                if (updateResponse > 0)
                {
                    // Return success response
                    return Ok(new BaseResponse
                    {
                        status = HttpStatusCode.OK,
                        message = "Personal information updated successfully"
                    });
                }

                // If update fails
                return Ok(new BaseResponse
                {
                    status = HttpStatusCode.BadRequest,
                    message = "Failed to update personal information"
                });
            }
            catch (Exception ex)
            {
                // Handle unexpected exceptions
                return StatusCode(500, new BaseResponse
                {
                    status = HttpStatusCode.InternalServerError,
                    message = $"An error occurred: {ex.InnerException?.Message ?? ex.Message}"
                });
            }
        }
        

        [HttpPut("UpdateProfessionalDetails")]
        public async Task<IActionResult> UpdateProfessionalDetails([FromBody] ProfessionalDetailsPM data)
        {
            try
            {
                // Check if the user exists
                // Check if the employee exists
                var existingEmployee = await _employeeService.GetEmployeeById(data.Id);
                if (existingEmployee == null)
                {
                    return Ok(new BaseResponse
                    {
                        status = HttpStatusCode.NotFound,
                        message = "Employee not found"
                    });
                }

                // Update employee details
                existingEmployee.JoiningDate = data.JoiningDate;
                existingEmployee.StatusIdFk = data.Status.Value;
                existingEmployee.SeparationDate = data.SeparationDate;

                // Save updated employee details
                var updateEmployeeResponse = await _employeeService.UpdateEmployee(existingEmployee);

                if (updateEmployeeResponse > 0)
                {
                    // Update related data (Roles, Tech Stack, Job Roles)
                    await _employeeService.UpdateUserRoles(data.UserRole, existingEmployee.Id);
                    await _employeeService.UpdateTechStack(data.TechStack, existingEmployee.Id);
                    await _employeeService.UpdateJobRoles(data.JobRole, existingEmployee.Id);

                    // Return success response
                    return Ok(new BaseResponse
                    {
                        status = HttpStatusCode.OK,
                        message = "Employee updated successfully"
                    });
                }

                if (updateEmployeeResponse > 0)
                {
                    // Return success response
                    return Ok(new BaseResponse
                    {
                        status = HttpStatusCode.OK,
                        message = "Personal information updated successfully"
                    });
                }

                // If update fails
                return Ok(new BaseResponse
                {
                    status = HttpStatusCode.BadRequest,
                    message = "Failed to update personal information"
                });
            }
            catch (Exception ex)
            {
                // Handle unexpected exceptions
                return StatusCode(500, new BaseResponse
                {
                    status = HttpStatusCode.InternalServerError,
                    message = $"An error occurred: {ex.InnerException?.Message ?? ex.Message}"
                });
            }
        }


        [HttpPost("BankDetail/Save")]
        public async Task<IActionResult> SaveBankDetail([FromBody] BankDetailVM data)
        {
            try
            {
                // Retrieve all users using the employee service
                EmployeeBankDetail newData = null;

                if (data.Id > 0)
                {
                    newData = await _employeeService.GetEmployeeBankDetail(data.Id);
                }
                if (newData == null)
                {
                    newData = new EmployeeBankDetail();
                    newData.CreatedBy = LoggedEmployee.Id;
                    newData.CreatedDate = DateTime.Now;
                }
                newData.EmployeeIdFk = data.EmployeeId;
                newData.BranchCode = data.BranchCode;
                newData.BankName = data.BankName;
                newData.AccountTitle = data.AccountTitle;
                newData.AccountNumber = data.AccountNumber;
                newData.Iban = data.Iban;
                newData.EmployeeIdFk = data.EmployeeId;
                newData.UpdatedBy = LoggedEmployee.Id;
                newData.UpdatedDate = DateTime.Now;

                var saveResponse = await _employeeService.SaveEmployeeBankDetail(newData);
                // Check if users list is not empty
                if (saveResponse > 0)
                {
                    // Return success response
                    return Ok(new BaseResponse
                    {
                        status = HttpStatusCode.OK,
                        message = "Bank detail saved successfully"
                    });
                }

                // Handle case where no users are found
                return Ok(new BaseResponse
                {
                    status = HttpStatusCode.BadRequest,
                    message = "Employee not saved"
                });
            }
            catch (Exception ex)
            {
                // Handle unexpected exceptions
                return StatusCode(500, new BaseResponse
                {
                    status = HttpStatusCode.InternalServerError,
                    message = $"An error occurred: {ex.Message}"
                });
            }
        }


    }
}

using GBS.Api.Model;
using GBS.Data.Model;
using GBS.Service;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GBS.Api.Controller
{
    [Route("[controller]")]
    [ApiController]
    public class ClientController(IClientService _clientService) : ControllerBase
    {
       


        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            int user = Convert.ToInt32(HttpContext.Items["ClientId"]);
            try
            {
                // Retrieve all users using the employee service
                var clients = await _clientService.GetClientList();
                if (clients != null && clients.Any())
                {
                    // Prepare the response data
                    var response = clients.Select(client => new ClientVM
                    {
                        Id = client.Id,
                        ClientName = client.ClientName,
                        ContactPerson = client.ContactPerson,
                        Phone = client.Phone,
                        Email = client.Email,
                        Address = client.Address,
                        City = client.City,
                        State = client.State,
                        PostalCode = client.PostalCode,
                        Gstnumber = client.Gstnumber,
                        CreatedBy = client.CreatedBy,
                        CreatedDate = client.CreatedDate,
                        ModifiedBy = client.ModifiedBy,
                        ModifiedDate = client.ModifiedDate,
                        IsActive = client.IsActive
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


        [HttpGet("{ClientId}")]
        public async Task<IActionResult> GetDetails([FromRoute] int ClientId)
        {
            try
            {
                // Retrieve all users using the employee service
                var client = await _clientService.GetClientById(ClientId);
                // Check if users list is not empty
                if (client != null)
                {
                    // Prepare the response data
                    var response = new ClientVM
                    {
                        Id = client.Id,
                        ClientName = client.ClientName,
                        ContactPerson = client.ContactPerson,
                        Phone = client.Phone,
                        Email = client.Email,
                        Address = client.Address,
                        City = client.City,
                        State = client.State,
                        PostalCode = client.PostalCode,
                        Gstnumber = client.Gstnumber,
                        CreatedBy = client.CreatedBy,
                        CreatedDate = client.CreatedDate,
                        ModifiedBy = client.ModifiedBy,
                        ModifiedDate = client.ModifiedDate,
                        IsActive = client.IsActive,
                        Orders = new List<GBS.Data.Model.OrderVM>(),
                        Payments = new List<GBS.Data.Model.PaymentVM>()
                    };
                    foreach (var order in client.Orders)
                    {
                        response.Orders.Add(new GBS.Data.Model.OrderVM
                        {
                            Id = order.Id,
                            OrderNumber = order.OrderNumber,
                            ClientIdFk = order.ClientIdFk,
                            OrderDate = order.OrderDate,
                            DeliveryDate = order.DeliveryDate,
                            StatusIdFk = order.StatusIdFk,
                            Status = order.StatusIdFkNavigation.Name,
                            TotalQuantity = order.TotalQuantity,
                            TotalAmount = order.TotalAmount,
                            AdvanceAmount = order.AdvanceAmount,
                            BalanceAmount = order.BalanceAmount,
                            Notes = order.Notes,
                            CreatedBy = "Hard Coded Name",
                            CreatedDate = order.CreatedDate,
                            ModifiedDate = order.ModifiedDate,
                            ModifiedBy = "Hard Coded Name",

                        });
                        foreach (var payment in order.Payments)
                        {
                            response.Payments.Add(new GBS.Data.Model.PaymentVM
                            {
                                Id = payment.Id,
                                OrderIdFk = payment.OrderIdFk,
                                PaymentDate = payment.PaymentDate,
                                Amount = payment.Amount,
                                PaymentMethodIdFk = payment.PaymentMethodIdFk,
                                ReferenceNumber = payment.ReferenceNumber,
                                Notes = payment.Notes,
                                CreatedBy = payment.CreatedBy,
                                CreatedDate = payment.CreatedDate
                            });
                        }
                    }
                   
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


        //[HttpPost("Save")]
        //public async Task<IActionResult> SaveClient([FromBody] ClientPM employee)
        //{
        //    try
        //    {
        //        // Retrieve all users using the employee service
        //        bool usernameAlreadyExists = false;
        //        usernameAlreadyExists = await _clientService.UsernameAlreadyExists(employee.Username);
        //        if (usernameAlreadyExists)
        //        {
        //            return Ok(new BaseResponse
        //            {
        //                status = HttpStatusCode.BadRequest,
        //                message = "Username already exists"
        //            });
        //        }

        //        Client user = null;
        //        user = await _clientService.GetClientById(employee.Id);
        //        if (user == null)
        //        {
        //            user = new Client();
        //        }
        //        user.FirstName = employee.FirstName;
        //        user.Username = employee.Username;
        //        user.LastName = employee.LastName;
        //        user.PersonalPhone = employee.PersonalPhone;
        //        user.PersonalEmail = employee.PersonalEmail;
        //        user.Cnic = employee.Cnic; ;
        //        user.JoiningDate = employee.JoiningDate;
        //        user.Username = employee.Username;
        //        user.Password = "Password@123";
        //        user.StatusIdFk = employee.Status;
        //        user.IsActive = true;
        //        user.ProfilePictureId = employee.ProfilePictureId;

        //        var saveResponse = await _clientService.SaveClient(user);
        //        // Check if users list is not empty
        //        if (saveResponse > 0)
        //        {
        //            Client savedClient = await _clientService.GetClientByUsername(user.Username);
        //            //Saved Userrole
        //            var ClientUserRoles = employee.UserRole.Select(r => new ClientUserRole
        //            {
        //                ClientIdFk = savedClient.Id,
        //                UserRoleIdFk = r,
        //                CreatedBy = LoggedClient.Id,
        //                CreatedDate = DateTime.Now,
        //            });

        //            var saveUserRole = await _clientService.SaveUserRole(ClientUserRoles);
        //            //Saved TechStack
        //            var ClientTechStack = employee.TechStack.Select(t => new ClientTechStack
        //            {
        //                ClientIdFk = savedClient.Id,
        //                TechStackIdFk = t,
        //                CreatedBy = LoggedClient.Id,
        //                CreatedDate = DateTime.Now,
        //            });
        //            var saveTechStack = await _clientService.SaveTeckStack(ClientTechStack);

        //            //Saved JobRole
        //            var ClientJobRole = employee.JobRole.Select(j => new ClientJobRole
        //            {
        //                ClientIdFk = savedClient.Id,
        //                JobRoleIdFk = j,
        //                CreatedBy = LoggedClient.Id,
        //                CreatedDate = DateTime.Now,
        //            });
        //            var saveJobRole = await _clientService.SaveJobRole(ClientJobRole);

        //            var savedUser = await _clientService.GetClientByUsername(user.Username);

        //            var employeeDto = new ClientDto
        //            {
        //                Id = savedUser.Id,
        //                Username = savedUser.Username,
        //                FirstName = savedUser.FirstName,
        //                LastName = savedUser.LastName,
        //                PersonalEmail = savedUser.PersonalEmail,
        //                PersonalPhone = savedUser.PersonalPhone
        //                // Map other properties...
        //            };

        //            // Return success response
        //            return Ok(new BaseResponse
        //            {
        //                status = HttpStatusCode.OK,
        //                message = "Client saved successfully",
        //                data = employeeDto
        //            });
        //        }

        //        // Handle case where no users are found
        //        return Ok(new BaseResponse
        //        {
        //            status = HttpStatusCode.BadRequest,
        //            message = "Client not saved"
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle unexpected exceptions
        //        return StatusCode(500, new BaseResponse
        //        {
        //            status = HttpStatusCode.InternalServerError,
        //            message = $"An error occurred: {ex.InnerException}"
        //        });
        //    }
        //}


        //[HttpPut("Update")]
        //public async Task<IActionResult> UpdateClient([FromBody] ClientUpdatePM employee)
        //{
        //    try
        //    {
        //        // Check if the employee exists
        //        var existingClient = await _clientService.GetClientById(employee.Id);
        //        if (existingClient == null)
        //        {
        //            return Ok(new BaseResponse
        //            {
        //                status = HttpStatusCode.NotFound,
        //                message = "Client not found"
        //            });
        //        }

        //        // Update employee details
        //        existingClient.FirstName = employee.FirstName;
        //        existingClient.LastName = employee.LastName;
        //        existingClient.PersonalPhone = employee.PersonalPhone;
        //        existingClient.PersonalEmail = employee.PersonalEmail;
        //        existingClient.Cnic = employee.Cnic;
        //        existingClient.JoiningDate = employee.JoiningDate;
        //        existingClient.StatusIdFk = employee.Status;
        //        existingClient.ProfilePictureId = employee.ProfilePictureId;

        //        // Save updated employee details
        //        var updateClientResponse = await _clientService.UpdateClient(existingClient);

        //        if (updateClientResponse > 0)
        //        {
        //            // Update related data (Roles, Tech Stack, Job Roles)
        //            await _clientService.UpdateUserRoles(employee.UserRole, existingClient.Id);
        //            await _clientService.UpdateTechStack(employee.TechStack, existingClient.Id);
        //            await _clientService.UpdateJobRoles(employee.JobRole, existingClient.Id);

        //            // Return success response
        //            return Ok(new BaseResponse
        //            {
        //                status = HttpStatusCode.OK,
        //                message = "Client updated successfully"
        //            });
        //        }

        //        // If update fails
        //        return Ok(new BaseResponse
        //        {
        //            status = HttpStatusCode.BadRequest,
        //            message = "Client update failed"
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle unexpected exceptions
        //        return StatusCode(500, new BaseResponse
        //        {
        //            status = HttpStatusCode.InternalServerError,
        //            message = $"An error occurred: {ex.InnerException?.Message ?? ex.Message}"
        //        });
        //    }
        //}


    }

   
}

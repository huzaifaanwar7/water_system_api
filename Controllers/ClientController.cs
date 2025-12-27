using GBS.Api.DbModels;
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


        [HttpPost("Save")]
        public async Task<IActionResult> SaveClient([FromBody] ClientVM clientPM)
        {
            try
            {
                int loggedInUserId = Convert.ToInt32(HttpContext.Items["ClientId"]);

                // Validate required fields
                if (string.IsNullOrWhiteSpace(clientPM.ClientName))
                {
                    return BadRequest(new BaseResponse
                    {
                        status = HttpStatusCode.BadRequest,
                        message = "Client name is required"
                    });
                }

                Client client;

                if (clientPM.Id > 0)
                {
                    // Update existing client
                    client = await _clientService.GetClientById(clientPM.Id);
                    if (client == null)
                    {
                        return NotFound(new BaseResponse
                        {
                            status = HttpStatusCode.NotFound,
                            message = "Client not found"
                        });
                    }
                    client.ModifiedBy = loggedInUserId;
                    client.ModifiedDate = DateTime.Now;
                }
                else
                {
                    // Create new client
                    client = new Client
                    {
                        CreatedBy = loggedInUserId,
                        CreatedDate = DateTime.Now
                    };
                }

                // Map properties
                client.ClientName = clientPM.ClientName;
                client.ContactPerson = clientPM.ContactPerson;
                client.Phone = clientPM.Phone;
                client.Email = clientPM.Email;
                client.Address = clientPM.Address;
                client.City = clientPM.City;
                client.State = clientPM.State;
                client.PostalCode = clientPM.PostalCode;
                client.Gstnumber = clientPM.Gstnumber;
                client.IsActive = clientPM.IsActive;

                var saveResponse = await _clientService.SaveClient(client);

                if (saveResponse > 0)
                {
                    return Ok(new BaseResponse
                    {
                        status = HttpStatusCode.OK,
                        message = clientPM.Id > 0 ? "Client updated successfully" : "Client saved successfully",
                        data = new { Id = client.Id }
                    });
                }

                return BadRequest(new BaseResponse
                {
                    status = HttpStatusCode.BadRequest,
                    message = "Failed to save client"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse
                {
                    status = HttpStatusCode.InternalServerError,
                    message = $"An error occurred: {ex.Message}"
                });
            }
        }


        [HttpDelete("{clientId}")]
        public async Task<IActionResult> DeleteClient([FromRoute] int clientId)
        {
            try
            {
                var client = await _clientService.GetClientById(clientId);
                if (client == null)
                {
                    return NotFound(new BaseResponse
                    {
                        status = HttpStatusCode.NotFound,
                        message = "Client not found"
                    });
                }

                // Soft delete
                client.IsActive = false;
                client.ModifiedBy = Convert.ToInt32(HttpContext.Items["ClientId"]);
                client.ModifiedDate = DateTime.Now;

                var result = await _clientService.SaveClient(client);

                if (result > 0)
                {
                    return Ok(new BaseResponse
                    {
                        status = HttpStatusCode.OK,
                        message = "Client deleted successfully"
                    });
                }

                return BadRequest(new BaseResponse
                {
                    status = HttpStatusCode.BadRequest,
                    message = "Failed to delete client"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse
                {
                    status = HttpStatusCode.InternalServerError,
                    message = $"An error occurred: {ex.Message}"
                });
            }
        }

    }

   
}

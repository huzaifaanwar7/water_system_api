using GBS.Data.Model;
using GBS.Service;
using GBS.Service.Service;  // Use GBS.Service.Service namespace
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;

namespace GBS.Api.Controller
{
    [Route("[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var orders = await _orderService.GetOrderList();
                if (orders != null && orders.Any())
                {
                    var response = orders.Select(order => new OrderVM
                    {
                        Id = order.Id,
                        OrderNumber = order.OrderNumber,
                        ClientIdFk = order.ClientIdFk,
                        OrderDate = order.OrderDate,
                        DeliveryDate = order.DeliveryDate,
                        StatusIdFk = order.StatusIdFk,
                        Status = order.StatusIdFkNavigation?.Name,
                        TotalQuantity = order.TotalQuantity,
                        TotalAmount = order.TotalAmount,
                        AdvanceAmount = order.AdvanceAmount,
                        BalanceAmount = order.BalanceAmount,
                        Notes = order.Notes,
                        CreatedBy = "Hard Coded Name",
                        CreatedDate = order.CreatedDate,
                        ModifiedDate = order.ModifiedDate,
                        ModifiedBy = "Hard Coded Name"
                    }).ToList();

                    return Ok(new
                    {
                        status = HttpStatusCode.OK,
                        data = response
                    });
                }

                return NotFound(new
                {
                    status = HttpStatusCode.NotFound,
                    message = "No orders found."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = HttpStatusCode.InternalServerError,
                    message = $"An error occurred: {ex.Message}"
                });
            }
        }
        [HttpGet("GetOrderList")]
        public async Task<IActionResult> GetOrderList()
        {
            try
            {
                var orders = await _orderService.GetOrderList();
                if (orders != null && orders.Any())
                {
                    var response = orders.Select(order => new OrderVM
                    {
                        Id = order.Id,
                        OrderNumber = order.OrderNumber,
                        ClientIdFk = order.ClientIdFk,
                        OrderDate = order.OrderDate,
                        DeliveryDate = order.DeliveryDate,
                        StatusIdFk = order.StatusIdFk,
                        Status = order.StatusIdFkNavigation?.Name,
                        TotalQuantity = order.TotalQuantity,
                        TotalAmount = order.TotalAmount,
                        AdvanceAmount = order.AdvanceAmount,
                        BalanceAmount = order.BalanceAmount,
                        Notes = order.Notes,
                        CreatedBy = "Hard Coded Name",
                        CreatedDate = order.CreatedDate,
                        ModifiedDate = order.ModifiedDate,
                        ModifiedBy = "Hard Coded Name"
                    }).ToList();

                    return Ok(new
                    {
                        status = System.Net.HttpStatusCode.OK,
                        data = response
                    });
                }

                return NotFound(new
                {
                    status = System.Net.HttpStatusCode.NotFound,
                    message = "No Orders Found"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = System.Net.HttpStatusCode.InternalServerError,
                    message = $"An Error Occurred: {ex.Message}"
                });
            }
        }


        [HttpGet("{OrderId}")]
        public async Task<IActionResult> GetDetails([FromRoute] int OrderId)
        {
            try
            {
                var order = await _orderService.GetOrderById(OrderId);
                if (order != null)
                {
                    var response = new OrderVM
                    {
                        Id = order.Id,
                        OrderNumber = order.OrderNumber,
                        ClientIdFk = order.ClientIdFk,
                        OrderDate = order.OrderDate,
                        DeliveryDate = order.DeliveryDate,
                        StatusIdFk = order.StatusIdFk,
                        Status = order.StatusIdFkNavigation?.Name,
                        TotalQuantity = order.TotalQuantity,
                        TotalAmount = order.TotalAmount,
                        AdvanceAmount = order.AdvanceAmount,
                        BalanceAmount = order.BalanceAmount,
                        Notes = order.Notes,
                        CreatedBy = "Hard Coded Name",
                        CreatedDate = order.CreatedDate,
                        ModifiedDate = order.ModifiedDate,
                        ModifiedBy = "Hard Coded Name"
                    };

                    return Ok(new
                    {
                        status = HttpStatusCode.OK,
                        data = response
                    });
                }

                return NotFound(new
                {
                    status = HttpStatusCode.NotFound,
                    message = "No order found."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = HttpStatusCode.InternalServerError,
                    message = $"An error occurred: {ex.Message}"
                });
            }
        }

        [HttpGet("Client/{ClientId}")]
        public async Task<IActionResult> GetOrdersByClient([FromRoute] int ClientId)
        {
            try
            {
                var orders = await _orderService.GetOrdersByClientId(ClientId);
                if (orders != null && orders.Any())
                {
                    var response = orders.Select(order => new OrderVM
                    {
                        Id = order.Id,
                        OrderNumber = order.OrderNumber,
                        ClientIdFk = order.ClientIdFk,
                        OrderDate = order.OrderDate,
                        DeliveryDate = order.DeliveryDate,
                        StatusIdFk = order.StatusIdFk,
                        Status = order.StatusIdFkNavigation?.Name,
                        TotalQuantity = order.TotalQuantity,
                        TotalAmount = order.TotalAmount,
                        AdvanceAmount = order.AdvanceAmount,
                        BalanceAmount = order.BalanceAmount,
                        Notes = order.Notes,
                        CreatedBy = "Hard Coded Name",
                        CreatedDate = order.CreatedDate,
                        ModifiedDate = order.ModifiedDate,
                        ModifiedBy = "Hard Coded Name"
                    }).ToList();

                    return Ok(new
                    {
                        status = HttpStatusCode.OK,
                        data = response
                    });
                }

                return NotFound(new
                {
                    status = HttpStatusCode.NotFound,
                    message = "No orders found for this client."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = HttpStatusCode.InternalServerError,
                    message = $"An error occurred: {ex.Message}"
                });
            }
        }
    }
}
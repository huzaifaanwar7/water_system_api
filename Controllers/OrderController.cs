using GBS.Api.DbModels;
using GBS.Api.Model.Post;
using GBS.Model;
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
                        Reference = order.Reference,
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
                        Reference = order.Reference,
                        ClientIdFk = order.ClientIdFk,
                        ClientName = order.ClientIdFkNavigation?.ClientName,
                        ClientReference = order.ClientIdFkNavigation?.Reference,
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
                        ModifiedBy = "Hard Coded Name",

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
                        Reference = order.Reference,
                        ClientIdFk = order.ClientIdFk,
                        ClientName = order.ClientIdFkNavigation?.ClientName,
                        ClientReference = order.ClientIdFkNavigation?.Reference,
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
                        ModifiedBy = "Hard Coded Name",

                        OrderCosts = order.OrderCosts?.Select(oc => new OrderCostVM
                        {
                            Id = oc.Id,
                            OrderIdFk = oc.OrderIdFk,
                            CostCategoryIdFk = oc.CostCategoryIdFk,
                            CostDescription = oc.CostDescription,
                            Quantity = oc.Quantity,
                            UnitCost = oc.UnitCost,
                            TotalCost = oc.TotalCost,
                            CostDate = oc.CostDate,
                            VendorName = oc.VendorName,
                            InvoiceNumber = oc.InvoiceNumber,
                            Notes = oc.Notes
                        }).ToList(),

                        OrderItems = order.OrderItems?.Select(oi => new OrderItemVM
                        {
                            Id = oi.Id,
                            OrderIdFk = oi.OrderIdFk,
                            ProductIdFk = oi.ProductIdFk,
                            ProductReference = oi.ProductIdFkNavigation.Reference,
                            ProductName = oi.ProductIdFkNavigation.ProductName,
                            Quantity = oi.Quantity,
                            SizeIdFk = oi.SizeIdFk,
                            SizeName = oi.SizeIdFkNavigation.Name,
                            Color = oi.Color,
                            UnitPrice = oi.UnitPrice,
                            TotalPrice = oi.TotalPrice,
                            SpecialInstructions = oi.SpecialInstructions,
                            IsCompleted = oi.IsCompleted,
                            CompletedQuantity = oi.CompletedQuantity
                        }).ToList(),

                        OrderLabors = order.OrderLabors?.Select(ol => new OrderLaborVM
                        {
                            Id = ol.Id,
                            OrderIdFk = ol.OrderIdFk,
                            OrderItemIdFk = ol.OrderItemIdFk,
                            EmployeeIdFk = ol.EmployeeIdFk,
                            WorkDate = ol.WorkDate,
                            QuantityCompleted = ol.QuantityCompleted,
                            HoursWorked = ol.HoursWorked,
                            RatePerPiece = ol.RatePerPiece,
                            TotalLaborCost = ol.TotalLaborCost,
                            CreatedBy = ol.CreatedBy,
                            CreatedDate = ol.CreatedDate
                        }).ToList(),

                        OrderMaterials = order.OrderMaterials?.Select(om => new OrderMaterialVM
                        {
                            Id = om.Id,
                            OrderIdFk = om.OrderIdFk,
                            MaterialIdFk = om.MaterialIdFk,
                            QuantityUsed = om.QuantityUsed,
                            UnitCost = om.UnitCost,
                            TotalCost = om.TotalCost,
                            UsageDate = om.UsageDate,
                            Notes = om.Notes,
                            CreatedBy = om.CreatedBy,
                            CreatedDate = om.CreatedDate
                        }).ToList(),

                        OrderStatusHistories = order.OrderStatusHistories?.Select(osh => new OrderStatusHistoryVM
                        {
                            Id = osh.Id,
                            OrderIdFk = osh.OrderIdFk,
                            StatusIdFk = osh.StatusIdFk,
                            StatusDate = osh.StatusDate,
                            ChangedBy = osh.ChangedBy,
                            Notes = osh.Notes
                        }).ToList()
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
                        Reference = order.Reference,
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


        [HttpPost("Save")]
        public async Task<IActionResult> SaveOrder([FromBody] OrderPM orderPM)
        {
            try
            {
                // Validate required fields
                if (orderPM.ClientId <= 0)
                {
                    return BadRequest(new
                    {
                        status = HttpStatusCode.BadRequest,
                        message = "Client is required"
                    });
                }

                Order order;
                bool isNew = orderPM.Id == 0;

                if (!isNew)
                {
                    // Update existing order
                    order = await _orderService.GetOrderById(orderPM.Id);
                    if (order == null)
                    {
                        return NotFound(new
                        {
                            status = HttpStatusCode.NotFound,
                            message = "Order not found"
                        });
                    }
                    order.ModifiedDate = DateTime.Now;
                    // order.ModifiedBy = loggedInUserId; // Uncomment when auth is ready
                }
                else
                {
                    // Create new order
                    order = new Order
                    {
                        Reference = "ORD123",
                        CreatedDate = DateTime.Now,
                        // CreatedBy = loggedInUserId // Uncomment when auth is ready
                    };
                }

                // Map properties
                order.ClientIdFk = orderPM.ClientId;
                order.OrderDate = orderPM.OrderDate;
                order.DeliveryDate = orderPM.DeliveryDate;
                order.StatusIdFk = orderPM.StatusId;
                order.TotalQuantity = orderPM.TotalQuantity;
                order.TotalAmount = orderPM.TotalAmount;
                order.AdvanceAmount = orderPM.AdvanceAmount;
                order.BalanceAmount = orderPM.BalanceAmount ?? (orderPM.TotalAmount - (orderPM.AdvanceAmount));
                order.Notes = orderPM.Notes;

                var saveResponse = await _orderService.SaveOrder(order);

                if (saveResponse > 0)
                {
                    //    // Handle Order Items
                    //    if (orderPM.OrderItems != null && orderPM.OrderItems.Any())
                    //    {
                    //        // Delete existing items if updating
                    //        if (!isNew)
                    //        {
                    //            await _orderService.DeleteOrderItems(order.Id);
                    //        }

                    //        var orderItems = orderPM.OrderItems.Select(item => new OrderItem
                    //        {
                    //            OrderIdFk = order.Id,
                    //            ProductIdFk = item.ProductIdFk,
                    //            Quantity = item.Quantity,
                    //            SizeIdFk = item.SizeIdFk,
                    //            Color = item.Color,
                    //            UnitPrice = item.UnitPrice,
                    //            TotalPrice = item.TotalPrice,
                    //            SpecialInstructions = item.SpecialInstructions,
                    //            IsCompleted = item.IsCompleted,
                    //            CompletedQuantity = item.CompletedQuantity
                    //        });

                    //        await _orderService.SaveOrderItems(orderItems);
                    //    }

                    //    // Handle Order Costs
                    //    if (orderPM.OrderCosts != null && orderPM.OrderCosts.Any())
                    //    {
                    //        // Delete existing costs if updating
                    //        if (!isNew)
                    //        {
                    //            await _orderService.DeleteOrderCosts(order.Id);
                    //        }

                    //        var orderCosts = orderPM.OrderCosts.Select(cost => new OrderCostVM
                    //        {
                    //            OrderIdFk = order.Id,
                    //            CostCategoryIdFk = cost.CostCategoryIdFk,
                    //            CostDescription = cost.CostDescription,
                    //            Quantity = cost.Quantity,
                    //            UnitCost = cost.UnitCost,
                    //            TotalCost = cost.TotalCost,
                    //            CostDate = cost.CostDate,
                    //            VendorName = cost.VendorName,
                    //            InvoiceNumber = cost.InvoiceNumber,
                    //            Notes = cost.Notes,
                    //            CreatedDate = DateTime.Now
                    //        });

                    //        await _orderService.SaveOrderCosts(orderCosts);
                    //    }

                    return Ok(new
                    {
                        status = HttpStatusCode.OK,
                        message = isNew ? "order created successfully" : "order updated successfully",
                        data = new { id = order.Id, ordernumber = order.Reference }
                    });
                }

                return BadRequest(new
                {
                    status = HttpStatusCode.BadRequest,
                    message = "Failed to save order"
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
        [HttpPost("SaveItem")]
        public async Task<IActionResult> SaveItem([FromBody] OrderItemPM model)
        {
            try
            {
                if (model.OrderId <= 0 || model.ProductIdFk <= 0)
                {
                    return BadRequest(new
                    {
                        status = HttpStatusCode.BadRequest,
                        message = "Invalid order or product"
                    });
                }

                var order = await _orderService.GetOrderById(model.OrderId);
                if (order == null)
                {
                    return NotFound(new
                    {
                        status = HttpStatusCode.NotFound,
                        message = "Order not found"
                    });
                }

                OrderItem item;

                if (model.Id > 0)
                {
                    item = order.OrderItems.FirstOrDefault(x => x.Id == model.Id);
                    if (item == null)
                        return NotFound("Order item not found");
                }
                else
                {
                    item = new OrderItem
                    {
                        OrderIdFk = model.OrderId
                    };
                }


                item.ProductIdFk = model.ProductIdFk;
                item.SizeIdFk = model.SizeIdFk;
                item.Color = model.Color;
                item.Quantity = model.Quantity;
                item.UnitPrice = model.UnitPrice;
                item.TotalPrice = model.UnitPrice * model.Quantity;
                item.SpecialInstructions = model.SpecialInstructions;

                await _orderService.SaveOrderItem(item);

                // Recalculate order totals
                order.TotalQuantity = order.OrderItems.Sum(x => x.Quantity);
                order.TotalAmount = (decimal)order.OrderItems.Sum(x => x.TotalPrice);
                order.BalanceAmount = order.TotalAmount - (order.AdvanceAmount);

                await _orderService.SaveOrder(order);

                return Ok(new
                {
                    status = HttpStatusCode.OK,
                    message = "Order item saved successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = HttpStatusCode.InternalServerError,
                    message = ex.Message
                });
            }
        }


        [HttpPost("{OrderId}/UpdateStatus")]
        public async Task<IActionResult> UpdateStatus([FromRoute] int OrderId, [FromBody] OrderStatusHistoryPM statusUpdate)
        {
            int loggedInUserId = Convert.ToInt32(HttpContext.Items["EmployeeId"]);
            try
            {
                if (statusUpdate.StatusId == null || statusUpdate.StatusId <= 0)
                {
                    return BadRequest(new
                    {
                        status = HttpStatusCode.BadRequest,
                        message = "Invalid status ID"
                    });
                }
                Order order = await _orderService.GetOrderById(OrderId);
                if (order == null)
                {
                    return NotFound(new
                    {
                        status = HttpStatusCode.NotFound,
                        message = "Order not found"
                    });
                }
                order.StatusIdFk = (int)statusUpdate.StatusId;
                order.ModifiedDate = DateTime.Now;
                order.ModifiedBy = loggedInUserId; // Uncomment when auth is ready
                var updateStatus = await _orderService.SaveOrder(order);
                if (updateStatus > 0)
                {
                    OrderStatusHistory orderStatusHistory = new OrderStatusHistory
                    {
                        OrderIdFk = OrderId,
                        StatusIdFk = (int)statusUpdate.StatusId,
                        StatusDate = DateTime.Now,
                        ChangedBy = loggedInUserId,
                        Notes = statusUpdate.Notes
                    };
                    var saveStatusHistory = await _orderService.SaveOrderStatusHistory(orderStatusHistory);
                    if (!saveStatusHistory)
                    {
                        return BadRequest(new
                        {
                            status = HttpStatusCode.BadRequest,
                            message = "Failed to save order status history"
                        });
                    }
                    if (saveStatusHistory)
                    {
                        return Ok(new
                        {
                            status = HttpStatusCode.OK,
                            message = "Order status updated successfully"
                        });
                    }
                }



                return BadRequest(new
                {
                    status = HttpStatusCode.BadRequest,
                    message = "Failed to update order status"
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
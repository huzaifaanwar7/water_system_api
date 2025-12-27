using GBS.Api.DbModels;
using GBS.Model;
using GBS.Service;
using GBS.Service.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace GBS.Api.Controller
{
    [Route("[controller]")]
    [ApiController]
    public class PaymentController(IPaymentService _paymentService, IOrderService _orderService) : ControllerBase
    {

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var payments = await _paymentService.GetPaymentList();
                if (payments != null && payments.Any())
                {
                    var response = payments.Select(p => new PaymentVM
                    {
                        Id = p.Id,
                        OrderIdFk = p.OrderIdFk,
                        PaymentDate = p.PaymentDate,
                        Amount = p.Amount,
                        PaymentMethodIdFk = p.PaymentMethodIdFk,
                        Reference = p.Reference,
                        Notes = p.Notes,
                        CreatedBy = p.CreatedBy,
                        CreatedDate = p.CreatedDate
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
                    message = "No payments found."
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

        [HttpGet("{PaymentId}")]
        public async Task<IActionResult> GetDetails([FromRoute] int PaymentId)
        {
            try
            {
                var payment = await _paymentService.GetPaymentById(PaymentId);
                if (payment != null)
                {
                    var response = new PaymentVM
                    {
                        Id = payment.Id,
                        OrderIdFk = payment.OrderIdFk,
                        PaymentDate = payment.PaymentDate,
                        Amount = payment.Amount,
                        PaymentMethodIdFk = payment.PaymentMethodIdFk,
                        Reference = payment.Reference,
                        Notes = payment.Notes,
                        CreatedBy = payment.CreatedBy,
                        CreatedDate = payment.CreatedDate
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
                    message = "No payment found."
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

        [HttpGet("Order/{OrderId}")]
        public async Task<IActionResult> GetPaymentsByOrder([FromRoute] int OrderId)
        {
            try
            {
                var payments = await _paymentService.GetPaymentsByOrderId(OrderId);
                if (payments != null && payments.Any())
                {
                    var response = payments.Select(p => new PaymentVM
                    {
                        Id = p.Id,
                        OrderIdFk = p.OrderIdFk,
                        PaymentDate = p.PaymentDate,
                        Amount = p.Amount,
                        PaymentMethodIdFk = p.PaymentMethodIdFk,
                        Reference = p.Reference,
                        Notes = p.Notes,
                        CreatedBy = p.CreatedBy,
                        CreatedDate = p.CreatedDate
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
                    message = "No payments found for this order."
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
        public async Task<IActionResult> SavePayment([FromBody] PaymentVM paymentPM)
        {
            try
            {
                // Validate required fields
                if (paymentPM.OrderIdFk <= 0)
                {
                    return BadRequest(new
                    {
                        status = HttpStatusCode.BadRequest,
                        message = "Order is required"
                    });
                }

                if (paymentPM.Amount <= 0)
                {
                    return BadRequest(new
                    {
                        status = HttpStatusCode.BadRequest,
                        message = "Payment amount must be greater than 0"
                    });
                }

                // Verify order exists
                var order = await _orderService.GetOrderById(paymentPM.OrderIdFk);
                if (order == null)
                {
                    return NotFound(new
                    {
                        status = HttpStatusCode.NotFound,
                        message = "Order not found"
                    });
                }

                Payment payment;
                decimal previousAmount = 0;
                bool isNew = paymentPM.Id == 0;

                if (!isNew)
                {
                    // Update existing payment
                    payment = await _paymentService.GetPaymentById(paymentPM.Id);
                    if (payment == null)
                    {
                        return NotFound(new
                        {
                            status = HttpStatusCode.NotFound,
                            message = "Payment not found"
                        });
                    }
                    previousAmount = payment.Amount; // Store for balance calculation
                }
                else
                {
                    // Create new payment
                    payment = new Payment
                    {
                        CreatedDate = DateTime.Now,
                        // CreatedBy = loggedInUserId // Uncomment when auth is ready
                    };
                }

                // Map properties
                payment.OrderIdFk = paymentPM.OrderIdFk;
                payment.PaymentDate = paymentPM.PaymentDate;
                payment.Amount = paymentPM.Amount;
                payment.PaymentMethodIdFk = paymentPM.PaymentMethodIdFk;
                payment.Reference = paymentPM.Reference;
                payment.Notes = paymentPM.Notes;

                var saveResponse = await _paymentService.SavePayment(payment);

                if (saveResponse > 0)
                {
                    // Update Order Balance
                    var totalPayments = await _paymentService.GetTotalPaymentsByOrderId(order.Id);
                    order.AdvanceAmount = totalPayments;
                    order.BalanceAmount = order.TotalAmount - totalPayments;
                    order.ModifiedDate = DateTime.Now;
                    await _orderService.SaveOrder(order);

                    return Ok(new
                    {
                        status = HttpStatusCode.OK,
                        message = isNew ? "Payment recorded successfully" : "Payment updated successfully",
                        data = new
                        {
                            Id = payment.Id,
                            OrderBalance = order.BalanceAmount,
                            TotalPaid = totalPayments
                        }
                    });
                }

                return BadRequest(new
                {
                    status = HttpStatusCode.BadRequest,
                    message = "Failed to save payment"
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

        //[HttpDelete("{paymentId}")]
        //public async Task<IActionResult> DeletePayment([FromRoute] int paymentId)
        //{
        //    try
        //    {
        //        var payment = await _paymentService.GetPaymentById(paymentId);
        //        if (payment == null)
        //        {
        //            return NotFound(new
        //            {
        //                status = HttpStatusCode.NotFound,
        //                message = "Payment not found"
        //            });
        //        }

        //        var orderId = payment.OrderIdFk;
        //        var deleteResponse = await _paymentService.DeletePayment(paymentId);

        //        if (deleteResponse > 0)
        //        {
        //            // Update Order Balance after deletion
        //            var order = await _orderService.GetOrderById(orderId);
        //            if (order != null)
        //            {
        //                var totalPayments = await _paymentService.GetTotalPaymentsByOrderId(orderId);
        //                order.AdvanceAmount = totalPayments;
        //                order.BalanceAmount = order.TotalAmount - totalPayments;
        //                order.ModifiedDate = DateTime.Now;
        //                await _orderService.SaveOrder(order);
        //            }

        //            return Ok(new
        //            {
        //                status = HttpStatusCode.OK,
        //                message = "Payment deleted successfully"
        //            });
        //        }

        //        return BadRequest(new
        //        {
        //            status = HttpStatusCode.BadRequest,
        //            message = "Failed to delete payment"
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new
        //        {
        //            status = HttpStatusCode.InternalServerError,
        //            message = $"An error occurred: {ex.Message}"
        //        });
        //    }
        //}


    }
}

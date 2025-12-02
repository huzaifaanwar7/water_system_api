using GBS.Data.Model;
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
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

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
                        ReferenceNumber = p.ReferenceNumber,
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
                        ReferenceNumber = payment.ReferenceNumber,
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
                        ReferenceNumber = p.ReferenceNumber,
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

       

        
    }
}

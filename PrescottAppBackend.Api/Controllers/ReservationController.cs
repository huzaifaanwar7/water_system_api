using System.Net;
using Microsoft.AspNetCore.Mvc;
using PrescottAppBackend.Api.Model;
using PrescottAppBackend.Domain;

namespace PrescottAppBackend.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController(IReservationService _reservationService) : ControllerBase
    {
        [HttpGet]
        public async Task<BaseResponse> Get()
        {
            try
            {
                var Reservations = await _reservationService.GetAllReservationsAsync();
                if (Reservations == null)
                {
                    return new BaseResponse
                    {
                        status = HttpStatusCode.NotFound,
                        data = Reservations
                    };
                }
                return new BaseResponse
                {
                    status = HttpStatusCode.OK,
                    data = Reservations
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse
                {
                    status = HttpStatusCode.InternalServerError,
                    message = ex.Message,
                    data = ex
                };
            }
        }

        [HttpGet("{id}")]
        public async Task<BaseResponse> Get(int id)
        {
            try
            {
                var Reservations = await _reservationService.GetReservationByIdAsync(id);
                if (Reservations == null)
                {
                    return new BaseResponse
                    {
                        status = HttpStatusCode.NotFound,
                        data = Reservations
                    };
                }
                return new BaseResponse
                {
                    status = HttpStatusCode.OK,
                    data = Reservations
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse
                {
                    status = HttpStatusCode.InternalServerError,
                    message = ex.Message,
                    data = ex
                };
            }
        }

        [HttpPost]
        public async Task<BaseResponse> AddUpdate([FromBody] ReservationVM vM)
        {
            try
            {
                var reservation = await _reservationService.AddUpdateReservationAsync(vM);
                if (reservation == "Already Exists")
                {
                    return new BaseResponse()
                    {
                        status = HttpStatusCode.Conflict,
                        data = reservation,
                        message = reservation,
                    };
                }
                else if (reservation == "Not Found")
                {
                    return new BaseResponse()
                    {
                        status = HttpStatusCode.NotFound,
                        data = reservation,
                        message = reservation,
                    };
                }
                else
                {
                    return new BaseResponse()
                    {
                        status = HttpStatusCode.OK,
                        data = reservation,
                        message = reservation,
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse()
                {
                    status = HttpStatusCode.InternalServerError,
                    data = ex,
                    message = ex.Message
                };
            }
        }


        [HttpDelete("{id}")]
        public async Task<BaseResponse> Delete(int id)
        {

            try
            {
                await _reservationService.DeleteReservationAsync(id);
                return new BaseResponse()
                {
                    status = HttpStatusCode.NoContent,
                    data = "",
                    message = "Deleted",
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse()
                {
                    status = HttpStatusCode.InternalServerError,
                    message = ex.Message,
                    data = ex
                };
            }
        }

    }
}
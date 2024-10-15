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
        // [HttpGet]
        // public async Task<BaseResponse> Get()
        // {
        //     try
        //     {
        //         var Reservations = await _reservationService.GetAllReservationsAsync();
        //         if (Reservations == null)
        //         {
        //             return new BaseResponse
        //             {
        //                 status = HttpStatusCode.NotFound,
        //                 data = Reservations
        //             };
        //         }
        //         return new BaseResponse
        //         {
        //             status = HttpStatusCode.OK,
        //             data = Reservations
        //         };
        //     }
        //     catch (Exception ex)
        //     {
        //         return new BaseResponse
        //         {
        //             status = HttpStatusCode.InternalServerError,
        //             message = ex.Message,
        //             data = ex
        //         };
        //     }
        // }

        // [HttpGet("{id}")]
        // public async Task<BaseResponse> Get(int id)
        // {
        //     try
        //     {
        //         var Reservations = await _reservationService.GetReservationByIdAsync(id);
        //         if (Reservations == null)
        //         {
        //             return new BaseResponse
        //             {
        //                 status = HttpStatusCode.NotFound,
        //                 data = Reservations
        //             };
        //         }
        //         return new BaseResponse
        //         {
        //             status = HttpStatusCode.OK,
        //             data = Reservations
        //         };
        //     }
        //     catch (Exception ex)
        //     {
        //         return new BaseResponse
        //         {
        //             status = HttpStatusCode.InternalServerError,
        //             message = ex.Message,
        //             data = ex
        //         };
        //     }
        // }

        // [HttpPost]
        // public async Task<BaseResponse> AddUpdate([FromBody] ReservationVM vM)
        // {
        //     try
        //     {
        //         var anouncement = await _reservationService.AddUpdateReservationAsync(vM);
        //         return new BaseResponse()
        //         {
        //             status = HttpStatusCode.OK,
        //             data = anouncement,
        //             message = "Data Saved",
        //         };

        //     }
        //     catch (Exception ex)
        //     {
        //         return new BaseResponse()
        //         {
        //             status = HttpStatusCode.InternalServerError,
        //             data = ex,
        //             message = ex.Message
        //         };
        //     }
        // }


        // [HttpDelete("{id}")]
        // public async Task<BaseResponse> Delete(int id)
        // {

        //     try
        //     {
        //         await _reservationService.DeleteReservationAsync(id);
        //         return new BaseResponse()
        //         {
        //             status = HttpStatusCode.NoContent,
        //             data = "",
        //             message = "Deleted",
        //         };
        //     }
        //     catch (Exception ex)
        //     {
        //         return new BaseResponse()
        //         {
        //             status = HttpStatusCode.InternalServerError,
        //             message = ex.Message,
        //             data = ex
        //         };
        //     }
        // }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PrescottAppBackend.Api.Model;
using PrescottAppBackend.Domain;
using PrescottAppBackend.Domain.DbModels;

namespace PrescottAppBackend.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AmenityController(IAmenityService _amenity) : ControllerBase
    {
        [HttpGet]
        public async Task<BaseResponse> Get()
        {
            try
            {
                var amenities = await _amenity.GetAllAmenitiesAsync();
                if (amenities == null)
                {
                    return new BaseResponse
                    {
                        status = HttpStatusCode.NotFound,
                        data = amenities
                    };
                }
                return new BaseResponse
                {
                    status = HttpStatusCode.OK,
                    data = amenities
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
                var Amenitys = await _amenity.GetAmenityByIdAsync(id);
                if (Amenitys == null)
                {
                    return new BaseResponse
                    {
                        status = HttpStatusCode.NotFound,
                        data = Amenitys
                    };
                }
                return new BaseResponse
                {
                    status = HttpStatusCode.OK,
                    data = Amenitys
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

        // [HttpPost]
        // public async Task<BaseResponse> AddUpdate([FromBody] AmenityVM vM)
        // {
        //     try
        //     {
        //         // var anouncement = await _amenity.AddUpdateAmenityAsync(vM);
        //         return new BaseResponse()
        //         {
        //             status = HttpStatusCode.OK,
        //             data =  null, //anouncement,
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
        //         // await _amenity.DeleteAmenityAsync(id);
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
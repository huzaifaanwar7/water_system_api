using GBS.Api.Model;
using GBS.Service;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GBS.Api.Controller
{
    [Route("[controller]")]
    [ApiController]
    public class AdminController(IAdminService _adminService) : ControllerBase
    {
        [HttpGet]
        [Route("Lookups")]
        public async Task<IActionResult> GetAllLookups()
        {
            try
            {
                // Retrieve all users using the employee service
                var data = await _adminService.GetAllLookups();
                
                    // Return success response
                    return Ok(new BaseResponse
                    {
                        status = HttpStatusCode.OK,
                        data = data
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
        [HttpGet]
        [Route("StatusList")]
        public async Task<IActionResult> GetStatusListAsync()
        {
            try
            {
                // Retrieve all users using the employee service
                var data = await _adminService.GetStatusList();
                
                    // Return success response
                    return Ok(new BaseResponse
                    {
                        status = HttpStatusCode.OK,
                        data = data
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
        [HttpGet]
        [Route("UserRoleList")]
        public async Task<IActionResult> GetUserRoleListAsync()
        {
            try
            {
                // Retrieve all users using the employee service
                var data = await _adminService.GetUserRoleList();

                // Return success response
                return Ok(new BaseResponse
                {
                    status = HttpStatusCode.OK,
                    data = data
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
        [HttpGet]
        [Route("JobRoleList")]
        public async Task<IActionResult> JobRoleListAsync()
        {
            try
            {
                // Retrieve all users using the employee service
                var data = await _adminService.GetJobRoleList();

                // Return success response
                return Ok(new BaseResponse
                {
                    status = HttpStatusCode.OK,
                    data = data
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
        [HttpGet]
        [Route("TechStackList")]
        public async Task<IActionResult> GetTechStackListAsync()
        {
            try
            {
                // Retrieve all users using the employee service
                var data = await _adminService.GetTechStackList();

                // Return success response
                return Ok(new BaseResponse
                {
                    status = HttpStatusCode.OK,
                    data = data
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

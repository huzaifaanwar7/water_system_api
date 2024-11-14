using Microsoft.AspNetCore.Mvc;
using FirebaseAdmin.Auth;
using GlobularsAdmin.Domain;
using GlobularsAdmin.Domain.DbModels;
using Microsoft.AspNetCore.Authorization;
using GlobularsAdmin.Api.Model;
using System.Net;


namespace GlobularsAdmin.Api
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IUserService _userService, IRoleService _roleService, IJwtUtils _jwtUtils) : ControllerBase
    {


        [HttpGet("user-exists")]
        public async Task<BaseResponse> IsUserExists(string email)
        {
            try
            {

                var user = await _userService.GetUserByUsernameAsync(email);
                return new BaseResponse
                {
                    status = HttpStatusCode.OK,
                    data = user.Id != null
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

        [HttpPost("verify-token")]
        public async Task<BaseResponse> VerifyToken([FromBody] TokenRequest request)
        {
            try
            {
                var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(request.Password);
                var uid = decodedToken.Uid;
                var user = await FirebaseAuth.DefaultInstance.GetUserAsync(uid);
                if (request.Email.Equals(user.Email))
                {
                    var dbUser = await _userService.GetUserByUsernameAsync(user.Email);
                    if (dbUser != null)
                    {
                        return new BaseResponse
                        {
                            status = HttpStatusCode.OK,
                            data = new { dbUser.Id, user.Email, user.DisplayName, request.Password, ExpiresIn = 36000 }
                        };
                    }
                    else
                    {
                        UserVM args = new UserVM()
                        {
                            DisplayName = user.DisplayName,
                            Email = user.Email,
                            EmailVerified = user.EmailVerified,
                            Password = request.Password,
                        };
                        var returnVal = await SignUp(args, request.UserType);
                        return new BaseResponse
                        {
                            status = HttpStatusCode.OK,
                            data = returnVal
                        };
                    }
                }
                return new BaseResponse
                {
                    status = HttpStatusCode.Unauthorized,
                    message = "Unauthorized"
                };
            }
            catch (FirebaseAuthException ex)
            {
                return new BaseResponse
                {
                    status = HttpStatusCode.InternalServerError,
                    message = ex.Message,
                    data = ex
                };
            }
        }

        [HttpPost("sign-up")]
        public async Task<BaseResponse> SignUp([FromBody] UserVM userArgs, string userType)
        {
            try
            {
                var userExist = await _userService.GetUserByUsernameAsync(userArgs.Email);
                if (userExist != null && userExist.Id != null)
                {
                    return new BaseResponse
                    {
                        status = HttpStatusCode.BadRequest,
                        message = "User already exists",
                        data = userExist
                    };
                }
                // UserRecord userRecord = await FirebaseAuth.DefaultInstance.CreateUserAsync(user);
                //var user = await FirebaseAuth.DefaultInstance.GetUserByEmailAsync(userArgs.Email);
                //var token = await FirebaseAuth.DefaultInstance.CreateCustomTokenAsync(user.Uid);
                if(string.IsNullOrEmpty(userArgs.RoleId)) {
                    var role = await _roleService.GetRoleByRolenameAsync("Tenant");
                    userArgs.RoleId = role.Id;
                }
                var newUser = new UserVM()
                {
                    Id = Guid.NewGuid().ToString(),
                    FirstName = userArgs.DisplayName,
                    RoleId = userArgs.RoleId,
                    BuildingId = userArgs.BuildingId,
                    Email = userArgs.Email,
                    Password = userArgs.Password,
                    PhotoUrl = userArgs.PhotoUrl,
                    //FirebaseId = user.Uid,
                    UserSignUpType = userType
                };
                await _userService.AddUserAsync(newUser);
                return await SignIn(newUser);
                // return new BaseResponse
                // {
                //     status = HttpStatusCode.OK,
                //     data = new { newUser.Id, user.Email, user.DisplayName, Password = token, ExpiresIn = 36000 }
                // };
            }
            catch (FirebaseAuthException ex)
            {
                return new BaseResponse
                {
                    status = HttpStatusCode.InternalServerError,
                    message = ex.Message,
                    data = ex
                };
            }
        }

        [AllowAnonymous]
        [HttpPost("signin")]
        public async Task<BaseResponse> SignIn(UserVM userVM)
        {
            try
            {
                var user = await _userService.ValidateUserAsync(userVM);
                if (user != null)
                {
                    var token = _jwtUtils.GenerateJwtToken(user);

                    return new BaseResponse
                    {
                        status = HttpStatusCode.OK,
                        data = new
                        {
                            user.Id,
                            user.BuildingId,
                            user.RoleId,
                            user.Email,
                            displayName = (user.FirstName + ' ' + user.LastName).Trim(),
                            photoURL = user.PhotoUrl,
                            token,
                            expiresIn = 36000
                        }
                    };
                }
                return new BaseResponse
                {
                    status = HttpStatusCode.Unauthorized,
                    message = "Unauthorized"
                };
            }
            catch (FirebaseAuthException ex)
            {
                return new BaseResponse
                {
                    status = HttpStatusCode.InternalServerError,
                    message = ex.Message,
                    data = ex
                };
            }
        }


    }

    public class TokenRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } = "";
        public string UserType { get; set; }
    }
}

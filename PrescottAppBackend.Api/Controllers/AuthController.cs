using Microsoft.AspNetCore.Mvc;
using FirebaseAdmin.Auth;
using FirebaseAuthException = Firebase.Auth.FirebaseAuthException;
using PrescottAppBackend.Domain;
using PrescottAppBackend.Domain.DbModels;
using Microsoft.AspNetCore.Authorization;
using PrescottAppBackend.Api.Model;
using System.Net;


namespace PrescottAppBackend.Api
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IUserService _userService, IRoleService _roleService) : ControllerBase
    {
       

        [HttpGet("user-exists")]
        public async Task<BaseResponse> IsUserExists(string email)
        {
            try
            {

                var user = await _userService.GetUserByUsernameAsync(email);
                {
                    return new BaseResponse
                    {
                        status = HttpStatusCode.OK,
                        data = user.Id != null
                    };
                }
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
                    if(dbUser != null){
                        return new BaseResponse
                        {
                            status = HttpStatusCode.OK,
                            data = new { dbUser.Id, user.Email, user.DisplayName, request.Password, ExpiresIn = 36000 }
                        };
                    }
                    else {
                        UserRecordArgs args = new UserRecordArgs()
                        {
                            Email = user.Email,
                            EmailVerified = user.EmailVerified,
                            Password = request.Password,
                            Disabled = false,
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
        public async Task<BaseResponse> SignUp(UserRecordArgs user, string userType)
        {
            try
            {
                UserRecord userRecord = await FirebaseAuth.DefaultInstance.CreateUserAsync(user);
                var token = await FirebaseAuth.DefaultInstance.CreateCustomTokenAsync(userRecord.Uid);
                var role = await _roleService.GetRoleByRolenameAsync("Admin");
                var newUser = new UserVM()
                {
                    Id = Guid.NewGuid().ToString(),
                    FirstName = user.DisplayName,
                    RoleId = role.Id,
                    Email = user.Email,
                    Password = user.Password,
                    FirebaseId = userRecord.Uid,
                    UserSignUpType = userType
                };
                await _userService.AddUserAsync(newUser);
                return new BaseResponse
                {
                    status = HttpStatusCode.OK,
                    data = new { newUser.Id, user.Email, user.DisplayName, Password = token, ExpiresIn = 36000 }
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

        [HttpPost("signin")]
        public async Task<BaseResponse> SignIn(string email, string password)
        {
            try
            {
                var user = await FirebaseAuth.DefaultInstance.GetUserByEmailAsync(email);
                if (user != null)
                {
                    // Normally you would handle password verification here.
                    // Firebase Admin SDK does not provide direct password verification.
                    // Use Firebase Authentication client SDK on the client side for password sign-in.
                    var token = await FirebaseAuth.DefaultInstance.CreateCustomTokenAsync(user.Uid);

                    return new BaseResponse
                    {
                        status = HttpStatusCode.OK,
                        data = new { token }
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

using Microsoft.AspNetCore.Mvc;
using FirebaseAdmin.Auth;
using FirebaseAuthException = Firebase.Auth.FirebaseAuthException;
using PrescottAppBackend.Domain;
using PrescottAppBackend.Domain.DbModels;
using Microsoft.AspNetCore.Authorization;


namespace PrescottAppBackend.Api
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        public AuthController(IUserService userService, IRoleService roleService){
            _userService = userService;
            _roleService = roleService;
        }

        [HttpGet("user-exists")]
        public async Task<IActionResult> IsUserExists(string email)
        {
            var user = await _userService.GetUserByUsernameAsync(email);
            if(user.Id != null){
                return Ok(true);
            }
            else{
                return Ok(false);
            }
        }

        [HttpPost("verify-token")]
        public async Task<IActionResult> VerifyToken([FromBody] TokenRequest request)
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
                        return Ok(new { dbUser.Id, user.Email, user.DisplayName, request.Password, ExpiresIn = 36000 });
                    }else {
                        UserRecordArgs args = new UserRecordArgs()
                        {
                            Email = user.Email,
                            EmailVerified = user.EmailVerified,
                            Password = request.Password,
                            Disabled = false,
                        };
                        var returnVal = await SignUp(args, request.UserType);
                        return Ok(returnVal);
                    }
                }
                return Unauthorized();
            }
            catch (FirebaseAuthException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp(UserRecordArgs user, string userType)
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
                return Ok(new { newUser.Id, user.Email, user.DisplayName, Password = token, ExpiresIn = 36000 });
            }
            catch (FirebaseAuthException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn(string email, string password)
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
                    return Ok(new { token });
                }
                return Unauthorized();
            }
            catch (FirebaseAuthException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("business-setup")]
        public async Task<IActionResult> BusinessSetUp([FromBody] UserVM user){
            var dbUser = await _userService.GetUserByIdAsync(user.Id ?? "");
            if(dbUser != null){
                dbUser.BusinessName = user.BusinessName;
                dbUser.BusinessType = user.BusinessType;
                dbUser.Mobile = user.Mobile;
                dbUser.Phone = user.Phone;
                dbUser.Address = user.Address;
                dbUser.UpdatedAt = DateTime.Now;
                dbUser.UpdatedBy = dbUser.Id;
                await _userService.UpdateUserAsync(dbUser);
                return Ok(dbUser);
            }
            return NotFound();
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

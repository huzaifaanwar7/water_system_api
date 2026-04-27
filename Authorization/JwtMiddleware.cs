namespace GBS.Api.Authorization;

using Newtonsoft.Json.Linq;
using System.Data;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IJwtUtils _jwtUtils;

    public JwtMiddleware(RequestDelegate next, IJwtUtils jwtUtils)
    {
        _next = next;
        _jwtUtils = jwtUtils;
    }
    
    public async Task Invoke(HttpContext context)
    {
        string token;

        var skipValidation = context.Request.Method == "OPTIONS" 
             || context.Request.Path.ToString().ToLower().Contains("users/login")
             || context.Request.Path.ToString().ToLower().Contains("user/authenticate")
             || context.Request.Path.ToString().ToLower().Contains("public")
             || context.Request.Path.ToString().ToLower().Contains("user/validateotp")
             || context.Request.Path.ToString().ToLower().Contains("download")
             || context.Request.Path.ToString().ToLower().Contains("/www");


        var endpoint = context.GetEndpoint();
        var a = endpoint?.Metadata?.GetMetadata<IAllowAnonymous>();
        var b = endpoint?.Metadata?.GetMetadata<AllowAnonymousAttribute>();
        if (skipValidation || a is not null || b is not null)
        {
            await _next(context);
        }
        else
        {
            if (context.Request.Path.ToString().ToLower().Contains("download") && context.Request.Path.ToString().ToLower().Contains("report"))
            {
                token = context.Request.Query["access_token"];
            }
            else
            {
                token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            }

            // TODO: Check the UserSession Table for the token existence and authorize the user
            //if (userId != null)
            //{
            //    // attach user to context on successful jwt validation
            //    context.Items["User"] = userService.GetById(userId.Value);
            //}
            var userId = _jwtUtils.ValidateJwtToken(token);
            if (userId == null)
            {
                context.Response.Clear();
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync(new JObject()
                {
                    ["status"] = "Unauthorized"
                }.ToString());
            }
            else
            {
                // Set the user in context for [Authorize] attribute to work
                var claims = new[] { new Claim("Id", userId.Value.ToString()) };
                var identity = new ClaimsIdentity(claims, "jwt");
                context.User = new ClaimsPrincipal(identity);

                context.Items["EmployeeId"] = userId;
                await _next(context);
            }
        }
        //await _next(context);
    }
}
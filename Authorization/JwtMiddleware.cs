namespace GBS.Api.Authorization;

using Newtonsoft.Json.Linq;
using System.Data;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using GBS.Service;
using GBS.Data.Model;

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

        var skipValidation = context.Request.Path.ToString().ToLower().Contains("user/authenticate")
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
            var EmployeeId = _jwtUtils.ValidateJwtToken(token);
            if (EmployeeId == null)
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
                context.Items["EmployeeId"] = EmployeeId;
                await _next(context);
                //if (body.Any(u => u["token"].ToString().EqualsIgnoreCase(token)))
                //{
                //    //context.Items["User"] = body.First();
                //    context.Items["User"] = userService.GetById(EmployeeId.Value);
                //    await _next(context);
                //}
                //else
                //{
                //    context.Response.Clear();
                //    context.Response.ContentType = "application/json";
                //    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                //    await context.Response.WriteAsync(new JObject()
                //    {
                //        ["status"] = "Unauthorized"
                //    }.ToString());
                //}

            }
        }
        //await _next(context);
    }
}
// Custom middleware class
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
public class CustomMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IMemoryCache _cache;

    public CustomMiddleware(RequestDelegate next, IMemoryCache cache)
    {
        _next = next;
        _cache = cache;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        if (endpoint != null)
        {
            var allowAnonymous = endpoint.Metadata.GetMetadata<IAllowAnonymous>();
            var authorize = endpoint.Metadata.GetMetadata<IAuthorizeData>();

            if (allowAnonymous != null)
            {
                // Skip authentication if [AllowAnonymous] is present attribute is found
                await _next(context);
                return;
            }
        }
        
        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
        if (authHeader != null && authHeader.StartsWith("Bearer "))
        {
            var token = authHeader.Substring("Bearer ".Length).Trim();

            FirebaseToken decodedToken = null;
            if (!_cache.TryGetValue(token, out decodedToken))
            {
                try
                {
                    decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(5)); // Cache for 5 minutes
                    _cache.Set(token, decodedToken, cacheEntryOptions);
                }
                catch
                {
                    context.Response.StatusCode = 401; // Unauthorized
                    await context.Response.WriteAsync("Unauthorized");
                    return;
                }
            }

            context.Items["User"] = decodedToken;
        }
        else
        {
            context.Response.StatusCode = 401; // Unauthorized
            await context.Response.WriteAsync("Unauthorized");
            return;
        }

        await _next(context);
    }
}

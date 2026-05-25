using System.Security.Claims;

namespace GBS.Api.Helpers
{
    public static class CurrentUser
    {
        public static int? GetUserId(this ClaimsPrincipal user)
        {
            var id = user?.FindFirst("Id")?.Value;
            return int.TryParse(id, out var i) ? i : (int?)null;
        }

        public static string GetRole(this ClaimsPrincipal user)
            => user?.FindFirst(ClaimTypes.Role)?.Value ?? "Fan";
    }
}

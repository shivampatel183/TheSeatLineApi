using System.Security.Claims;
using TheSeatLineApi.Common.Enums;

namespace TheSeatLineApi.AuthServices.Helpers
{
    public static class ClaimsExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            var id = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.Parse(id!);
        }

        public static UserRole GetUserRole(this ClaimsPrincipal user)
        {
            var role = user.FindFirst(ClaimTypes.Role)?.Value;
            return Enum.Parse<UserRole>(role!);
        }
    }
}

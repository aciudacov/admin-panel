using System.Security.Claims;

namespace OSS.IPTV.Ministra.Admin.Services.AppUser
{
    public class AppUser
    {
        public static AppUser Create(ClaimsPrincipal user)
        {
            return new AppUser
            {
                IsAuthenticated = user.Identity?.IsAuthenticated ?? false,
                Login = user.Identity?.Name,
                Name = user.FindFirst("name")?.Value,
            };
        }

        public bool IsAuthenticated { get; init; }
        public string Login { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
    }
}

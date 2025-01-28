using ASG.Api2.Utilities;
using System.Security.Claims;

namespace ASG.ApiService.Repositories;

public interface IUserClaims
{
    ClaimsPrincipal? User { get; }
    string UserId { get; }
    string? UserRole { get; }
    string? Email { get; }
    //long NumberRequests { get; }
    string? ServiceId { get; }
    string? ServiceToken { get; }
}

public class UserClaims(IHttpContextAccessor httpContext) : IUserClaims
{
    private HttpContext? HttpContext => httpContext?.HttpContext;
    public ClaimsPrincipal? User => HttpContext?.User;
    public string UserId => (HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier))!;
    public string? UserRole => HttpContext?.User?.FindFirstValue(ClaimTypes.Role);
    public string? Email => HttpContext?.User?.FindFirstValue(ClaimTypes.Email);
    //public string? Email => HttpContext?.User?.FindFirstValue(ClaimTypes.Email);
    public string? ServiceId => HttpContext?.User?.FindFirstValue(ClaimTypes2.ServiceId);
    public string? ServiceToken => HttpContext?.User?.FindFirstValue(ClaimTypes2.ServiceToken);

}


using System.Security.Claims;
using games_application.Contracts;
using Microsoft.AspNetCore.Http;

namespace TbdDevelop.GameTrove.Games.Infrastructure.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public string? UserId => httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    public string? Email => httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value;
    public bool IsAuthenticated => httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
}
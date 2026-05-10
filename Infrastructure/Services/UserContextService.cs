using System.Security.Claims;

using Application.Common.Interfaces;

using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services;

public class UserContextService : IUserContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContextService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? UserId =>
        _httpContextAccessor.HttpContext?.User?
            .FindFirst(ClaimTypes.NameIdentifier)?.Value;

    public bool IsInRole(string role) =>
    _httpContextAccessor.HttpContext?.User?.IsInRole(role) ?? false;


}



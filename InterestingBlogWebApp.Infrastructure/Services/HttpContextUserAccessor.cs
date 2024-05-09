using InterestingBlogWebApp.Application.Common_Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

public class HttpContextUserAccessor : IUserAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextUserAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetCurrentUserId()
    {
        return _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }

    public string GetCurrentUserName()
    {
        return _httpContextAccessor.HttpContext?.User.Identity?.Name;
    }
}
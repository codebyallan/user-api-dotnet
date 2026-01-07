using System.Security.Claims;
using User.Api.Application.DTOs.Request;

namespace User.Api.Application.Interfaces;

public interface IAuthService
{
    Task<ClaimsPrincipal?> Login(AuthRequest request);
}

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using User.Api.Application.DTOs.Request;
using User.Api.Application.Interfaces;
using User.Api.Application.Notifications;
using User.Api.Domain.Interfaces;
using User.Api.Domain.ValueObjects;

namespace User.Api.Application.Services;

public class AuthService(IUserRepository _repository, NotificationContext _context, IHashPasswordService _hashPasswordService) : IAuthService
{
    public async Task<ClaimsPrincipal?> Login(AuthRequest request)
    {
        Domain.Entities.User? user = await _repository.GetByEmailAsync(new Email(request.Email));

        if (user is null || !_hashPasswordService.VerifyPassword(user, user.Password.Value, request.Password))
        {
            _context.AddNotification("Invalid Credentials", "Invalid credentials!");
            return null;
        }
        List<Claim>? claims = [
            new(ClaimTypes.NameIdentifier,user.Id.ToString()),
            new(ClaimTypes.GivenName,user.FullName.FirstName),
            new(ClaimTypes.Surname,user.FullName.LastName),
            new(ClaimTypes.Email, user.EmailAddress.Address)
        ];
        ClaimsIdentity? claimsIdentity = new(
            claims, CookieAuthenticationDefaults.AuthenticationScheme
            );

        return new ClaimsPrincipal(claimsIdentity);
    }
}

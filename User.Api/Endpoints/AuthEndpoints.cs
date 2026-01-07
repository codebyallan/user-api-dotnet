using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using User.Api.Application.DTOs.Request;
using User.Api.Application.DTOs.Response;
using User.Api.Application.Interfaces;
using User.Api.Application.Notifications;

namespace User.Api.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var auth = app.MapGroup("/auth").WithTags("Auth");
        auth.MapPost("/login", async (AuthRequest request, IAuthService authService, HttpContext httpContext, NotificationContext context) =>
{
    ClaimsPrincipal? principal = await authService.Login(request);

    if (principal == null || !context.IsValid)
    {
        return Results.BadRequest(new ErrorResponse(context.Notifications));
    }

    await httpContext.SignInAsync(
        CookieAuthenticationDefaults.AuthenticationScheme,
        principal,
        new AuthenticationProperties { IsPersistent = true });

    return Results.NoContent();
}).WithName("Login")
.WithSummary("Login user")
.WithDescription("Login user with email and password!")
.Produces(StatusCodes.Status204NoContent)
.Produces<ErrorResponse>(StatusCodes.Status400BadRequest);

        auth.MapPost("/logout", async (HttpContext httpContext) =>
        {
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Results.Ok();
        }).WithName("logout")
        .WithSummary("Logout user")
        .WithDescription("Logout user!");
    }
}

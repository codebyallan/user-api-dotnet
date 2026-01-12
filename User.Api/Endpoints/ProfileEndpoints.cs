using System.Security.Claims;
using User.Api.Application.DTOs.Request;
using User.Api.Application.DTOs.Response;
using User.Api.Application.Extensions;
using User.Api.Application.Interfaces;
using User.Api.Application.Notifications;

namespace User.Api.Endpoints;

public static class ProfileEndpoints
{
    public static void MapProfileEndpoints(this IEndpointRouteBuilder app)
    {
        var profile = app.MapGroup("/profile").WithTags("Profile").RequireAuthorization();

        profile.MapGet("/", async (IUserService userService, ClaimsPrincipal claimsUser, NotificationContext context) =>
        {
            Guid? userId = claimsUser.GetId();
            if (userId is null)
            {
                return Results.Unauthorized();
            }
            var user = await userService.GetUserByIdAsync(userId.Value);
            return context.IsValid ? Results.Ok(user) : context.Notifications.Any(n => n.Key == "Not Found") ? Results.NotFound(new ErrorResponse(context.Notifications)) : Results.BadRequest(new ErrorResponse(context.Notifications));
        }).WithName("GetCurrentUser")
        .WithSummary("Get logged in user profile")
        .WithDescription("Returns the profile of the currently authenticated user based on the cookie/token.")
        .Produces<UserResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest);

        profile.MapPut("/", async (UpdateUserRequest request, ClaimsPrincipal claimsUser, IUserService userService, NotificationContext context) =>
        {
            Guid? userId = claimsUser.GetId();
            if (userId is null)
            {
                return Results.Unauthorized();
            }
            var user = await userService.UpdateUserAsync(userId.Value, request);
            return context.IsValid ? Results.Ok(user) : context.Notifications.Any(n => n.Key == "Not Found") ? Results.NotFound(new ErrorResponse(context.Notifications)) : Results.BadRequest(new ErrorResponse(context.Notifications));
        }).WithName("UpdatedCurrentUser")
        .WithSummary("Update logged in user profile")
        .WithDescription("Updates the profile of the currently authenticated user based on the cookie/token.")
        .Produces<UserResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest);

        profile.MapDelete("/", async (IUserService userService, ClaimsPrincipal claimsUser, NotificationContext context) =>
        {
            Guid? userId = claimsUser.GetId();
            if (userId is null)
            {
                return Results.Unauthorized();
            }
            var user = await userService.SoftDeleteUserAsync(userId.Value);
            return context.IsValid ? Results.Ok(user) : context.Notifications.Any(n => n.Key == "Not Found") ? Results.NotFound(new ErrorResponse(context.Notifications)) : Results.BadRequest(new ErrorResponse(context.Notifications));
        }).WithName("DeletedCurrentUser")
        .WithSummary("Soft Delete logged in user profile")
        .WithDescription("Deletes the profile of the currently authenticated user based on the cookie/token.")
        .Produces<UserResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest);
    }
}

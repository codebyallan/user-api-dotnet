using Microsoft.AspNetCore.Http.HttpResults;
using User.Api.Application.DTOs.Request;
using User.Api.Application.Interfaces;
using User.Api.Application.Notifications;
using User.Api.Persistence;

namespace User.Api.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var user = app.MapGroup("/users");

        user.MapPost("/", async (CreateUserRequest request, IUserService userService, NotificationContext context) =>
        {
            var result = await userService.CreateUserAsync(request);
            return context.IsValid ? Results.Ok(new { success = true, data = result }) : Results.BadRequest(new { success = false, errors = context.Notifications });
        }
        ).WithName("CreatedUser");

        user.MapGet("/", async (IUserService userService, NotificationContext context) =>
        {
            var users = await userService.GetAllUsersAsync();
            return context.IsValid ? Results.Ok(new { success = true, data = users }) : Results.BadRequest(new { success = false, errors = context.Notifications });
        }).WithName("GetAllUsers");

        user.MapGet("/{id:guid}", async (Guid id, IUserService userService, NotificationContext context) =>
        {
            var user = await userService.GetUserByIdAsync(id);
            return context.IsValid ? Results.Ok(new { success = true, data = user }) : Results.BadRequest(new { success = false, errors = context.Notifications });
        }).WithName("GetUserById");

        user.MapPut("/{id:guid}", async (Guid id, UpdateUserRequest request, IUserService userService, NotificationContext context) =>
        {
            var user = await userService.UpdateUserAsync(id, request);
            return context.IsValid ? Results.Ok(new { success = true, data = user }) : Results.BadRequest(new { success = false, errors = context.Notifications });
        }).WithName("UpdatedUser");

        user.MapDelete("/{id:guid}", async (Guid id, IUserService userService, NotificationContext context) =>
        {
            var user = await userService.SoftDeleteUserAsync(id);
            return context.IsValid ? Results.Ok(new { success = true, data = user }) : Results.BadRequest(new { success = false, errors = context.Notifications });
        }).WithName("DeletedUser");

    }
}

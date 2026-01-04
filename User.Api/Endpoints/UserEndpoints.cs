using Microsoft.AspNetCore.Http.HttpResults;
using User.Api.Application.DTOs.Request;
using User.Api.Application.DTOs.Response;
using User.Api.Application.Interfaces;
using User.Api.Application.Notifications;
using User.Api.Persistence;

namespace User.Api.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var user = app.MapGroup("/users").WithTags("Users");

        user.MapPost("/", async (CreateUserRequest request, IUserService userService, NotificationContext context) =>
        {
            var result = await userService.CreateUserAsync(request);
            return context.IsValid ? Results.Created($"/users/{result!.Id}", result) : Results.BadRequest(new ErrorResponse(context.Notifications));
        }
        ).WithName("CreatedUser")
        .WithSummary("Create a new User")
        .WithDescription("Create a new User with the given information and returns the created User.")
        .Produces<UserResponse>(StatusCodes.Status201Created)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest);

        user.MapGet("/", async (IUserService userService, NotificationContext context) =>
        {
            var users = await userService.GetAllUsersAsync();
            return context.IsValid ? Results.Ok(users) : Results.BadRequest(new ErrorResponse(context.Notifications));
        }).WithName("GetAllUsers")
        .WithSummary("Get all Users")
        .WithDescription("Returns all Users in the database.")
        .Produces<IEnumerable<UserResponse>>(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest);

        user.MapGet("/{id:guid}", async (Guid id, IUserService userService, NotificationContext context) =>
        {
            var user = await userService.GetUserByIdAsync(id);
            return context.IsValid ? Results.Ok(user) : context.Notifications.Any(n => n.Key == "Not Found") ? Results.NotFound(new ErrorResponse(context.Notifications)) : Results.BadRequest(new ErrorResponse(context.Notifications));
        }).WithName("GetUserById")
        .WithSummary("Get a User by ID")
        .WithDescription("Returns a User with the given ID in the database.")
        .Produces<UserResponse>(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest);

        user.MapPut("/{id:guid}", async (Guid id, UpdateUserRequest request, IUserService userService, NotificationContext context) =>
        {
            var user = await userService.UpdateUserAsync(id, request);
            return context.IsValid ? Results.Ok(user) : context.Notifications.Any(n => n.Key == "Not Found") ? Results.NotFound(new ErrorResponse(context.Notifications)) : Results.BadRequest(new ErrorResponse(context.Notifications));
        }).WithName("UpdatedUser")
        .WithSummary("Update a User by ID")
        .WithDescription("Updates a User with the given ID in the database.")
        .Produces<UserResponse>(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest);

        user.MapDelete("/{id:guid}", async (Guid id, IUserService userService, NotificationContext context) =>
        {
            var user = await userService.SoftDeleteUserAsync(id);
            return context.IsValid ? Results.Ok(user) : context.Notifications.Any(n => n.Key == "Not Found") ? Results.NotFound(new ErrorResponse(context.Notifications)) : Results.BadRequest(new ErrorResponse(context.Notifications));
        }).WithName("DeletedUser")
        .WithSummary("Soft Delete a User by ID")
        .WithDescription("Deletes a User with the given ID in the database.")
        .Produces<UserResponse>(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest);

    }
}

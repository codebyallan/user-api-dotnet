namespace User.Api.Application.DTOs.Response;

public record UserResponse
(
    Guid Id,
    string FirstName,
    string LastName,
    string EmailAddress
);

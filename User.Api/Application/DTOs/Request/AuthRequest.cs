using User.Api.Domain.ValueObjects;

namespace User.Api.Application.DTOs.Request;

public record AuthRequest(string Email, string Password);

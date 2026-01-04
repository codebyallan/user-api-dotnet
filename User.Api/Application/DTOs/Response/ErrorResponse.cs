using User.Api.Domain.Notifications;

namespace User.Api.Application.DTOs.Response;

public record ErrorResponse(IEnumerable<NotificationMessage> Errors);

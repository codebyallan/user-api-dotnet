using User.Api.Application.DTOs.Response;

namespace User.Api.Application.Mappings;

public static class UserMapper
{
    public static UserResponse ToResponse(this Domain.Entities.User user) => new(user.Id, user.FullName.FirstName, user.FullName.FirstName, user.EmailAddress.Address);
}

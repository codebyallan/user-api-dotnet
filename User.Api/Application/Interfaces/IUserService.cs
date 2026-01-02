using User.Api.Application.DTOs.Request;
using User.Api.Application.DTOs.Response;

namespace User.Api.Application.Interfaces;

public interface IUserService
{
    Task<UserResponse> CreateUserAsync(CreateUserRequest request);
    Task<IEnumerable<UserResponse>> GetAllUsersAsync();
    Task<UserResponse> GetUserByIdAsync(Guid id);
    Task<UserResponse> GetUserByEmailAsync(string email);
    Task<UserResponse> UpdateUserAsync(Guid id, UpdateUserRequest request);
    Task<UserResponse> SoftDeleteUserAsync(Guid id);
}

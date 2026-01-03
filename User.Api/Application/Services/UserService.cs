using User.Api.Application.DTOs.Request;
using User.Api.Application.DTOs.Response;
using User.Api.Application.Interfaces;
using User.Api.Domain.Interfaces;
using User.Api.Domain.ValueObjects;

namespace User.Api.Application.Services;

public class UserService(IUserRepository _userRepository) : IUserService
{
    public async Task<UserResponse> CreateUserAsync(CreateUserRequest request)
    {
        Domain.Entities.User? userWithEmail = await _userRepository.GetByEmailAsync(new Email(request.Email));
        if (userWithEmail is not null) throw new Exception("Email already exists  in the system");
        Domain.Entities.User newUser = new(Guid.NewGuid(), new FullName(request.FirstName, request.LastName), new Email(request.Email), new Password(request.Password));
        await _userRepository.AddAsync(newUser);
        return new UserResponse(
            newUser.Id,
            newUser.FullName.FirstName,
            newUser.FullName.LastName,
            newUser.EmailAddress.EmailAddress
        );
    }

    public async Task<IEnumerable<UserResponse>> GetAllUsersAsync()
    {
        IEnumerable<Domain.Entities.User> users = await _userRepository.GetAllAsync();
        return users.Select(u => new UserResponse(
            u.Id,
            u.FullName.FirstName,
            u.FullName.LastName,
            u.EmailAddress.EmailAddress
        )).ToList();
    }

    public async Task<UserResponse> GetUserByEmailAsync(string email)
    {
        Domain.Entities.User? user = await _userRepository.GetByEmailAsync(new Email(email)) ?? throw new Exception("User not found!");
        return new UserResponse(
            user.Id,
            user.FullName.FirstName,
            user.FullName.LastName,
            user.EmailAddress.EmailAddress
        );
    }

    public async Task<UserResponse> GetUserByIdAsync(Guid id)
    {
        Domain.Entities.User? user = await _userRepository.GetByIdAsync(id) ?? throw new Exception("User not found!");
        return new UserResponse(
            user.Id,
            user.FullName.FirstName,
            user.FullName.LastName,
            user.EmailAddress.EmailAddress
        );
    }

    public async Task<UserResponse> SoftDeleteUserAsync(Guid id)
    {
        Domain.Entities.User? user = await _userRepository.GetByIdAsync(id) ?? throw new Exception("User not found!");
        Domain.Entities.User deletedUser = user.MarkAsDeleted();
        await _userRepository.UpdateAsync(deletedUser);
        return new UserResponse(
            user.Id,
            user.FullName.FirstName,
            user.FullName.LastName,
            user.EmailAddress.EmailAddress
        );
    }

    public async Task<UserResponse> UpdateUserAsync(Guid id, UpdateUserRequest request)
    {
        Domain.Entities.User? user = await _userRepository.GetByIdAsync(id) ?? throw new Exception("User not found!");
        if (!string.IsNullOrEmpty(request.Email) && request.Email != user.EmailAddress.EmailAddress)
        {
            Domain.Entities.User? userWithEmail = await _userRepository.GetByEmailAsync(new Email(request.Email));
            if (userWithEmail != null) throw new Exception("Email already exists  in the system");
            user = user.ChangeEmailAddress(request.Email);
        }
        if (!string.IsNullOrEmpty(request.FirstName)) user = user.ChangeFirstName(request.FirstName);
        if (!string.IsNullOrEmpty(request.LastName)) user = user.ChangeLastName(request.LastName);
        if (!string.IsNullOrEmpty(request.Password)) user = user.ChangePassword(request.Password);
        await _userRepository.UpdateAsync(user);
        return new UserResponse(
            user.Id,
            user.FullName.FirstName,
            user.FullName.LastName,
            user.EmailAddress.EmailAddress
        );
    }
}

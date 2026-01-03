using User.Api.Application.DTOs.Request;
using User.Api.Application.DTOs.Response;
using User.Api.Application.Interfaces;
using User.Api.Application.Notifications;
using User.Api.Domain.Interfaces;
using User.Api.Domain.ValueObjects;

namespace User.Api.Application.Services;

public class UserService(IUserRepository _userRepository, NotificationContext _context) : IUserService
{
    public async Task<UserResponse?> CreateUserAsync(CreateUserRequest request)
    {
        Domain.Entities.User? userWithEmail = await _userRepository.GetByEmailAsync(new Email(request.Email));
        if (userWithEmail is not null)
        {
            _context.AddNotification("Email", "Email already exists in the system!");
        }
        Domain.Entities.User newUser = new(Guid.NewGuid(), new FullName(request.FirstName, request.LastName), new Email(request.Email), new Password(request.Password));
        if (!newUser.IsValid || !_context.IsValid)
        {
            _context.AddNotifications(newUser);
            return null;
        }
        await _userRepository.AddAsync(newUser);
        return new UserResponse(
            newUser.Id,
            newUser.FullName.FirstName,
            newUser.FullName.LastName,
            newUser.EmailAddress.Address
        );
    }

    public async Task<IEnumerable<UserResponse>> GetAllUsersAsync()
    {
        IEnumerable<Domain.Entities.User> users = await _userRepository.GetAllAsync();
        return users.Select(u => new UserResponse(
            u.Id,
            u.FullName.FirstName,
            u.FullName.LastName,
            u.EmailAddress.Address
        )).ToList();
    }

    public async Task<UserResponse?> GetUserByEmailAsync(string email)
    {
        Domain.Entities.User? user = await _userRepository.GetByEmailAsync(new Email(email));
        if (user is null)
        {
            _context.AddNotification("Not Found", "User not found!");
            return null;
        }
        return new UserResponse(
            user.Id,
            user.FullName.FirstName,
            user.FullName.LastName,
            user.EmailAddress.Address
        );
    }

    public async Task<UserResponse?> GetUserByIdAsync(Guid id)
    {
        Domain.Entities.User? user = await _userRepository.GetByIdAsync(id);
        if (user is null)
        {
            _context.AddNotification("Not Found", "User not found!");
            return null;
        }
        return new UserResponse(
            user.Id,
            user.FullName.FirstName,
            user.FullName.LastName,
            user.EmailAddress.Address
        );
    }

    public async Task<UserResponse?> SoftDeleteUserAsync(Guid id)
    {
        Domain.Entities.User? user = await _userRepository.GetByIdAsync(id);
        if (user is null)
        {
            _context.AddNotification("Not Found", "User not found!");
            return null;
        }
        user.MarkAsDeleted();
        await _userRepository.UpdateAsync(user);
        return new UserResponse(
            user.Id,
            user.FullName.FirstName,
            user.FullName.LastName,
            user.EmailAddress.Address
        );
    }

    public async Task<UserResponse?> UpdateUserAsync(Guid id, UpdateUserRequest request)
    {
        Domain.Entities.User? user = await _userRepository.GetByIdAsync(id);
        if (user is null)
        {
            _context.AddNotification("Not Found", "User not found!");
            return null;
        }
        if (!string.IsNullOrEmpty(request.Email) && request.Email != user.EmailAddress.Address)
        {
            Domain.Entities.User? userWithEmail = await _userRepository.GetByEmailAsync(new Email(request.Email));
            if (userWithEmail != null)
            {
                _context.AddNotification("Email", "Email already exists  in the system!");
            }
            user.ChangeEmailAddress(request.Email);
        }
        if (!string.IsNullOrEmpty(request.FirstName)) user.ChangeFirstName(request.FirstName);
        if (!string.IsNullOrEmpty(request.LastName)) user.ChangeLastName(request.LastName);
        if (!string.IsNullOrEmpty(request.Password)) user.ChangePassword(request.Password);
        if (!user.IsValid || !_context.IsValid)
        {
            _context.AddNotifications(user);
            return null;
        }
        await _userRepository.UpdateAsync(user);
        return new UserResponse(
            user.Id,
            user.FullName.FirstName,
            user.FullName.LastName,
            user.EmailAddress.Address
        );
    }
}

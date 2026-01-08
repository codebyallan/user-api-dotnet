using User.Api.Application.DTOs.Request;
using User.Api.Application.DTOs.Response;
using User.Api.Application.Interfaces;
using User.Api.Application.Mappings;
using User.Api.Application.Notifications;
using User.Api.Domain.Interfaces;
using User.Api.Domain.ValueObjects;

namespace User.Api.Application.Services;

public class UserService(IUserRepository _userRepository, NotificationContext _context, IHashPasswordService _hashPasswordService, IUserFactory _userFactory) : IUserService
{
    public async Task<UserResponse?> CreateUserAsync(CreateUserRequest request)
    {
        Domain.Entities.User? userWithEmail = await _userRepository.GetByEmailAsync(new Email(request.Email));
        if (userWithEmail is not null)
        {
            _context.AddNotification("Email", "Email already exists in the system!");
        }
        Domain.Entities.User newUser = _userFactory.Create(request.FirstName, request.LastName, request.Email, request.Password);
        if (!newUser.IsValid || !_context.IsValid)
        {
            _context.AddNotifications(newUser);
            return null;
        }
        if (newUser.Password.IsValid)
        {
            string passwordHashed = _hashPasswordService.HashPassword(newUser, request.Password);
            newUser.ApplyPasswordHash(passwordHashed);
        }
        await _userRepository.AddAsync(newUser);
        return newUser.ToResponse();
    }

    public async Task<IEnumerable<UserResponse>> GetAllUsersAsync()
    {
        IEnumerable<Domain.Entities.User> users = await _userRepository.GetAllAsync();
        return users.Select(u => u.ToResponse()).ToList();
    }

    public async Task<UserResponse?> GetUserByEmailAsync(string email)
    {
        Domain.Entities.User? user = await _userRepository.GetByEmailAsync(new Email(email));
        if (user is null)
        {
            _context.AddNotification("Not Found", "User not found!");
            return null;
        }
        return user.ToResponse();
    }

    public async Task<UserResponse?> GetUserByIdAsync(Guid id)
    {
        Domain.Entities.User? user = await _userRepository.GetByIdAsync(id);
        if (user is null)
        {
            _context.AddNotification("Not Found", "User not found!");
            return null;
        }
        return user.ToResponse();
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
        return user.ToResponse();
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
        if (!string.IsNullOrEmpty(request.Password))
        {
            user.ChangePassword(request.Password);
            if (user.Password.IsValid)
            {
                string passwordHashed = _hashPasswordService.HashPassword(user, request.Password);
                user.ApplyPasswordHash(passwordHashed);
            }
        }
        if (!user.IsValid || !_context.IsValid)
        {
            _context.AddNotifications(user);
            return null;
        }
        await _userRepository.UpdateAsync(user);
        return user.ToResponse();
    }
}

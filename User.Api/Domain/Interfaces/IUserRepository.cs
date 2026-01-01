using User.Api.Domain.ValueObjects;

namespace User.Api.Domain.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<Entities.User>> GetAllAsync();
    Task<Entities.User?> GetByIdAsync(Guid id);
    Task<Entities.User?> GetByEmailAsync(Email email);
    Task AddAsync(Entities.User user);
    Task UpdateAsync(Entities.User user);
    Task SoftDeleteAsync(Entities.User user);
}

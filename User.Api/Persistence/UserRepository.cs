using MongoDB.Driver;
using User.Api.Domain.Interfaces;
using User.Api.Domain.ValueObjects;

namespace User.Api.Persistence;

public class UserRepository(MongoDbContext _dbContext) : IUserRepository
{
    public async Task AddAsync(Domain.Entities.User user)
    {
        await _dbContext.Users.InsertOneAsync(user);
    }

    public async Task<IEnumerable<Domain.Entities.User>> GetAllAsync()
    {
        return await _dbContext.Users.Find(u => u.DeletedAt == null).ToListAsync();
    }

    public async Task<Domain.Entities.User?> GetByEmailAsync(Email email)
    {
        return await _dbContext.Users.Find(u => u.DeletedAt == null && u.EmailAddress == email).FirstOrDefaultAsync();
    }

    public async Task<Domain.Entities.User?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Users.Find(u => u.DeletedAt == null && u.Id == id).FirstOrDefaultAsync();
    }

    public async Task SoftDeleteAsync(Domain.Entities.User user)
    {
        await _dbContext.Users.ReplaceOneAsync(u => u.Id == user.Id, user);
    }

    public async Task UpdateAsync(Domain.Entities.User user)
    {
        await _dbContext.Users.ReplaceOneAsync(u => u.Id == user.Id, user);
    }
}

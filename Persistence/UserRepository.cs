using Microsoft.EntityFrameworkCore;
using TrefingreGymControl.Api.Domain.Exceptions;
using TrefingreGymControl.Api.Domain.Users;

namespace TrefingreGymControl.Api.Persistence
{
    public class UserRepository : IUserRepository
    {
        private readonly ILogger<UserRepository> _logger;
        private readonly TFGymControlDbContext _dbContext;

        public UserRepository(TFGymControlDbContext dbContext, ILogger<UserRepository> logger)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task AddAsync(TFGCUser user)
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("User with ID {UserId} added successfully.", user.Id);
        }

        public async Task DeleteUserAsync(Guid userId, bool isAdmin = false, CancellationToken cancellationToken = default)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user is null)
                throw new NoUserWithIdFoundException(userId.ToString(), _logger);

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync(cancellationToken);
            if (isAdmin)
                _logger.LogInformation($"Admin deleted user with ID '{userId}' successfully.", userId);
            else
                _logger.LogInformation($"User with ID '{userId}' deleted successfully.", userId);

        }

        public async Task DeleteUserAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId) ?? throw new NoUserWithIdFoundException(userId.ToString(), _logger);
            user.Delete();
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<TFGCUser>> GetAllUsersAsync(bool includeDeleted, CancellationToken cancellationToken = default)
        {
            if (includeDeleted)
                return await _dbContext.Users.ToListAsync(cancellationToken);
            else
                return await _dbContext.Users.Where(u => !u.IsDeleted).ToListAsync(cancellationToken);
        }

        public Task<TFGCUser?> GetByEmailAsync(string email) => _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

        public async Task<TFGCUser> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
            return user ?? throw new NoUserWithIdFoundException(userId.ToString(), _logger);
        }

        public async Task SaveChangesAsync() => await _dbContext.SaveChangesAsync();

        public async Task UpdateUserAsync(Guid userId, string fullname, CancellationToken cancellationToken = default)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user is null)
                throw new NoUserWithIdFoundException(userId.ToString(), _logger);

            user.UpdateFullname(fullname);

            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("User with ID {UserId} updated successfully.", userId);
        }
    }
}
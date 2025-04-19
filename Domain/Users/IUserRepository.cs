using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrefingreGymControl.Api.Domain.Users
{
    public interface IUserRepository
    {
        Task<TFGCUser?> GetByEmailAsync(string email);
        Task AddAsync(TFGCUser user);
        Task SaveChangesAsync();
        Task<TFGCUser> GetByIdAsync(Guid userId, CancellationToken cancellationToken);
        Task UpdateUserAsync(Guid userId, string fullname, CancellationToken cancellationToken = default);
        Task DeleteUserAsync(Guid userId, bool isAdmin = false, CancellationToken cancellationToken = default);
        Task<List<TFGCUser>> GetAllUsersAsync(bool includeDeleted, CancellationToken cancellationToken = default);
        Task DeleteUserAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
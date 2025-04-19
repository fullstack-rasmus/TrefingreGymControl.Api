using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrefingreGymControl.Api.Domain.Users
{
    public interface IUserService
    {
        Task RegisterUserAsync(string fullname, string email, string password, string role = "User");
        Task<string> AuthenticateUserAsync(string email, string password, CancellationToken cancellationToken = default);
        Task<TFGCUser> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task UpdateUserAsync(Guid userId, string fullname, CancellationToken cancellationToken = default);
        Task DeleteUserAsync(Guid userId, bool isAdmin = false, CancellationToken cancellationToken = default);
        Task<List<TFGCUser>> GetAllUsersAsync(bool includeDeleted, CancellationToken cancellationToken = default);
        Task DeleteUserAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
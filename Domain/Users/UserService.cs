using System.Security.Claims;
using FastEndpoints.Security;
using TrefingreGymControl.Api.Domain.Exceptions;

namespace TrefingreGymControl.Api.Domain.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, ILogger<UserService> logger)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        public async Task RegisterUserAsync(string fullname, string email, string password, string role = "User")
        {
            var existing = await _userRepository.GetByEmailAsync(email);
            if (existing is not null)
                throw new EmailAlreadyInUseException(email, _logger);

            var user = TFGCUser.Register(fullname, email, password, role);
            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();
        }

        public async Task<string> AuthenticateUserAsync(string email, string password, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid credentials.");
            }

            var jwt = JwtBearer.CreateToken(opt =>
            {
                opt.SigningKey = "your-super-secret-signing-key-121213131414";
                opt.ExpireAt = DateTime.UtcNow.AddHours(1);
                opt.User.Claims.Add(new Claim[] {
                    new("UserId", user.Id.ToString()),
                    new(ClaimTypes.Name, user.Fullname),
                    new(ClaimTypes.Email, user.Email),
                    new("role", user.Role)
                });
            });

            return jwt;
        }

        public async Task<TFGCUser> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _userRepository.GetByIdAsync(userId, cancellationToken);
        }

        public async Task UpdateUserAsync(Guid userId, string fullname, CancellationToken cancellationToken = default)
        {
            await _userRepository.UpdateUserAsync(userId, fullname, cancellationToken);
        }

        public Task DeleteUserAsync(Guid userId, bool isAdmin = false, CancellationToken cancellationToken = default)
        {
            return _userRepository.DeleteUserAsync(userId, isAdmin, cancellationToken);
        }

        public async Task<List<TFGCUser>> GetAllUsersAsync(bool includeDeleted, CancellationToken cancellationToken = default)
        {
            return await _userRepository.GetAllUsersAsync(includeDeleted, cancellationToken);   
        }

        public async Task DeleteUserAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            await _userRepository.DeleteUserAsync(userId, cancellationToken);
        }
    }
}
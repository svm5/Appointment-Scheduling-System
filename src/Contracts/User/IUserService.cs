namespace Contracts.User;

public interface IUserService
{
    Task<UserDetails> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken);
    Task<LoginResponse> LoginUser(LoginRequest request, CancellationToken cancellationToken);
    Task<ICollection<UserDetails>> GetAllUsersAsync(CancellationToken cancellationToken);
    Task<UserDetails> GetUserByIdAsync(int id, CancellationToken cancellationToken);
    Task DeleteUserByIdAsync(int id, CancellationToken cancellationToken);
}
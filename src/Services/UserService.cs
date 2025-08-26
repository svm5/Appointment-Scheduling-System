using System.Security.Claims;
using AutoMapper;
using Contracts.Security;
using Contracts.User;
using Domain.User;
using Exceptions;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Services;

public class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;
    private readonly IJwtUtil _jwtUtil;

    public UserService(ApplicationDbContext context, IMapper mapper, IJwtUtil jwtUtil)
    {
        _context = context;
        _mapper = mapper;
        _jwtUtil = jwtUtil;
    }
    
    public async Task<UserDetails> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken)
    {
        User user = _mapper.Map<User>(request);
        foreach (var roleName in request.Roles)
        {
            Role role = await _context.Roles.SingleAsync(r => r.Name == roleName, cancellationToken);;
            user.Roles.Add(role);
        }

        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<UserDetails>(user);
    }

    public async Task<LoginResponse> LoginUser(LoginRequest request, CancellationToken cancellationToken)
    {
        User? user = await _context.Users
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(u => u.Username == request.Username, cancellationToken);

        if (user == null)
        {
            throw new InvalidLoginCredentialsException("Login credentials are invalid.");
        }

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
        {
            throw new InvalidLoginCredentialsException("Login credentials are invalid.");
        }

        var claims = new List<Claim>();
        claims.Add(new Claim(ClaimTypes.Name, user.Username));
        foreach (var role in user.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role.Name));
        }
        
        return new LoginResponse(_jwtUtil.GenerateJwtToken(claims));
    }

    public async Task<ICollection<UserDetails>> GetAllUsersAsync(CancellationToken cancellationToken)
    {
        List<User> users = await _context.Users
            .Include(u => u.Roles)
            .ToListAsync(cancellationToken);
        
        return _mapper.Map<List<UserDetails>>(users);
    }

    public async Task<UserDetails> GetUserByIdAsync(int id, CancellationToken cancellationToken)
    {
        User user = await _context.Users
            .Include(u => u.Roles)
            .SingleAsync(o => o.Id == id, cancellationToken);
        
        return _mapper.Map<UserDetails>(user);
    }

    public async Task DeleteUserByIdAsync(int id, CancellationToken cancellationToken)
    {
        await _context.Users.Where(u => u.Id == id).ExecuteDeleteAsync(cancellationToken);
    }
}
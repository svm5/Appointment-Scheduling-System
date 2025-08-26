using AutoMapper;
using Contracts.User;
using Domain.User;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Services;

public class RoleService : IRoleService
{
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;

    public RoleService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<RoleDetails> CreateRoleAsync(CreateRoleRequest request, CancellationToken cancellationToken)
    {
        Role role = _mapper.Map<Role>(request);
        
        await _context.Roles.AddAsync(role, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<RoleDetails>(role);
    }

    public async Task<RoleDetails> GetRoleByNameAsync(string name, CancellationToken cancellationToken)
    {
        Role role = await _context.Roles.SingleAsync(
            r=> r.Name == name, cancellationToken);
        
        return _mapper.Map<RoleDetails>(role);
    }
}
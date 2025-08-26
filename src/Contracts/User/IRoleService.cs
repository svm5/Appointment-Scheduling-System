using Contracts.Appointment.SlotHelpers;
using Domain.User;

namespace Contracts.User;

public interface IRoleService
{
    Task<RoleDetails> CreateRoleAsync(CreateRoleRequest request, CancellationToken cancellationToken);
    Task<RoleDetails> GetRoleByNameAsync(string name, CancellationToken cancellationToken);
}
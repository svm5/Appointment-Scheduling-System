using AutoMapper;
using Contracts.User;
using Domain.User;

namespace Mapping;

public class RoleMapping : Profile
{
    public RoleMapping()
    {
        CreateMap<CreateRoleRequest, Role>();
        CreateMap<Role, RoleDetails>();
    }
}
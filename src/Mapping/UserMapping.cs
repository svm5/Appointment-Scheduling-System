using AutoMapper;
using Contracts.User;
using Domain.User;

namespace Mapping;

public class UserMapping : Profile
{
    public UserMapping()
    {
        CreateMap<Role, string>().ConvertUsing(r => r.Name);
        CreateMap<CreateUserRequest, User>()
            .ForMember(dest => dest.Roles, opt => opt.Ignore());
        CreateMap<User, UserDetails>();
    }
}
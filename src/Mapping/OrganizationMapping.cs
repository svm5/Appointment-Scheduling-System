using AutoMapper;
using Contracts.Organization;
using Domain;

namespace Mapping;

public class OrganizationMapping : Profile
{
    public OrganizationMapping()
    {
        CreateMap<Place, int>().ConvertUsing(p => p.Id);
        CreateMap<Organization, OrganizationDetails>();
        CreateMap<CreateOrganizationRequest, Organization>();
    }
}
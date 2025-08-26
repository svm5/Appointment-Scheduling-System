using AutoMapper;
using Contracts.Organization;
using Domain;
using Domain.Appointment;

namespace Mapping;

public class PlaceMapping : Profile
{
    public PlaceMapping()
    {
        CreateMap<Slot, int>().ConvertUsing(s => s.Id);
        CreateMap<CreatePlaceRequest, Place>();
            // .ForMember(p => p.OrganizationId, opt => opt.MapFrom(r => r.OrganizationId));
        CreateMap<Place, PlaceDetails>();
    }
}
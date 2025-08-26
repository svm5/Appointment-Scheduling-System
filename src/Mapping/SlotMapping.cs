using AutoMapper;
using Contracts.Appointment.SlotHelpers;
using Domain.Appointment;

namespace Mapping;

public class SlotMapping : Profile
{
    public SlotMapping()
    {
        // CreateMap<Slot, int>().ConvertUsing(s => s.Id);
        CreateMap<Appointment, int>().ConvertUsing(a => a.Id);
        CreateMap<CreateOneSlotRequest, Slot>();
        CreateMap<Slot, OneSlotDetails>();
            // .ForMember(s => s.PlaceId, o => o.MapFrom(s => s.PlaceId))
            // .ForMember(s => s.Details, opt => opt.MapFrom(src => new SlotDetails(src.Id, src.From, src.To)));
        // .ForMember(p => p.OrganizationId, opt => opt.MapFrom(r => r.OrganizationId));
        CreateMap<Slot, SlotDetails>();
    }
}
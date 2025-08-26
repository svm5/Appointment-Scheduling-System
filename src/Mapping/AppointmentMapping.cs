using AutoMapper;
using Contracts.Appointment.AppointmentHelpers;
using Domain.Appointment;
using Domain.User;

namespace Mapping;

public class AppointmentMapping : Profile
{
    public AppointmentMapping()
    {
        CreateMap<User, int>().ConvertUsing(u => u.Id);
        CreateMap<CreateAppointmentRequest, Appointment>();
        CreateMap<Appointment, AppointmentDetails>();
    }
}
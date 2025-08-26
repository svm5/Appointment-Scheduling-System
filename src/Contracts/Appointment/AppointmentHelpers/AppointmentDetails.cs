namespace Contracts.Appointment.AppointmentHelpers;

public record AppointmentDetails(int Id, int SlotId, ICollection<int> Users);
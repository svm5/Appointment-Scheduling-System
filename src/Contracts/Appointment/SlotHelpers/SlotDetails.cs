namespace Contracts.Appointment.SlotHelpers;

public record SlotDetails(int Id, DateTime From, DateTime To, int? AppointmentId);
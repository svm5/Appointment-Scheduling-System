namespace Contracts.Appointment.SlotHelpers;

public record OneSlotDetails(int PlaceId, int Id, DateTime From, DateTime To, int? AppointmentId);
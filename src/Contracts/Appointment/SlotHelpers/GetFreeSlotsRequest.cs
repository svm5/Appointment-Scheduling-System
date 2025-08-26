namespace Contracts.Appointment.SlotHelpers;

public record GetFreeSlotsRequest(int PlaceId, DateTime From, DateTime To);
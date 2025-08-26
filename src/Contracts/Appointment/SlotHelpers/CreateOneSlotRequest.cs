namespace Contracts.Appointment.SlotHelpers;

public record CreateOneSlotRequest(int PlaceId, DateTime From, DateTime To);
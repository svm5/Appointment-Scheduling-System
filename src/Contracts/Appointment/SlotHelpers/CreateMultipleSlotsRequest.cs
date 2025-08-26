namespace Contracts.Appointment.SlotHelpers;

public record CreateMultipleSlotsRequest(int PlaceId, int SlotsAmount, DateTime From, TimeSpan Duration);

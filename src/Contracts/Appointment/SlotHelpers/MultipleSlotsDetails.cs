namespace Contracts.Appointment.SlotHelpers;

public record MultipleSlotsDetails(int PlaceId, ICollection<SlotDetails> Details);
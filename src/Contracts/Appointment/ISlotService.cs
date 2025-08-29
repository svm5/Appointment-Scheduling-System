using Contracts.Appointment.SlotHelpers;

namespace Contracts.Appointment;

public interface ISlotService
{
   Task<OneSlotDetails> CreateSlotAsync(CreateOneSlotRequest request, CancellationToken cancellationToken);
   Task<MultipleSlotsDetails> CreateSlotRangeAsync(CreateMultipleSlotsRequest request, CancellationToken cancellationToken);
   Task<OneSlotDetails> GetSlotByIdAsync(int slotId, CancellationToken cancellationToken);
   Task<ICollection<OneSlotDetails>> GetSlotsAsync(GetAllSlotsRequest request, CancellationToken cancellationToken);
   // Task<ICollection<OneSlotDetails>> GetAllSlotsAsync(CancellationToken cancellationToken);
   // Task<ICollection<OneSlotDetails>> GetAllSlotsInPlaceAsync(int placeId, CancellationToken cancellationToken);
   // Task<ICollection<OneSlotDetails>> GetFreeSlotsAsync(GetFreeSlotsRequest request, CancellationToken cancellationToken);
   Task DeleteSlotByIdAsync(int slotId, CancellationToken cancellationToken);
}
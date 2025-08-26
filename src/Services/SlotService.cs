using AutoMapper;
using Contracts.Appointment;
using Contracts.Appointment.SlotHelpers;
using Contracts.Organization;
using Domain.Appointment;
using Exceptions;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Services;

public class SlotService : ISlotService
{
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;

    public SlotService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<OneSlotDetails> CreateSlotAsync(CreateOneSlotRequest request, CancellationToken cancellationToken)
    {
        Slot slot = _mapper.Map<Slot>(request);
        
        await _context.Slots.AddAsync(slot, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<OneSlotDetails>(slot);
    }

    public async Task<MultipleSlotsDetails> CreateSlotRangeAsync(CreateMultipleSlotsRequest request, CancellationToken cancellationToken)
    {
        List<Slot> slots = new List<Slot>();
        DateTime from = request.From;
        if (CheckPlaceIsBusy(request.PlaceId, from, from + request.Duration * request.SlotsAmount))
        {
            throw new PlaceIsBusyException("Place with id ... is busy during ... to ...");
        }
        
        for (int i = 0; i < request.SlotsAmount; i++)
        {
            DateTime to = from.Add(request.Duration);
            CreateOneSlotRequest newSlot = new CreateOneSlotRequest(request.PlaceId, from, to);
            slots.Add(_mapper.Map<Slot>(newSlot));
            
            from = to;
        }

        await _context.Slots.AddRangeAsync(slots, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        return new MultipleSlotsDetails(request.PlaceId, _mapper.Map<List<SlotDetails>>(slots));
    }

    public async Task<OneSlotDetails> GetSlotByIdAsync(int slotId, CancellationToken cancellationToken)
    {
        Slot slot = await _context.Slots
            .Include(s => s.Appointment)
            .SingleAsync(s => s.Id == slotId, cancellationToken);
        
        return _mapper.Map<OneSlotDetails>(slot);
    }

    public async Task<ICollection<OneSlotDetails>> GetAllSlotsAsync(CancellationToken cancellationToken)
    {
        List<Slot> slots = await _context.Slots
            .Include(s => s.Appointment)
            .ToListAsync(cancellationToken);
        
        return _mapper.Map<List<OneSlotDetails>>(slots);
    }

    public async Task<ICollection<OneSlotDetails>> GetAllSlotsInPlaceAsync(int placeId, CancellationToken cancellationToken)
    {
        List<Slot> slots = await _context.Slots
            .Where(s => s.PlaceId == placeId)
            .Include(s => s.Appointment)
            .ToListAsync(cancellationToken);
        
        return _mapper.Map<List<OneSlotDetails>>(slots);
    }

    public async Task<ICollection<OneSlotDetails>> GetFreeSlotsAsync(GetFreeSlotsRequest request, CancellationToken cancellationToken)
    {
        var slots = await _context.Slots
            .Where(s => (s.PlaceId == request.PlaceId) && (s.From >= request.From) && (s.To <= request.To))
            .Include(s => s.Appointment)
            .ToListAsync(cancellationToken);
        slots = slots.Where(s => s.Appointment == null).ToList();
        
        return _mapper.Map<List<OneSlotDetails>>(slots);
    }

    public async Task DeleteSlotByIdAsync(int slotId, CancellationToken cancellationToken)
    {
        await _context.Slots.Where(p => p.Id == slotId).ExecuteDeleteAsync(cancellationToken);
    }

    private bool CheckPlaceIsBusy(int placeId, DateTime from, DateTime to)
    {
        var cnt = _context.Slots
            .Count(s => (s.PlaceId == placeId) && !(s.From >= to || from >= s.To));
        return cnt != 0;
    }
}
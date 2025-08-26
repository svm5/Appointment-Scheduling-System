using AutoMapper;
using Contracts.Organization;
using Contracts.User;
using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Services;

public class PlaceService : IPlaceService
{
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;
    private const int _maxSlotsAmount = 10;

    public PlaceService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<PlaceDetails> CreatePlaceAsync(CreatePlaceRequest request, CancellationToken cancellationToken)
    {
        Place place = _mapper.Map<Place>(request);
        await _context.Places.AddAsync(place, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<PlaceDetails>(place);
    }

    public async Task<PlaceDetails> GetPlaceByIdAsync(int id, CancellationToken cancellationToken)
    {
        Place place = await _context.Places
            .Include(p => p.Slots.Where(s => s.PlaceId == id))
            .SingleAsync(p => p.Id == id, cancellationToken);
        
        return _mapper.Map<PlaceDetails>(place);
    }

    public async Task<ICollection<PlaceDetails>> GetAllPlacesAsync(GetPlacesFilters filters, CancellationToken cancellationToken)
    {
        List<Place> places;
        
        var placesTemp =_context.Places
            .Include(p => p.Slots);

        if (filters.OrganizationId != null)
        {
            places = await placesTemp
                .Where(p => p.OrganizationId == filters.OrganizationId)
                .ToListAsync(cancellationToken);
        }
        else {
            places = await placesTemp
            .ToListAsync(cancellationToken);
        }
        
        for (int i = 0; i < places.Count; i++)
        {
            places[i].Slots = places[i].Slots
                .OrderBy(s => s.To)
                .Take(_maxSlotsAmount)
                .ToList();
        }
        
        return _mapper.Map<List<PlaceDetails>>(places);
    }

    public async Task DeletePlaceByIdAsync(int id, CancellationToken cancellationToken)
    {
        await _context.Places.Where(p => p.Id == id).ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<PlaceDetails?> FindPlaceByIdAsync(int id, CancellationToken cancellationToken)
    {
        Place? place = await _context.Places
            .Include(p => p.Slots.Where(s => s.PlaceId == id))
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        if (place == null)
        {
            return null;
        }
        
        return _mapper.Map<PlaceDetails>(place);
    }
}
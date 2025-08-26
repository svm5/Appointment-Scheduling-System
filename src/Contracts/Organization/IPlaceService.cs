using Contracts.Organization;
using Contracts.User;

namespace Contracts.Organization;

public interface IPlaceService
{
    Task<PlaceDetails> CreatePlaceAsync(CreatePlaceRequest request, CancellationToken cancellationToken);
    Task<PlaceDetails> GetPlaceByIdAsync(int id, CancellationToken cancellationToken);
    Task<ICollection<PlaceDetails>> GetAllPlacesAsync(GetPlacesFilters filters, CancellationToken cancellationToken);
    Task DeletePlaceByIdAsync(int id, CancellationToken cancellationToken);
    Task<PlaceDetails?> FindPlaceByIdAsync(int id, CancellationToken cancellationToken);
}
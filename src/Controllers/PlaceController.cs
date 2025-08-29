using Contracts.Organization;
using Contracts.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Controllers;

[ApiController]
[Authorize(Roles = "Admin,SeniorManager")]
[Route("api/v1/place")]
public class PlaceController : ControllerBase
{
    private readonly IPlaceService _placeService;
    private readonly IPermissionService _permissionService;

    public PlaceController(IPlaceService placeService, IPermissionService permissionService)
    {
        _placeService = placeService;
        _permissionService = permissionService;
    }
    
    /// <summary>
    /// Creates a place.
    /// </summary>
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PlaceDetails))]
    public async Task<IActionResult> Create([FromBody]CreatePlaceRequest request, CancellationToken cancellationToken)
    {
        Console.WriteLine(User.Identity.Name);
        if (!(await _permissionService.CheckPlaceAccess(User.Identity.Name, request.OrganizationId)))
        {
            return Forbid();
        }
        // Console.WriteLine(HttpContext.User);
        var placeDetails = await _placeService.CreatePlaceAsync(request, cancellationToken);
        
        return CreatedAtAction(nameof(GetPlaceById), new { id = placeDetails.Id }, placeDetails);
    }

    /// <summary>
    /// Returns a specific place.
    /// </summary>
    /// <param name="id"></param>
    [HttpGet("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PlaceDetails))]
    public async Task<IActionResult> GetPlaceById([FromRoute] int id, CancellationToken cancellationToken)
    {
        if (!(await _permissionService.CheckPlaceAccess(User.Identity.Name, id)))
        {
            return Forbid();
        }
        
        var placeDetails = await _placeService.GetPlaceByIdAsync(id, cancellationToken);
        return Ok(placeDetails);
    }

    /// <summary>
    /// Returns all places.
    /// </summary>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PlaceDetails>))]
    public async Task<IActionResult> GetPlaces(CancellationToken cancellationToken)
    {
        var filters = await _permissionService.GetPlacesFilters(User.Identity.Name);
        var places = await _placeService.GetAllPlacesAsync(filters, cancellationToken);
        
        return Ok(places);
    }

    /// <summary>
    /// Deletes a specific place.
    /// </summary>
    /// <param name="id"></param>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeletePlace([FromRoute] int id, CancellationToken cancellationToken)
    {
        var placeDetails = await _placeService.FindPlaceByIdAsync(id, cancellationToken);
        if (placeDetails == null)
        {
            return NoContent();
        }

        if (!(await _permissionService.CheckPlaceAccess(User.Identity.Name, placeDetails.OrganizationId)))
        {
            return Forbid();
        }
        
        await _placeService.DeletePlaceByIdAsync(id, cancellationToken);
        
        return NoContent();
    }
}
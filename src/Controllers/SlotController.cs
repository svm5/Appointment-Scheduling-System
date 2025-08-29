using Contracts.Appointment;
using Contracts.Appointment.SlotHelpers;
using Contracts.Organization;
using Contracts.User;
using Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Controllers;

[ApiController]
[Route("api/v1/slot")]
[Authorize]
public class SlotController : ControllerBase
{
    private readonly ISlotService _slotService;
    private readonly IPermissionService _permissionService;

    public SlotController(ISlotService slotService, IPermissionService permissionService)
    {
        _slotService = slotService;
        _permissionService = permissionService;
    }
    
    /// <summary>
    /// Creates a slot.
    /// </summary>
    [HttpPost("one")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OneSlotDetails))]
    public async Task<IActionResult> Create([FromBody]CreateOneSlotRequest request, CancellationToken cancellationToken)
    {
        if (!(await _permissionService.CheckAdminOrManagerInOrganization(User.Identity.Name, request.PlaceId)))
        {
            return Forbid();
        }
        
        var slotDetails = await _slotService.CreateSlotAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetSlotById), new { id = slotDetails.Id }, slotDetails);
    }
    
    /// <summary>
    /// Creates a multiple slots.
    /// </summary>
    [HttpPost("range")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(MultipleSlotsDetails))]
    public async Task<IActionResult> CreateRange([FromBody]CreateMultipleSlotsRequest request, CancellationToken cancellationToken)
    {
        if (!(await _permissionService.CheckAdminOrManagerInOrganization(User.Identity.Name, request.PlaceId)))
        {
            return Forbid();
        }
        
        var slotsDetails = await _slotService.CreateSlotRangeAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetAllSlots), new { }, slotsDetails);
    }
    
    /// <summary>
    /// Returns a specific slot.
    /// </summary>
    /// <param name="id"></param>
    [HttpGet("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OneSlotDetails))]
    public async Task<IActionResult> GetSlotById([FromRoute] int id, CancellationToken cancellationToken)
    {
        if (!(await _permissionService.CheckSlotPermissionAccess(User.Identity.Name, id)))
        {
            return Forbid();
        }
        
        var slotDetails = await _slotService.GetSlotByIdAsync(id, cancellationToken);
        return Ok(slotDetails);
    }
    
    /// <summary>
    /// Returns all slots with filters.
    /// </summary>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<OneSlotDetails>))]
    public async Task<IActionResult> GetAllSlots(int? placeId, bool? isFree, DateTime? from, DateTime? to, CancellationToken cancellationToken)
    {
        int? organizationId = await _permissionService.GetOrganizationId(User.Identity.Name);
        var request = GetAllSlotsRequest.Builder;
        if (organizationId.HasValue)
        {
            request = request.WithOrganizationId(organizationId.Value);
        }
        if (placeId.HasValue)
        {
            request = request.WithPlaceId(placeId.Value);
        }
        if (isFree.HasValue)
        {
            request = request.WithIsFree(isFree.Value);
        }
        if (from.HasValue)
        {
            request = request.WithFrom(from.Value);
        }

        if (to.HasValue)
        {
            request = request.WithTo(to.Value);
        }
        
        return Ok(await _slotService.GetSlotsAsync(request.Build(), cancellationToken));
    }
    
    /// <summary>
    /// Deletes a specific slot.
    /// </summary>
    /// <param name="id"></param>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteSlot([FromRoute] int id, CancellationToken cancellationToken)
    {
        try
        {
            bool checkAccess = await _permissionService.CheckAdminOrManagerInOrganization(User.Identity.Name, id);
            if (!checkAccess)
            {
                return Forbid();
            }
            await _slotService.DeleteSlotByIdAsync(id, cancellationToken);
        }
        catch (NotFoundException exception)
        {
            return NoContent();
        }
        
        return NoContent();
    }
}
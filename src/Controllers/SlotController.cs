using Contracts.Appointment;
using Contracts.Appointment.SlotHelpers;
using Contracts.Organization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Controllers;

[ApiController]
[Route("api/v1/slot")]
public class SlotController : ControllerBase
{
    private readonly ISlotService _slotService;

    public SlotController(ISlotService slotService)
    {
        _slotService = slotService;
    }
    
    /// <summary>
    /// Creates a slot.
    /// </summary>
    [HttpPost("one")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OneSlotDetails))]
    public async Task<IActionResult> Create([FromBody]CreateOneSlotRequest request, CancellationToken cancellationToken)
    {
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
        var slotDetails = await _slotService.GetSlotByIdAsync(id, cancellationToken);
        
        return Ok(slotDetails);
    }
    
    /// <summary>
    /// Returns all slots with filters.
    /// </summary>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<OneSlotDetails>))]
    public async Task<IActionResult> GetAllSlots(int? placeId, CancellationToken cancellationToken)
    {
        if (placeId == null)
        {
            return Ok(await _slotService.GetAllSlotsAsync(cancellationToken));
        }
        
        return Ok(await _slotService.GetAllSlotsInPlaceAsync(placeId.Value, cancellationToken));
    }
    
    /// <summary>
    /// Returns all slots with filters.
    /// </summary>
    [HttpGet("free")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<OneSlotDetails>))]
    public async Task<IActionResult> GetFreeSlots(int placeId, DateTime from, DateTime to, CancellationToken cancellationToken)
    {
        var request = new GetFreeSlotsRequest(placeId, from, to);
        
        return Ok(await _slotService.GetFreeSlotsAsync(request, cancellationToken));
    }
    
    /// <summary>
    /// Deletes a specific slot.
    /// </summary>
    /// <param name="id"></param>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteUser([FromRoute] int id, CancellationToken cancellationToken)
    {
        await _slotService.DeleteSlotByIdAsync(id, cancellationToken);
        
        return NoContent();
    }
}
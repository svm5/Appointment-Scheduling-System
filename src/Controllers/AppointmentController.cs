using Contracts.Appointment;
using Contracts.Appointment.AppointmentHelpers;
using Contracts.Organization;
using Domain.Appointment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Controllers;

[ApiController]
[Route("api/v1/appointment")]
public class AppointmentController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }
    
    /// <summary>
    /// Creates an appointment.
    /// </summary>
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AppointmentDetails))]
    public async Task<IActionResult> Create([FromBody]CreateAppointmentRequest request, CancellationToken cancellationToken)
    {
        var appointmentDetails = await _appointmentService.CreateAppointmentAsync(request, cancellationToken);
        
        return CreatedAtAction(nameof(GetAppointmentById), new { id = appointmentDetails.Id }, appointmentDetails);
    }
    
    /// <summary>
    /// Returns a specific appoinment.
    /// </summary>
    /// <param name="id"></param>
    [HttpGet("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AppointmentDetails))]
    public async Task<IActionResult> GetAppointmentById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var appointmentDetails = await _appointmentService.GetAppointmentByIdAsync(id, cancellationToken);
        
        return Ok(appointmentDetails);
    }
    
    /// <summary>
    /// Returns all appointments.
    /// </summary>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<AppointmentDetails>))]
    public async Task<IActionResult> GetAppointments(CancellationToken cancellationToken)
    {
        var appointments = await _appointmentService.GetAppointmentsAsync(cancellationToken);
        
        return Ok(appointments);
    }

    /// <summary>
    /// Adds user to appointment.
    /// </summary>
    /// <param name="id"></param>
    [HttpPut("user/add/{id}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AppointmentDetails))]
    public async Task<IActionResult> AddUser(int id, int userId, CancellationToken cancellationToken)
    {
        var appointmentDetails = await _appointmentService.AddUserToAppointmentAsync(id, userId, cancellationToken);
        
        return Ok(appointmentDetails);
    }
    
    /// <summary>
    /// Removes user to appointment.
    /// </summary>
    /// <param name="id"></param>
    [HttpPut("user/remove/{id}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AppointmentDetails))]
    public async Task<IActionResult> RemoveUser(int id, int userId, CancellationToken cancellationToken)
    {
        var appointmentDetails = await _appointmentService.RemoveUserFromAppointmentAsync(id, userId, cancellationToken);
        
        return Ok(appointmentDetails);
    }
    
    /// <summary>
    /// Returns ids of a specific user appointments.
    /// </summary>
    /// <param name="id"></param>
    [HttpGet("user/{id}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<int>))]
    public async Task<IActionResult> GetUsersAppointments([FromRoute] int id, CancellationToken cancellationToken)
    {
        return Ok(await _appointmentService.GetAppointmentsIdsByUserIdAsync(id, cancellationToken));
    }
    
    /// <summary>
    /// Deletes a specific organization.
    /// </summary>
    /// <param name="id"></param>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteOrganization([FromRoute] int id, CancellationToken cancellationToken)
    {
        await _appointmentService.DeleteAppointmentByIdAsync(id, cancellationToken);
        
        return NoContent();
    }
}
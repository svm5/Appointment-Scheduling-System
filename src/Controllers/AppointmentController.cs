using Contracts.Appointment;
using Contracts.Appointment.AppointmentHelpers;
using Contracts.Organization;
using Contracts.User;
using Domain.Appointment;
using Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Controllers;

[ApiController]
[Route("api/v1/appointment")]
[Authorize]
public class AppointmentController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;
    private readonly IPermissionService _permissionService;

    public AppointmentController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }
    
    /// <summary>
    /// Creates an appointment.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin,SeniorManager,Manager")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AppointmentDetails))]
    public async Task<IActionResult> Create([FromBody]CreateAppointmentRequest request, CancellationToken cancellationToken)
    {
        if (!(await _permissionService.CheckAdminOrManagerInOrganizationBySlotId(User.Identity.Name, request.SlotId)))
        {
            return Forbid();
        }
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
        if (!(await _permissionService.CheckAppointmentPermissionAccess(User.Identity.Name, id)))
        {
            return Forbid();
        }
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
        // admin - all, manager - only in organization, user - only where him
        int? organizationId = await _permissionService.GetOrganizationId(User.Identity.Name);
        int? userId = null;
        if (await _permissionService.HasUserRole(User.Identity.Name))
        {
            userId = await _permissionService.GetUserId(User.Identity.Name);
        }
        
        var request = GetAllAppointmentsRequest.Builder
            .WithOrganizationId(organizationId)
            .WithUserId(userId)
            .Build();
        var appointments = await _appointmentService.GetAppointmentsAsync(request, cancellationToken);
        
        return Ok(appointments);
    }

    /// <summary>
    /// Adds user to appointment.
    /// </summary>
    /// <param name="id"></param>
    [HttpPut("user/add/{id}")]
    [Authorize(Roles = "Admin,SeniorManager,Manager")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AppointmentDetails))]
    public async Task<IActionResult> AddUser(int id, int userId, CancellationToken cancellationToken)
    {
        if (!(await _permissionService.CheckAppointmentPermissionAccess(User.Identity.Name, id)))
        {
            return Forbid();
        }
        
        var appointmentDetails = await _appointmentService.AddUserToAppointmentAsync(id, userId, cancellationToken);
        return Ok(appointmentDetails);
    }
    
    /// <summary>
    /// Removes user to appointment.
    /// </summary>
    /// <param name="id"></param>
    [HttpPut("user/remove/{id}")]
    [Authorize(Roles = "Admin,SeniorManager,Manager")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AppointmentDetails))]
    public async Task<IActionResult> RemoveUser(int id, int userId, CancellationToken cancellationToken)
    {
        if (!(await _permissionService.CheckAppointmentPermissionAccess(User.Identity.Name, id)))
        {
            return Forbid();
        }
        
        var appointmentDetails = await _appointmentService.RemoveUserFromAppointmentAsync(id, userId, cancellationToken);
        return Ok(appointmentDetails);
    }
    
    // /// <summary>
    // /// Returns ids of a specific user appointments.
    // /// </summary>
    // /// <param name="id"></param>
    // [HttpGet("user/{id}")]
    // [Produces("application/json")]
    // [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<int>))]
    // public async Task<IActionResult> GetUsersAppointments([FromRoute] int id, CancellationToken cancellationToken)
    // {
    //     return Ok(await _appointmentService.GetAppointmentsIdsByUserIdAsync(id, cancellationToken));
    // }
    
    /// <summary>
    /// Deletes a specific organization.
    /// </summary>
    /// <param name="id"></param>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,SeniorManager,Manager")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteAppointment([FromRoute] int id, CancellationToken cancellationToken)
    {
        try
        {
            bool checkAccess = await _permissionService.CheckAppointmentPermissionAccess(User.Identity.Name, id);
            if (!checkAccess)
            {
                return Forbid();
            }
        }
        catch (NotFoundException)
        {
            return NoContent();
        }
        
        return NoContent();
    }
}
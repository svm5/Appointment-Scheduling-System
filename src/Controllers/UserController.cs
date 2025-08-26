using Contracts.Organization;
using Contracts.User;
using Domain.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Controllers;

[ApiController]
[Route("api/v1/user")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IPermissionService _permissionService;

    public UserController(IUserService userService, IPermissionService permissionService)
    {
        _userService = userService;
        _permissionService = permissionService;
    }
    
    /// <summary>
    /// Creates a user.
    /// </summary>
    [HttpPost("create/user")]
    [AllowAnonymous]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserDetails))]
    public async Task<IActionResult> Create([FromBody]CreateUserRequest request, CancellationToken cancellationToken)
    {
        request.Roles.Add(RoleTypes.User.ToString());
        var userDetails = await _userService.CreateUserAsync(request, cancellationToken);
        
        return CreatedAtAction(nameof(GetUserById), new { id = userDetails.Id }, userDetails);
    }
    
    /// <summary>
    /// Creates a admin.
    /// </summary>
    [HttpPost("create/admin")]
    [Authorize(Roles = "Admin")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserDetails))]
    public async Task<IActionResult> CreateAdmin([FromBody]CreateUserRequest request, CancellationToken cancellationToken)
    {
        request.Roles.Add(RoleTypes.Admin.ToString());
        var userDetails = await _userService.CreateUserAsync(request, cancellationToken);
        
        return CreatedAtAction(nameof(GetUserById), new { id = userDetails.Id }, userDetails);
    }
    
    /// <summary>
    /// Creates a manager.
    /// </summary>
    [HttpPost("create/manager")]
    [Authorize(Roles = "Admin,SeniorManager")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserDetails))]
    public async Task<IActionResult> CreateManager([FromBody]CreateUserRequest request, CancellationToken cancellationToken)
    {
        if (!(await _permissionService.CheckCreateManagerAccess(User.Identity.Name, request.OrganizationId)))
        {
            return Forbid();
        }
        
        request.Roles.Add(RoleTypes.Manager.ToString());
        var userDetails = await _userService.CreateUserAsync(request, cancellationToken);
        
        return CreatedAtAction(nameof(GetUserById), new { id = userDetails.Id }, userDetails);
    }
    
    /// <summary>
    /// Creates a senior manager.
    /// </summary>
    [HttpPost("create/senior-manager")]
    [Authorize(Roles = "Admin,SeniorManager")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserDetails))]
    public async Task<IActionResult> CreateSeniorManager([FromBody]CreateUserRequest request, CancellationToken cancellationToken)
    {
        if (!(await _permissionService.CheckCreateManagerAccess(User.Identity.Name, request.OrganizationId)))
        {
            return Forbid();
        }
        
        request.Roles.Add(RoleTypes.SeniorManager.ToString());
        var userDetails = await _userService.CreateUserAsync(request, cancellationToken);
        
        return CreatedAtAction(nameof(GetUserById), new { id = userDetails.Id }, userDetails);
    }
    
    [HttpPost("login")]
    [AllowAnonymous]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponse))]
    public async Task<IActionResult> Login([FromBody]LoginRequest request, CancellationToken cancellationToken)
    {
        return Ok(await _userService.LoginUser(request, cancellationToken));
    }

    /// <summary>
    /// Returns a specific user.
    /// </summary>
    /// <param name="id"></param>
    [HttpGet("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDetails))]
    public async Task<IActionResult> GetUserById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var userDetails = await _userService.GetUserByIdAsync(id, cancellationToken);
        
        return Ok(userDetails);
    }

    /// <summary>
    /// Returns all users.
    /// </summary>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UserDetails>))]
    public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
    {
        var users = await _userService.GetAllUsersAsync(cancellationToken);
        
        return Ok(users);
    }

    /// <summary>
    /// Deletes a specific user.
    /// </summary>
    /// <param name="id"></param>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteUser([FromRoute] int id, CancellationToken cancellationToken)
    {
        await _userService.DeleteUserByIdAsync(id, cancellationToken);
        
        return NoContent();
    }
}
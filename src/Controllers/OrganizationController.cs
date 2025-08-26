using Contracts.Organization;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace Controllers;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/v1/organization")]
public class OrganizationController : ControllerBase
{
    private readonly IOrganizationService _organizationService;

    public OrganizationController(IOrganizationService organizationService)
    {
        _organizationService = organizationService;
    }
    
    /// <summary>
    /// Creates an organization.
    /// </summary>
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OrganizationDetails))]
    public async Task<IActionResult> Create([FromBody]CreateOrganizationRequest request, CancellationToken cancellationToken)
    {
        var organizationDetails = await _organizationService.CreateOrganizationAsync(request, cancellationToken);
        
        return CreatedAtAction(nameof(GetOrganizationById), new { id = organizationDetails.Id }, organizationDetails);
    }

    /// <summary>
    /// Returns a specific organization.
    /// </summary>
    /// <param name="id"></param>
    [HttpGet("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrganizationDetails))]
    public async Task<IActionResult> GetOrganizationById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var organizationDetails = await _organizationService.GetOrganizationByIdAsync(id, cancellationToken);
        
        return Ok(organizationDetails);
    }

    /// <summary>
    /// Returns all organizations.
    /// </summary>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<OrganizationDetails>))]
    public async Task<IActionResult> GetOrganizations(CancellationToken cancellationToken)
    {
        var organizations = await _organizationService.GetAllOrganizationsAsync(cancellationToken);
        
        return Ok(organizations);
    }

    /// <summary>
    /// Deletes a specific organization.
    /// </summary>
    /// <param name="id"></param>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteOrganization([FromRoute] int id, CancellationToken cancellationToken)
    {
        await _organizationService.DeleteOrganizationByIdAsync(id, cancellationToken);
        
        return NoContent();
    }
}
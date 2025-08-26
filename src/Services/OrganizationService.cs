using AutoMapper;
using Contracts.Organization;
using Domain;
using Exceptions;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Services;

public class OrganizationService : IOrganizationService
{
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;

    public OrganizationService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<OrganizationDetails> CreateOrganizationAsync(CreateOrganizationRequest createOrganizationRequest, CancellationToken cancellationToken)
    {
        Organization organization = _mapper.Map<Organization>(createOrganizationRequest);
        await _context.Organizations.AddAsync(organization, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<OrganizationDetails>(organization);
    }
    //  System.InvalidOperationException: Sequence contains no elements.
    public async Task<OrganizationDetails> GetOrganizationByIdAsync(int id, CancellationToken cancellationToken)
    {
        Organization organization;
        try
        {
            organization = await _context.Organizations
                .Include(o => o.Places.Where(p => p.OrganizationId == id))
                .SingleAsync(o => o.Id == id, cancellationToken);
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine(e.Message);
            if (e.Message != "Sequence contains no elements.") throw;
            var message = $"Organization with id {id} was not found";
            throw new NotFoundException(message);

        }
        // Console.WriteLine(organization.Places.Count);
        return _mapper.Map<OrganizationDetails>(organization);
    }

    public async Task<ICollection<OrganizationDetails>> GetAllOrganizationsAsync(CancellationToken cancellationToken)
    {
        List<Organization> organizations = await _context.Organizations
            .Include(o => o.Places)
            .ToListAsync(cancellationToken);
        
        return _mapper.Map<List<OrganizationDetails>>(organizations);
    }

    public async Task DeleteOrganizationByIdAsync(int id, CancellationToken cancellationToken)
    {
        await _context.Organizations.Where(o => o.Id == id).ExecuteDeleteAsync(cancellationToken);
    }
}
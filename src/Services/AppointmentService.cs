using AutoMapper;
using Contracts.Appointment;
using Contracts.Appointment.AppointmentHelpers;
using Contracts.Organization;
using Domain.Appointment;
using Domain.User;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Services;

public class AppointmentService : IAppointmentService
{
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;

    public AppointmentService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<AppointmentDetails> CreateAppointmentAsync(CreateAppointmentRequest request, CancellationToken cancellationToken)
    {
        Appointment appointment = _mapper.Map<Appointment>(request);
        
        await _context.Appointments.AddAsync(appointment, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<AppointmentDetails>(appointment);
    }

    public async Task<ICollection<AppointmentDetails>> GetAppointmentsAsync(CancellationToken cancellationToken)
    {
        List<Appointment> appointments = await _context.Appointments
            .Include(a => a.Users)
            .ToListAsync(cancellationToken);
        
        return _mapper.Map<List<AppointmentDetails>>(appointments);
    }

    public async Task<AppointmentDetails> GetAppointmentByIdAsync(int id, CancellationToken cancellationToken)
    {
        Appointment appointment = await _context.Appointments
            .Include(a => a.Users)
            .SingleAsync(a => a.Id == id, cancellationToken);
        
        return _mapper.Map<AppointmentDetails>(appointment);
    }

    public async Task<AppointmentDetails> AddUserToAppointmentAsync(int appointmentId, int userId, CancellationToken cancellationToken)
    {
        Console.WriteLine($"appointmentId: {appointmentId}, userId: {userId}");
        Appointment appointment = await _context.Appointments
            .Include(a => a.Users)
            .SingleAsync(a => a.Id == appointmentId, cancellationToken);

        Console.WriteLine("Appointment");
        User user = await _context.Users.SingleAsync(u => u.Id == userId, cancellationToken);
        Console.WriteLine("User");
        appointment.Users.Add(user);
        
        await _context.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<AppointmentDetails>(appointment);
    }

    public async Task<AppointmentDetails> RemoveUserFromAppointmentAsync(int appointmentId, int userId, CancellationToken cancellationToken)
    {
        Appointment appointment = await _context.Appointments
            .Include(a => a.Users)
            .SingleAsync(a => a.Id == appointmentId, cancellationToken);

        User user = await _context.Users.SingleAsync(u => u.Id == userId, cancellationToken);
        appointment.Users.Remove(user);
        
        await _context.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<AppointmentDetails>(appointment);
    }
    
    public async Task<ICollection<int>> GetAppointmentsIdsByUserIdAsync(int userId, CancellationToken cancellationToken)
    {
        return _context.Users
            .Include(u => u.Appointments)
            .Where(u => u.Id == userId)
            .Select(u => u.Id)
            .ToList();
    }

    public async Task DeleteAppointmentByIdAsync(int id, CancellationToken cancellationToken)
    {
        await _context.Users.Where(o => o.Id == id).ExecuteDeleteAsync(cancellationToken);
    }
}
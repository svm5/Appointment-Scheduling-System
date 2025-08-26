using Contracts.Appointment.AppointmentHelpers;

namespace Contracts.Appointment;

public interface IAppointmentService
{
    Task<AppointmentDetails> CreateAppointmentAsync(CreateAppointmentRequest request, CancellationToken cancellationToken);
    Task<ICollection<AppointmentDetails>> GetAppointmentsAsync(CancellationToken cancellationToken);
    Task<AppointmentDetails> GetAppointmentByIdAsync(int id, CancellationToken cancellationToken);
    Task<AppointmentDetails> AddUserToAppointmentAsync(int appointmentId, int userId, CancellationToken cancellationToken);
    Task<AppointmentDetails> RemoveUserFromAppointmentAsync(int appointmentId, int userId, CancellationToken cancellationToken);
    Task<ICollection<int>> GetAppointmentsIdsByUserIdAsync(int userId, CancellationToken cancellationToken);
    Task DeleteAppointmentByIdAsync(int id, CancellationToken cancellationToken);
}
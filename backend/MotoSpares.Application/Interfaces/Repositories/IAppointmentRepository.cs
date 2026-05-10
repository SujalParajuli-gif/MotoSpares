using MotoSpares.Domain.Entities;

namespace MotoSpares.Application.Interfaces.Repositories;

public interface IAppointmentRepository : IRepositoryBase<Appointment>
{
    Task<List<Appointment>> GetAllWithUsersAsync();
    Task<List<Appointment>> GetByUserIdAsync(Guid userId);
    Task<Appointment?> GetByIdWithUsersAsync(int appointmentId);
    Task AddUserAppointmentAsync(UserAppointment userAppointment);
}

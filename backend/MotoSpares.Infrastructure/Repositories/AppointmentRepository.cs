using Microsoft.EntityFrameworkCore;
using MotoSpares.Application.Interfaces.Repositories;
using MotoSpares.Domain.Entities;
using MotoSpares.Infrastructure.Data;

namespace MotoSpares.Infrastructure.Repositories;

public class AppointmentRepository : RepositoryBase<Appointment>, IAppointmentRepository
{
    public AppointmentRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<List<Appointment>> GetAllWithUsersAsync()
    {
        return await _context.Appointments
            .Include(a => a.UserAppointments)
                .ThenInclude(ua => ua.User)
            .OrderByDescending(a => a.AppointmentDate)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<Appointment>> GetByUserIdAsync(Guid userId)
    {
        return await _context.Appointments
            .Where(a => a.UserAppointments.Any(ua => ua.UserId == userId))
            .Include(a => a.UserAppointments)
                .ThenInclude(ua => ua.User)
            .OrderByDescending(a => a.AppointmentDate)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Appointment?> GetByIdWithUsersAsync(int appointmentId)
    {
        return await _context.Appointments
            .Where(a => a.AppointmentId == appointmentId)
            .Include(a => a.UserAppointments)
                .ThenInclude(ua => ua.User)
            .FirstOrDefaultAsync();
    }

    public async Task AddUserAppointmentAsync(UserAppointment userAppointment)
    {
        await _context.UserAppointments.AddAsync(userAppointment);
        await _context.SaveChangesAsync();
    }
}

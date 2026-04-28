namespace MotoSpares.Domain.Entities;

public class UserAppointment
{
    public Guid UserId { get; set; }
    public int AppointmentId { get; set; }

    public ApplicationUser? User { get; set; }
    public Appointment? Appointment { get; set; }
}

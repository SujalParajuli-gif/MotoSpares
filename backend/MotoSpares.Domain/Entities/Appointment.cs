using System.ComponentModel.DataAnnotations;
using MotoSpares.Domain.Enums;

namespace MotoSpares.Domain.Entities;

public class Appointment
{
    public int AppointmentId { get; set; }

    public DateTime AppointmentDate { get; set; }

    public AppointmentStatus AppointmentStatus { get; set; }

    [Required]
    [StringLength(100)]
    public string ServiceType { get; set; } = string.Empty;

    [StringLength(500)]
    public string? AppointmentNotes { get; set; }

    public ICollection<UserAppointment> UserAppointments { get; set; } = new List<UserAppointment>();
}

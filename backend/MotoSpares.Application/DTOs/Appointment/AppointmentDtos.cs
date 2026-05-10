using System.ComponentModel.DataAnnotations;

namespace MotoSpares.Application.DTOs.Appointment;

public class CreateAppointmentDto
{
    [Required]
    public DateTime AppointmentDate { get; set; }

    [Required]
    [StringLength(100)]
    public string ServiceType { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Notes { get; set; }
}

public class UpdateAppointmentDto
{
    public DateTime? AppointmentDate { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }
}

public class UpdateAppointmentStatusDto
{
    [Required]
    public string Status { get; set; } = string.Empty;
}

public class AppointmentResponseDto
{
    public int AppointmentId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string ServiceType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public Guid CustomerId { get; set; }
}

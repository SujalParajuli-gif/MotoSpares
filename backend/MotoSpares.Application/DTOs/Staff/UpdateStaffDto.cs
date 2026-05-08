using System.ComponentModel.DataAnnotations;

namespace MotoSpares.Application.DTOs.Staff;

public class UpdateStaffDto
{
    [Required]
    [StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    [StringLength(20)]
    public string? Phone { get; set; }

    [StringLength(200)]
    public string? Address { get; set; }
}

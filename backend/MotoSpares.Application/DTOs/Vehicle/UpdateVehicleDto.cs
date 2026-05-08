using System.ComponentModel.DataAnnotations;

namespace MotoSpares.Application.DTOs.Vehicle;

public class UpdateVehicleDto
{
    [Required]
    [StringLength(50)]
    public string VehicleNumber { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Make { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Model { get; set; } = string.Empty;

    [Range(1900, 2100)]
    public int Year { get; set; }
}

using System.ComponentModel.DataAnnotations;

namespace MotoSpares.Application.DTOs.Customer;

public class RegisterCustomerWithVehicleDto
{
    // Customer Details
    [Required]
    [StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;

    [StringLength(20)]
    public string? Phone { get; set; }

    [StringLength(200)]
    public string? Address { get; set; }

    // Vehicle Details
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

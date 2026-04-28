using System.ComponentModel.DataAnnotations;

namespace MotoSpares.Application.DTOs.Vendors;

public class CreateVendorDto
{
    [Required]
    [StringLength(100)]
    public string VendorName { get; set; } = string.Empty;

    [EmailAddress]
    [StringLength(100)]
    public string? VendorEmail { get; set; }

    [StringLength(20)]
    public string? VendorPhone { get; set; }

    [StringLength(200)]
    public string? VendorAddress { get; set; }
}

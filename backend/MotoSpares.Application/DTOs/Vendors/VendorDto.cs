namespace MotoSpares.Application.DTOs.Vendors;

public class VendorDto
{
    public int VendorId { get; set; }
    public string VendorName { get; set; } = string.Empty;
    public string? VendorEmail { get; set; }
    public string? VendorPhone { get; set; }
    public string? VendorAddress { get; set; }
}

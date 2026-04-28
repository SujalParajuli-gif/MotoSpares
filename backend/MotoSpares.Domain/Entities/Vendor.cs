using System.ComponentModel.DataAnnotations;

namespace MotoSpares.Domain.Entities;

public class Vendor
{
    public int VendorId { get; set; }

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

    public ICollection<PurchaseInvoice> PurchaseInvoices { get; set; } = new List<PurchaseInvoice>();
    public ICollection<UserVendor> UserVendors { get; set; } = new List<UserVendor>();
}

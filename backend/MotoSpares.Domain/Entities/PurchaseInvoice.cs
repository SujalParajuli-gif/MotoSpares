using System.ComponentModel.DataAnnotations;

namespace MotoSpares.Domain.Entities;

public class PurchaseInvoice
{
    public int PurchaseInvoiceId { get; set; }

    public DateTime PurchaseDate { get; set; }

    [Range(0, double.MaxValue)]
    public decimal PurchaseTotal { get; set; }

    public int VendorId { get; set; }

    public Guid CreatedBy { get; set; }

    public Vendor? Vendor { get; set; }
    public ApplicationUser? CreatedByUser { get; set; }
    public ICollection<PurchaseInvoiceItem> PurchaseInvoiceItems { get; set; } = new List<PurchaseInvoiceItem>();
    public ICollection<UserPurchaseInvoice> UserPurchaseInvoices { get; set; } = new List<UserPurchaseInvoice>();
}

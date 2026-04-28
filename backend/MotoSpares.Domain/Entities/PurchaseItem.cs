using System.ComponentModel.DataAnnotations;

namespace MotoSpares.Domain.Entities;

public class PurchaseItem
{
    public int PurchaseItemId { get; set; }

    [Range(1, int.MaxValue)]
    public int PurchaseQuantity { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal PurchaseUnitCost { get; set; }

    public int PartId { get; set; }

    public Part? Part { get; set; }
    public ICollection<PurchaseInvoiceItem> PurchaseInvoiceItems { get; set; } = new List<PurchaseInvoiceItem>();
}

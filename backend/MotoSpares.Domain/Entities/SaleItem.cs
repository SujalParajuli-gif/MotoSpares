using System.ComponentModel.DataAnnotations;

namespace MotoSpares.Domain.Entities;

public class SaleItem
{
    public int SaleItemId { get; set; }

    [Range(1, int.MaxValue)]
    public int SaleQuantity { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal SaleUnitPrice { get; set; }

    public int PartId { get; set; }

    public Part? Part { get; set; }
    public ICollection<SaleInvoiceItem> SaleInvoiceItems { get; set; } = new List<SaleInvoiceItem>();
}

namespace MotoSpares.Domain.Entities;

public class SaleInvoiceItem
{
    public int SaleInvoiceId { get; set; }
    public int SaleItemId { get; set; }

    public SaleInvoice? SaleInvoice { get; set; }
    public SaleItem? SaleItem { get; set; }
}

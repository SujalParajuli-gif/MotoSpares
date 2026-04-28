namespace MotoSpares.Domain.Entities;

public class PurchaseInvoiceItem
{
    public int PurchaseInvoiceId { get; set; }
    public int PurchaseItemId { get; set; }

    public PurchaseInvoice? PurchaseInvoice { get; set; }
    public PurchaseItem? PurchaseItem { get; set; }
}

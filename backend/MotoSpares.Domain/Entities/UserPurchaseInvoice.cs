namespace MotoSpares.Domain.Entities;

public class UserPurchaseInvoice
{
    public Guid UserId { get; set; }
    public int PurchaseInvoiceId { get; set; }

    public ApplicationUser? User { get; set; }
    public PurchaseInvoice? PurchaseInvoice { get; set; }
}

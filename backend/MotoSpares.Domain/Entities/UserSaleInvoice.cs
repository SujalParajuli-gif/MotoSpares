namespace MotoSpares.Domain.Entities;

public class UserSaleInvoice
{
    public Guid UserId { get; set; }
    public int SaleInvoiceId { get; set; }

    public ApplicationUser? User { get; set; }
    public SaleInvoice? SaleInvoice { get; set; }
}

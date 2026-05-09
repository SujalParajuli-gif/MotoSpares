namespace MotoSpares.Application.Interfaces;

public interface IInvoiceService
{
    Task SendInvoiceEmailAsync(int saleInvoiceId);
}

using MotoSpares.Domain.Entities;

namespace MotoSpares.Application.Interfaces.Repositories;

public interface IInvoiceRepository
{
    Task<SaleInvoice?> GetSaleInvoiceByIdAsync(int saleInvoiceId);
}

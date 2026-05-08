using MotoSpares.Domain.Entities;

namespace MotoSpares.Application.Interfaces.Repositories;

public interface IPurchaseInvoiceRepository
{
    Task<PurchaseInvoice?> GetByIdAsync(int id);
    Task<IEnumerable<PurchaseInvoice>> GetAllAsync(int page, int pageSize);
    Task AddAsync(PurchaseInvoice invoice);
}

using MotoSpares.Domain.Entities;

namespace MotoSpares.Application.Interfaces.Repositories;

/// <summary>
/// PurchaseInvoice-specific repository extending the generic base.
/// Adds custom queries for paginated listing with includes.
/// </summary>
public interface IPurchaseInvoiceRepository : IRepositoryBase<PurchaseInvoice>
{
    new Task<PurchaseInvoice?> GetByIdAsync(int id);
    Task<IEnumerable<PurchaseInvoice>> GetAllAsync(int page, int pageSize);
    Task AddAsync(PurchaseInvoice invoice);
}

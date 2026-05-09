using MotoSpares.Domain.Entities;

namespace MotoSpares.Application.Interfaces.Repositories;

public interface IFinanceRepository
{
    Task<List<SaleInvoice>> GetSalesInDateRangeAsync(DateTime start, DateTime end);
    Task<List<PurchaseInvoice>> GetPurchasesInDateRangeAsync(DateTime start, DateTime end);
}

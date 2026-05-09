using Microsoft.EntityFrameworkCore;
using MotoSpares.Application.Interfaces.Repositories;
using MotoSpares.Domain.Entities;
using MotoSpares.Infrastructure.Data;

namespace MotoSpares.Infrastructure.Repositories;

public class FinanceRepository : IFinanceRepository
{
    private readonly AppDbContext _context;

    public FinanceRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<SaleInvoice>> GetSalesInDateRangeAsync(DateTime start, DateTime end)
    {
        return await _context.SaleInvoices
            .Where(s => s.SaleDate >= start && s.SaleDate <= end)
            .ToListAsync();
    }

    public async Task<List<PurchaseInvoice>> GetPurchasesInDateRangeAsync(DateTime start, DateTime end)
    {
        return await _context.PurchaseInvoices
            .Where(p => p.PurchaseDate >= start && p.PurchaseDate <= end)
            .ToListAsync();
    }
}

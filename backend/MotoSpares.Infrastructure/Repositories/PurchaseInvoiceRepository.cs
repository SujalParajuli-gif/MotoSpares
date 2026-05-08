using Microsoft.EntityFrameworkCore;
using MotoSpares.Application.Interfaces.Repositories;
using MotoSpares.Domain.Entities;
using MotoSpares.Infrastructure.Data;

namespace MotoSpares.Infrastructure.Repositories;

public class PurchaseInvoiceRepository : IPurchaseInvoiceRepository
{
    private readonly AppDbContext _context;

    public PurchaseInvoiceRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PurchaseInvoice?> GetByIdAsync(int id)
    {
        return await _context.PurchaseInvoices
            .Include(p => p.Vendor)
            .Include(p => p.PurchaseInvoiceItems)
                .ThenInclude(pi => pi.PurchaseItem)
                    .ThenInclude(item => item.Part)
            .FirstOrDefaultAsync(p => p.PurchaseInvoiceId == id);
    }

    public async Task<IEnumerable<PurchaseInvoice>> GetAllAsync(int page, int pageSize)
    {
        return await _context.PurchaseInvoices
            .Include(p => p.Vendor)
            .Include(p => p.PurchaseInvoiceItems)
                .ThenInclude(pi => pi.PurchaseItem)
                    .ThenInclude(item => item.Part)
            .OrderByDescending(p => p.PurchaseDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task AddAsync(PurchaseInvoice invoice)
    {
        await _context.PurchaseInvoices.AddAsync(invoice);
        await _context.SaveChangesAsync();
    }
}

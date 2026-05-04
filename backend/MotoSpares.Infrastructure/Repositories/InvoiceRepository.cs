using Microsoft.EntityFrameworkCore;
using MotoSpares.Application.Interfaces.Repositories;
using MotoSpares.Domain.Entities;
using MotoSpares.Infrastructure.Data;

namespace MotoSpares.Infrastructure.Repositories;

public class InvoiceRepository : IInvoiceRepository
{
    private readonly AppDbContext _context;

    public InvoiceRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<SaleInvoice?> GetSaleInvoiceByIdAsync(int saleInvoiceId)
    {
        return await _context.SaleInvoices
            .AsNoTracking()
            .Include(si => si.UserSaleInvoices)
                .ThenInclude(usi => usi.User)
            .Include(si => si.SaleInvoiceItems)
                .ThenInclude(sii => sii.SaleItem!)
                    .ThenInclude(item => item.Part)
            .FirstOrDefaultAsync(si => si.SaleInvoiceId == saleInvoiceId);
    }
}

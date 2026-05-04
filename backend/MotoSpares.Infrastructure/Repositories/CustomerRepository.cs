using Microsoft.EntityFrameworkCore;
using MotoSpares.Application.Interfaces.Repositories;
using MotoSpares.Domain.Entities;
using MotoSpares.Infrastructure.Data;

namespace MotoSpares.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _context;

    public CustomerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ApplicationUser>> GetAllCustomersAsync()
    {
        return await _context.Users
            .AsNoTracking()
            .Where(u => u.Role == "Customer")
            .Include(u => u.UserVehicles)
            .Include(u => u.UserSaleInvoices)
            .OrderBy(u => u.FullName)
            .ToListAsync();
    }

    public async Task<ApplicationUser?> GetCustomerByIdAsync(Guid userId)
    {
        return await _context.Users
            .AsNoTracking()
            .Where(u => u.Id == userId && u.Role == "Customer")
            .Include(u => u.UserVehicles)
                .ThenInclude(uv => uv.Vehicle)
            .Include(u => u.UserSaleInvoices)
                .ThenInclude(usi => usi.SaleInvoice!)
                    .ThenInclude(si => si.SaleInvoiceItems)
                        .ThenInclude(sii => sii.SaleItem!)
                            .ThenInclude(item => item.Part)
            .FirstOrDefaultAsync();
    }
}
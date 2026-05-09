using Microsoft.EntityFrameworkCore;
using MotoSpares.Application.Interfaces.Repositories;
using MotoSpares.Domain.Entities;
using MotoSpares.Infrastructure.Data;

namespace MotoSpares.Infrastructure.Repositories;

public class CustomerRepository : RepositoryBase<ApplicationUser>, ICustomerRepository
{
    public CustomerRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<List<ApplicationUser>> GetAllCustomersAsync()
    {
        return await _context.Users
            .Where(u => u.Role == "Customer")
            .Include(u => u.UserVehicles)
            .Include(u => u.UserSaleInvoices)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<ApplicationUser?> GetCustomerByIdAsync(Guid userId)
    {
        return await _context.Users
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

    public async Task<IEnumerable<ApplicationUser>> SearchCustomersAsync(string query)
    {
        var lowerQuery = query.ToLower();

        // Need to check Name, Phone, ID, or Vehicle Number
        return await _context.Users
            .Include(u => u.UserVehicles)
                .ThenInclude(uv => uv.Vehicle)
            .Where(u => u.Role == "Customer" && (
                u.FullName.ToLower().Contains(lowerQuery) ||
                (u.PhoneNumber != null && u.PhoneNumber.Contains(lowerQuery)) ||
                u.Id.ToString().ToLower() == lowerQuery ||
                u.UserVehicles.Any(uv => uv.Vehicle != null && uv.Vehicle.VehicleNumber.ToLower().Contains(lowerQuery))
            ))
            .ToListAsync();
    }
}

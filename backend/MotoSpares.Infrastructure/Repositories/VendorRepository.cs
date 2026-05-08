using Microsoft.EntityFrameworkCore;
using MotoSpares.Application.Interfaces.Repositories;
using MotoSpares.Domain.Entities;
using MotoSpares.Infrastructure.Data;

namespace MotoSpares.Infrastructure.Repositories;

public class VendorRepository : RepositoryBase<Vendor>, IVendorRepository
{
    public VendorRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Vendor>> GetAllAsync()
    {
        return await _context.Vendors.OrderBy(vendor => vendor.VendorName).ToListAsync();
    }

    public async Task AddAsync(Vendor vendor)
    {
        Create(vendor);
        await SaveChangesAsync();
    }

    public async Task UpdateAsync(Vendor vendor)
    {
        Update(vendor);
        await SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var vendor = await _context.Vendors.FindAsync(id);
        if (vendor != null)
        {
            Delete(vendor);
            await SaveChangesAsync();
        }
    }
}

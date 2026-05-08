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

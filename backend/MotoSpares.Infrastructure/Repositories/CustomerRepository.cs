using System.Threading.Tasks;
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

    public async Task AddVehicleForUserAsync(UserVehicle userVehicle)
    {
        await _context.UserVehicles.AddAsync(userVehicle);
        await _context.SaveChangesAsync();
    }
}

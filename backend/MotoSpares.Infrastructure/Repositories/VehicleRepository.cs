using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MotoSpares.Application.Interfaces.Repositories;
using MotoSpares.Domain.Entities;
using MotoSpares.Infrastructure.Data;

namespace MotoSpares.Infrastructure.Repositories;

public class VehicleRepository : IVehicleRepository
{
    private readonly AppDbContext _context;

    public VehicleRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<(int VehicleId, string VehicleNumber, string Make, string Model, int Year)>> GetVehiclesByUserIdAsync(Guid userId)
    {
        return await _context.UserVehicles
            .Where(uv => uv.UserId == userId)
            .Include(uv => uv.Vehicle)
            .Select(uv => new ValueTuple<int, string, string, string, int>(
                uv.Vehicle!.VehicleId,
                uv.Vehicle.VehicleNumber,
                uv.Vehicle.Make,
                uv.Vehicle.Model,
                uv.Vehicle.Year
            ))
            .ToListAsync();
    }

    public async Task<bool> VehicleNumberExistsAsync(string vehicleNumber, int? excludeId = null)
    {
        var query = _context.Vehicles.Where(v => v.VehicleNumber == vehicleNumber);
        if (excludeId.HasValue)
            query = query.Where(v => v.VehicleId != excludeId.Value);
        return await query.AnyAsync();
    }

    public async Task<Vehicle> AddVehicleAsync(Vehicle vehicle)
    {
        await _context.Vehicles.AddAsync(vehicle);
        await _context.SaveChangesAsync();
        return vehicle;
    }

    public async Task AddUserVehicleLinkAsync(UserVehicle userVehicle)
    {
        await _context.UserVehicles.AddAsync(userVehicle);
        await _context.SaveChangesAsync();
    }

    public async Task<(UserVehicle? link, Vehicle? vehicle)> GetUserVehicleAsync(Guid userId, int vehicleId)
    {
        var link = await _context.UserVehicles
            .Include(uv => uv.Vehicle)
            .FirstOrDefaultAsync(uv => uv.UserId == userId && uv.VehicleId == vehicleId);
        return (link, link?.Vehicle);
    }

    public async Task UpdateAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task DeleteVehicleAsync(UserVehicle link, Vehicle vehicle)
    {
        _context.UserVehicles.Remove(link);
        _context.Vehicles.Remove(vehicle);
        await _context.SaveChangesAsync();
    }
}

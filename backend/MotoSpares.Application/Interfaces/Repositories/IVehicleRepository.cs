using System.Collections.Generic;
using System.Threading.Tasks;
using MotoSpares.Domain.Entities;

namespace MotoSpares.Application.Interfaces.Repositories;

public interface IVehicleRepository
{
    Task<IEnumerable<(int VehicleId, string VehicleNumber, string Make, string Model, int Year)>> GetVehiclesByUserIdAsync(Guid userId);
    Task<bool> VehicleNumberExistsAsync(string vehicleNumber, int? excludeId = null);
    Task<Vehicle> AddVehicleAsync(Vehicle vehicle);
    Task AddUserVehicleLinkAsync(UserVehicle userVehicle);
    Task<(UserVehicle? link, Vehicle? vehicle)> GetUserVehicleAsync(Guid userId, int vehicleId);
    Task UpdateAsync();
    Task DeleteVehicleAsync(UserVehicle link, Vehicle vehicle);
}

using System.Collections.Generic;
using System.Threading.Tasks;
using MotoSpares.Application.DTOs;
using MotoSpares.Application.DTOs.Vehicle;

namespace MotoSpares.Application.Interfaces;

public interface IVehicleService
{
    Task<ApiResponse<IEnumerable<VehicleDto>>> GetMyVehiclesAsync(string userId);
    Task<ApiResponse<VehicleDto>> AddVehicleAsync(string userId, CreateVehicleDto dto);
    Task<ApiResponse<VehicleDto>> UpdateVehicleAsync(string userId, int vehicleId, UpdateVehicleDto dto);
    Task<ApiResponse<bool>> DeleteVehicleAsync(string userId, int vehicleId);
}

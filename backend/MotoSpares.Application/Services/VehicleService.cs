using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MotoSpares.Application.DTOs;
using MotoSpares.Application.DTOs.Vehicle;
using MotoSpares.Application.Interfaces;
using MotoSpares.Application.Interfaces.Repositories;
using MotoSpares.Domain.Entities;

namespace MotoSpares.Application.Services;

public class VehicleService : IVehicleService
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly ILogger<VehicleService> _logger;

    public VehicleService(IVehicleRepository vehicleRepository, ILogger<VehicleService> logger)
    {
        _vehicleRepository = vehicleRepository;
        _logger = logger;
    }

    public async Task<ApiResponse<IEnumerable<VehicleDto>>> GetMyVehiclesAsync(string userId)
    {
        var userGuid = Guid.Parse(userId);
        var vehicles = await _vehicleRepository.GetVehiclesByUserIdAsync(userGuid);

        var dtos = vehicles.Select(v => new VehicleDto
        {
            VehicleId = v.VehicleId,
            VehicleNumber = v.VehicleNumber,
            Make = v.Make,
            Model = v.Model,
            Year = v.Year
        });

        return ApiResponse<IEnumerable<VehicleDto>>.Success(dtos);
    }

    public async Task<ApiResponse<VehicleDto>> AddVehicleAsync(string userId, CreateVehicleDto dto)
    {
        var userGuid = Guid.Parse(userId);

        if (await _vehicleRepository.VehicleNumberExistsAsync(dto.VehicleNumber.Trim()))
            return ApiResponse<VehicleDto>.Fail("A vehicle with this number plate already exists.");

        var vehicle = new Vehicle
        {
            VehicleNumber = dto.VehicleNumber.Trim(),
            Make = dto.Make.Trim(),
            Model = dto.Model.Trim(),
            Year = dto.Year
        };

        await _vehicleRepository.AddVehicleAsync(vehicle);

        await _vehicleRepository.AddUserVehicleLinkAsync(new UserVehicle
        {
            UserId = userGuid,
            VehicleId = vehicle.VehicleId
        });

        _logger.LogInformation("Vehicle {VehicleNumber} added for user {UserId}", vehicle.VehicleNumber, userId);

        return ApiResponse<VehicleDto>.Success(new VehicleDto
        {
            VehicleId = vehicle.VehicleId,
            VehicleNumber = vehicle.VehicleNumber,
            Make = vehicle.Make,
            Model = vehicle.Model,
            Year = vehicle.Year
        }, "Vehicle added successfully.");
    }

    public async Task<ApiResponse<VehicleDto>> UpdateVehicleAsync(string userId, int vehicleId, UpdateVehicleDto dto)
    {
        var userGuid = Guid.Parse(userId);
        var (link, vehicle) = await _vehicleRepository.GetUserVehicleAsync(userGuid, vehicleId);

        if (link == null || vehicle == null)
            return ApiResponse<VehicleDto>.Fail("Vehicle not found.");

        if (await _vehicleRepository.VehicleNumberExistsAsync(dto.VehicleNumber.Trim(), vehicleId))
            return ApiResponse<VehicleDto>.Fail("Another vehicle with this number plate already exists.");

        vehicle.VehicleNumber = dto.VehicleNumber.Trim();
        vehicle.Make = dto.Make.Trim();
        vehicle.Model = dto.Model.Trim();
        vehicle.Year = dto.Year;

        await _vehicleRepository.UpdateAsync();

        return ApiResponse<VehicleDto>.Success(new VehicleDto
        {
            VehicleId = vehicle.VehicleId,
            VehicleNumber = vehicle.VehicleNumber,
            Make = vehicle.Make,
            Model = vehicle.Model,
            Year = vehicle.Year
        }, "Vehicle updated successfully.");
    }

    public async Task<ApiResponse<bool>> DeleteVehicleAsync(string userId, int vehicleId)
    {
        var userGuid = Guid.Parse(userId);
        var (link, vehicle) = await _vehicleRepository.GetUserVehicleAsync(userGuid, vehicleId);

        if (link == null || vehicle == null)
            return ApiResponse<bool>.Fail("Vehicle not found.");

        await _vehicleRepository.DeleteVehicleAsync(link, vehicle);

        _logger.LogInformation("Vehicle {VehicleId} deleted for user {UserId}", vehicleId, userId);
        return ApiResponse<bool>.Success(true, "Vehicle deleted successfully.");
    }
}

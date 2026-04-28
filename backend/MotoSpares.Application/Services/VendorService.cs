using Microsoft.Extensions.Logging;
using MotoSpares.Application.DTOs;
using MotoSpares.Application.DTOs.Vendors;
using MotoSpares.Application.Interfaces;
using MotoSpares.Application.Interfaces.Repositories;
using MotoSpares.Domain.Entities;

namespace MotoSpares.Application.Services;

public class VendorService : IVendorService
{
    private readonly IVendorRepository _vendorRepository;
    private readonly ILogger<VendorService> _logger;

    public VendorService(IVendorRepository vendorRepository, ILogger<VendorService> logger)
    {
        _vendorRepository = vendorRepository;
        _logger = logger;
    }

    public async Task<ApiResponse<VendorDto>> CreateAsync(CreateVendorDto dto)
    {
        var vendor = new Vendor
        {
            VendorName = dto.VendorName.Trim(),
            VendorEmail = dto.VendorEmail?.Trim().ToLower(),
            VendorPhone = dto.VendorPhone?.Trim(),
            VendorAddress = dto.VendorAddress?.Trim()
        };

        await _vendorRepository.AddAsync(vendor);
        _logger.LogInformation("Vendor created: {VendorName}", vendor.VendorName);

        return ApiResponse<VendorDto>.Success(MapToDto(vendor), "Vendor created successfully.");
    }

    public async Task<ApiResponse<List<VendorDto>>> GetAllAsync()
    {
        var vendors = await _vendorRepository.GetAllAsync();
        return ApiResponse<List<VendorDto>>.Success(vendors.Select(MapToDto).ToList());
    }

    public async Task<ApiResponse<VendorDto>> GetByIdAsync(int id)
    {
        var vendor = await _vendorRepository.GetByIdAsync(id);
        if (vendor == null)
        {
            return ApiResponse<VendorDto>.Fail("Vendor not found.");
        }

        return ApiResponse<VendorDto>.Success(MapToDto(vendor));
    }

    public async Task<ApiResponse<VendorDto>> UpdateAsync(int id, UpdateVendorDto dto)
    {
        var vendor = await _vendorRepository.GetByIdAsync(id);
        if (vendor == null)
        {
            return ApiResponse<VendorDto>.Fail("Vendor not found.");
        }

        vendor.VendorName = dto.VendorName.Trim();
        vendor.VendorEmail = dto.VendorEmail?.Trim().ToLower();
        vendor.VendorPhone = dto.VendorPhone?.Trim();
        vendor.VendorAddress = dto.VendorAddress?.Trim();

        await _vendorRepository.UpdateAsync(vendor);
        _logger.LogInformation("Vendor updated: {VendorId}", vendor.VendorId);

        return ApiResponse<VendorDto>.Success(MapToDto(vendor), "Vendor updated successfully.");
    }

    public async Task<ApiResponse<bool>> DeleteAsync(int id)
    {
        var vendor = await _vendorRepository.GetByIdAsync(id);
        if (vendor == null)
        {
            return ApiResponse<bool>.Fail("Vendor not found.");
        }

        await _vendorRepository.DeleteAsync(id);
        _logger.LogInformation("Vendor deleted: {VendorId}", id);

        return ApiResponse<bool>.Success(true, "Vendor deleted successfully.");
    }

    private static VendorDto MapToDto(Vendor vendor)
    {
        return new VendorDto
        {
            VendorId = vendor.VendorId,
            VendorName = vendor.VendorName,
            VendorEmail = vendor.VendorEmail,
            VendorPhone = vendor.VendorPhone,
            VendorAddress = vendor.VendorAddress
        };
    }
}

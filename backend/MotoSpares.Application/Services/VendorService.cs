using MotoSpares.Application.DTOs.Vendors;
using MotoSpares.Application.Interfaces.Repositories;
using MotoSpares.Domain.Entities;
using System.Numerics;

namespace MotoSpares.Application.Services;

public class VendorService
{
    private readonly IVendorRepository _vendorRepository;

    public VendorService(IVendorRepository vendorRepository)
    {
        _vendorRepository = vendorRepository;
    }

    public async Task<VendorDto> CreateVendorAsync(CreateVendorDto dto)
    {
        var vendor = new Vendor(
            dto.VendorName,
            dto.ContactPerson,
            dto.Phone,
            dto.Email,
            dto.Address
        );

        await _vendorRepository.AddAsync(vendor);

        return MapToDto(vendor);
    }

    public async Task<IEnumerable<VendorDto>> GetAllVendorsAsync()
    {
        var vendors = await _vendorRepository.GetAllAsync();
        return vendors.Select(MapToDto);
    }

    public async Task<VendorDto?> GetVendorByIdAsync(Guid id)
    {
        var vendor = await _vendorRepository.GetByIdAsync(id);
        if (vendor == null) return null;

        return MapToDto(vendor);
    }

    public async Task<VendorDto?> UpdateVendorAsync(Guid id, UpdateVendorDto dto)
    {
        var vendor = await _vendorRepository.GetByIdAsync(id);
        if (vendor == null) return null;

        vendor.UpdateDetails(
            dto.VendorName,
            dto.ContactPerson,
            dto.Phone,
            dto.Email,
            dto.Address
        );

        await _vendorRepository.UpdateAsync(vendor);

        return MapToDto(vendor);
    }

    public async Task<bool> DeleteVendorAsync(Guid id)
    {
        var vendor = await _vendorRepository.GetByIdAsync(id);
        if (vendor == null) return false;

        await _vendorRepository.DeleteAsync(id);
        return true;
    }

    private static VendorDto MapToDto(Vendor vendor)
    {
        return new VendorDto
        {
            Id = vendor.Id,
            VendorName = vendor.VendorName,
            ContactPerson = vendor.ContactPerson,
            Phone = vendor.Phone,
            Email = vendor.Email,
            Address = vendor.Address,
            CreatedAt = vendor.CreatedAt
        };
    }
}
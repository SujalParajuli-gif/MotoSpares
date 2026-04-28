using MotoSpares.Application.DTOs;
using MotoSpares.Application.DTOs.Vendors;

namespace MotoSpares.Application.Interfaces;

public interface IVendorService
{
    Task<ApiResponse<List<VendorDto>>> GetAllAsync();
    Task<ApiResponse<VendorDto>> GetByIdAsync(int id);
    Task<ApiResponse<VendorDto>> CreateAsync(CreateVendorDto dto);
    Task<ApiResponse<VendorDto>> UpdateAsync(int id, UpdateVendorDto dto);
    Task<ApiResponse<bool>> DeleteAsync(int id);
}

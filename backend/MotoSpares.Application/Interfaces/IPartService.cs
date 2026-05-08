using MotoSpares.Application.DTOs;
using MotoSpares.Application.DTOs.Part;

namespace MotoSpares.Application.Interfaces;

public interface IPartService
{
    Task<ApiResponse<IEnumerable<PartResponseDto>>> GetAllAsync(int page, int pageSize);
    Task<ApiResponse<PartResponseDto>> GetByIdAsync(int id);
    Task<ApiResponse<IEnumerable<PartResponseDto>>> GetLowStockAsync();
    Task<ApiResponse<PartResponseDto>> CreateAsync(PartCreateDto dto);
    Task<ApiResponse<PartResponseDto>> UpdateAsync(int id, PartUpdateDto dto);
    Task<ApiResponse<bool>> DeleteAsync(int id);
}

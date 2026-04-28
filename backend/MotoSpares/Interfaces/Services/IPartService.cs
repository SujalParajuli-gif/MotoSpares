using System.Collections.Generic;
using System.Threading.Tasks;
using MotoSpares.Common.Responses;
using MotoSpares.DTOs.Part;

namespace MotoSpares.Interfaces.Services
{
    public interface IPartService
    {
        Task<ApiResponse<IEnumerable<PartResponseDto>>> GetAllAsync(int page = 1, int pageSize = 10);
        Task<ApiResponse<PartResponseDto>> GetByIdAsync(int id);
        Task<ApiResponse<PartResponseDto>> CreateAsync(PartCreateDto dto);
        Task<ApiResponse<PartResponseDto>> UpdateAsync(PartUpdateDto dto);
        Task<ApiResponse<bool>> DeleteAsync(int id);
        Task<ApiResponse<IEnumerable<PartResponseDto>>> GetLowStockAsync();
    }
}

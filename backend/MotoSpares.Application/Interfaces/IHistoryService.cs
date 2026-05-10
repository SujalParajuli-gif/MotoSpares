using MotoSpares.Application.DTOs;
using MotoSpares.Application.DTOs.History;

namespace MotoSpares.Application.Interfaces;

public interface IHistoryService
{
    Task<ApiResponse<FullHistoryDto>> GetFullHistoryAsync(Guid userId);
    Task<ApiResponse<List<PurchaseHistoryItemDto>>> GetPurchaseHistoryAsync(Guid userId);
    Task<ApiResponse<List<ServiceHistoryItemDto>>> GetServiceHistoryAsync(Guid userId);
}

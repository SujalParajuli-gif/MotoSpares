using MotoSpares.Application.DTOs;
using MotoSpares.Application.DTOs.Loyalty;

namespace MotoSpares.Application.Interfaces;

public interface ILoyaltyService
{
    Task<ApiResponse<LoyaltyStatusDto>> GetLoyaltyStatusAsync(Guid customerId);
    Task<ApiResponse<DiscountResultDto>> CalculateDiscountAsync(CalculateDiscountDto dto);
}

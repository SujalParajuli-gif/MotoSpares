using MotoSpares.Application.DTOs;
using MotoSpares.Application.DTOs.Review;

namespace MotoSpares.Application.Interfaces;

public interface IReviewService
{
    Task<ApiResponse<ReviewResponseDto>> CreateReviewAsync(CreateReviewDto dto, Guid userId);
    Task<ApiResponse<List<ReviewResponseDto>>> GetMyReviewsAsync(Guid userId);
    Task<ApiResponse<List<ReviewResponseDto>>> GetAllReviewsAsync();
}

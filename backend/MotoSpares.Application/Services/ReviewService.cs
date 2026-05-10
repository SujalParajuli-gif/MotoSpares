using MotoSpares.Application.DTOs;
using MotoSpares.Application.DTOs.Review;
using MotoSpares.Application.Interfaces;
using MotoSpares.Application.Interfaces.Repositories;
using MotoSpares.Domain.Entities;

namespace MotoSpares.Application.Services;

public class ReviewService : IReviewService
{
    private readonly IReviewRepository _reviewRepository;

    public ReviewService(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }

    public async Task<ApiResponse<ReviewResponseDto>> CreateReviewAsync(CreateReviewDto dto, Guid userId)
    {
        var review = new Review
        {
            Rating = dto.Rating,
            ReviewText = dto.ReviewText?.Trim(),
            ReviewDate = DateTime.UtcNow
        };

        _reviewRepository.Create(review);
        await _reviewRepository.SaveChangesAsync();

        // Link the review to the customer via the join table
        var userReview = new UserReview
        {
            UserId = userId,
            ReviewId = review.ReviewId
        };
        await _reviewRepository.AddUserReviewAsync(userReview);

        // Reload with user info
        var allByUser = await _reviewRepository.GetByUserIdAsync(userId);
        var created = allByUser.FirstOrDefault(r => r.ReviewId == review.ReviewId);

        return ApiResponse<ReviewResponseDto>.Success(MapToDto(created ?? review), "Review submitted successfully.");
    }

    public async Task<ApiResponse<List<ReviewResponseDto>>> GetMyReviewsAsync(Guid userId)
    {
        var reviews = await _reviewRepository.GetByUserIdAsync(userId);
        var dtos = reviews.Select(MapToDto).ToList();
        return ApiResponse<List<ReviewResponseDto>>.Success(dtos);
    }

    public async Task<ApiResponse<List<ReviewResponseDto>>> GetAllReviewsAsync()
    {
        var reviews = await _reviewRepository.GetAllWithUsersAsync();
        var dtos = reviews.Select(MapToDto).ToList();
        return ApiResponse<List<ReviewResponseDto>>.Success(dtos);
    }

    private static ReviewResponseDto MapToDto(Review review)
    {
        var user = review.UserReviews?.FirstOrDefault()?.User;
        return new ReviewResponseDto
        {
            ReviewId = review.ReviewId,
            Rating = review.Rating,
            ReviewText = review.ReviewText,
            ReviewDate = review.ReviewDate,
            CustomerName = user?.FullName ?? string.Empty,
            CustomerId = user?.Id ?? Guid.Empty
        };
    }
}

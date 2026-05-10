using MotoSpares.Domain.Entities;

namespace MotoSpares.Application.Interfaces.Repositories;

public interface IReviewRepository : IRepositoryBase<Review>
{
    Task<List<Review>> GetAllWithUsersAsync();
    Task<List<Review>> GetByUserIdAsync(Guid userId);
    Task AddUserReviewAsync(UserReview userReview);
}

using Microsoft.EntityFrameworkCore;
using MotoSpares.Application.Interfaces.Repositories;
using MotoSpares.Domain.Entities;
using MotoSpares.Infrastructure.Data;

namespace MotoSpares.Infrastructure.Repositories;

public class ReviewRepository : RepositoryBase<Review>, IReviewRepository
{
    public ReviewRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<List<Review>> GetAllWithUsersAsync()
    {
        return await _context.Reviews
            .Include(r => r.UserReviews)
                .ThenInclude(ur => ur.User)
            .OrderByDescending(r => r.ReviewDate)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<Review>> GetByUserIdAsync(Guid userId)
    {
        return await _context.Reviews
            .Where(r => r.UserReviews.Any(ur => ur.UserId == userId))
            .Include(r => r.UserReviews)
                .ThenInclude(ur => ur.User)
            .OrderByDescending(r => r.ReviewDate)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task AddUserReviewAsync(UserReview userReview)
    {
        await _context.UserReviews.AddAsync(userReview);
        await _context.SaveChangesAsync();
    }
}

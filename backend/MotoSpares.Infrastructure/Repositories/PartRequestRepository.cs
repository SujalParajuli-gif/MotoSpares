using Microsoft.EntityFrameworkCore;
using MotoSpares.Application.Interfaces.Repositories;
using MotoSpares.Domain.Entities;
using MotoSpares.Infrastructure.Data;

namespace MotoSpares.Infrastructure.Repositories;

public class PartRequestRepository : RepositoryBase<PartRequest>, IPartRequestRepository
{
    public PartRequestRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<List<PartRequest>> GetAllWithUsersAsync()
    {
        return await _context.PartRequests
            .Include(pr => pr.UserPartRequests)
                .ThenInclude(upr => upr.User)
            .OrderByDescending(pr => pr.RequestDate)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<PartRequest>> GetByUserIdAsync(Guid userId)
    {
        return await _context.PartRequests
            .Where(pr => pr.UserPartRequests.Any(upr => upr.UserId == userId))
            .Include(pr => pr.UserPartRequests)
                .ThenInclude(upr => upr.User)
            .OrderByDescending(pr => pr.RequestDate)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<PartRequest?> GetByIdWithUsersAsync(int requestId)
    {
        return await _context.PartRequests
            .Where(pr => pr.RequestId == requestId)
            .Include(pr => pr.UserPartRequests)
                .ThenInclude(upr => upr.User)
            .FirstOrDefaultAsync();
    }

    public async Task AddUserPartRequestAsync(UserPartRequest userPartRequest)
    {
        await _context.UserPartRequests.AddAsync(userPartRequest);
        await _context.SaveChangesAsync();
    }
}

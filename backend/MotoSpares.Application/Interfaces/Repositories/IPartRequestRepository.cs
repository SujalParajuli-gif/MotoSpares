using MotoSpares.Domain.Entities;

namespace MotoSpares.Application.Interfaces.Repositories;

public interface IPartRequestRepository : IRepositoryBase<PartRequest>
{
    Task<List<PartRequest>> GetAllWithUsersAsync();
    Task<List<PartRequest>> GetByUserIdAsync(Guid userId);
    Task<PartRequest?> GetByIdWithUsersAsync(int requestId);
    Task AddUserPartRequestAsync(UserPartRequest userPartRequest);
}

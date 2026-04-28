using MotoSpares.Domain.Entities;

namespace MotoSpares.Application.Interfaces.Repositories;

public interface IStaffRepository
{
    Task<Staff?> GetByIdAsync(Guid id);
    Task<Staff?> GetByUserIdAsync(Guid userId);
    Task<IEnumerable<Staff>> GetAllAsync();
    Task AddAsync(Staff staff);
    Task UpdateAsync(Staff staff);
    Task DeleteAsync(Guid id);
}
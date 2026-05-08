using MotoSpares.Domain.Entities;

namespace MotoSpares.Application.Interfaces.Repositories;

public interface IPartRepository
{
    Task<Part?> GetByIdAsync(int id);
    Task<IEnumerable<Part>> GetAllAsync(int page, int pageSize);
    Task<int> GetTotalCountAsync();
    Task<IEnumerable<Part>> GetLowStockAsync();
    Task<bool> ExistsByPartNumberAsync(string partNumber, int? excludeId = null);
    Task AddAsync(Part part);
    Task UpdateAsync(Part part);
    Task DeleteAsync(int id);
}

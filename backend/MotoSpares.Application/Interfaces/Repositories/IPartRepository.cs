using MotoSpares.Domain.Entities;

namespace MotoSpares.Application.Interfaces.Repositories;

/// <summary>
/// Part-specific repository extending the generic base.
/// Adds custom queries unique to Parts (low stock, part number check, pagination).
/// </summary>
public interface IPartRepository : IRepositoryBase<Part>
{
    Task<IEnumerable<Part>> GetAllAsync(int page, int pageSize);
    Task<int> GetTotalCountAsync();
    Task<IEnumerable<Part>> GetLowStockAsync();
    Task<bool> ExistsByPartNumberAsync(string partNumber, int? excludeId = null);
    Task AddAsync(Part part);
    Task UpdateAsync(Part part);
    Task DeleteAsync(int id);
}

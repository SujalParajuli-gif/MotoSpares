using MotoSpares.Domain.Entities;

namespace MotoSpares.Application.Interfaces.Repositories;

/// <summary>
/// Vendor-specific repository extending the generic base.
/// </summary>
public interface IVendorRepository : IRepositoryBase<Vendor>
{
    Task<IEnumerable<Vendor>> GetAllAsync();
    Task AddAsync(Vendor vendor);
    Task UpdateAsync(Vendor vendor);
    Task DeleteAsync(int id);
}

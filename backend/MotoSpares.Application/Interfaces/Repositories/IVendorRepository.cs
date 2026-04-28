using MotoSpares.Domain.Entities;

namespace MotoSpares.Application.Interfaces.Repositories;

public interface IVendorRepository
{
    Task<Vendor?> GetByIdAsync(int id);
    Task<IEnumerable<Vendor>> GetAllAsync();
    Task AddAsync(Vendor vendor);
    Task UpdateAsync(Vendor vendor);
    Task DeleteAsync(int id);
}

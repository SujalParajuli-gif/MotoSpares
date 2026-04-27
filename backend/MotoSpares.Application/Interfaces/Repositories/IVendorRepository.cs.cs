using MotoSpares.Domain.Entities;
using System.Numerics;

namespace MotoSpares.Application.Interfaces.Repositories;

public interface IVendorRepository
{
    Task<Vendor?> GetByIdAsync(Guid id);
    Task<IEnumerable<Vendor>> GetAllAsync();
    Task AddAsync(Vendor vendor);
    Task UpdateAsync(Vendor vendor);
    Task DeleteAsync(Guid id);
}
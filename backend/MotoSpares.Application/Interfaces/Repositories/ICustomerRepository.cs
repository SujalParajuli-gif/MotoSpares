using MotoSpares.Domain.Entities;

namespace MotoSpares.Application.Interfaces.Repositories;

/// <summary>
/// Customer-specific repository extending the generic base.
/// Adds custom multi-criteria search query.
/// </summary>
public interface ICustomerRepository : IRepositoryBase<ApplicationUser>
{
    Task<IEnumerable<ApplicationUser>> SearchCustomersAsync(string query);
}

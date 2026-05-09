using MotoSpares.Domain.Entities;

namespace MotoSpares.Application.Interfaces.Repositories;

/// <summary>
/// Customer-specific repository extending the generic base.
/// Provides customer lookup, search, and vehicle management methods.
/// </summary>
public interface ICustomerRepository : IRepositoryBase<ApplicationUser>
{
    Task<List<ApplicationUser>> GetAllCustomersAsync();
    Task<ApplicationUser?> GetCustomerByIdAsync(Guid userId);
    Task<IEnumerable<ApplicationUser>> SearchCustomersAsync(string query);
    Task AddVehicleForUserAsync(UserVehicle userVehicle);
}

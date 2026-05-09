using MotoSpares.Domain.Entities;

namespace MotoSpares.Application.Interfaces.Repositories;

public interface ICustomerRepository
{
    Task<List<ApplicationUser>> GetAllCustomersAsync();
    Task<ApplicationUser?> GetCustomerByIdAsync(Guid userId);
}
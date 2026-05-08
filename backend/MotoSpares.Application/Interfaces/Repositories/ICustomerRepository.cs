using MotoSpares.Domain.Entities;

namespace MotoSpares.Application.Interfaces.Repositories;

public interface ICustomerRepository
{
    Task<IEnumerable<ApplicationUser>> SearchCustomersAsync(string query);
}

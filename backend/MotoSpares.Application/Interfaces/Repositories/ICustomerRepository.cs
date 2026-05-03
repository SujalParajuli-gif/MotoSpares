using MotoSpares.Application.DTOs.Customers;

namespace MotoSpares.Application.Interfaces.Repositories;

public interface ICustomerRepository
{
    Task<List<CustomerListDto>> GetAllCustomersAsync();
    Task<CustomerProfileDto?> GetCustomerProfileAsync(Guid userId);
}
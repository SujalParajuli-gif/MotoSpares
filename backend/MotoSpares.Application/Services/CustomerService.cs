using MotoSpares.Application.DTOs.Customers;
using MotoSpares.Application.Interfaces;
using MotoSpares.Application.Interfaces.Repositories;

namespace MotoSpares.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _repository;

    public CustomerService(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<CustomerListDto>> GetAllCustomersAsync()
    {
        return await _repository.GetAllCustomersAsync();
    }

    public async Task<CustomerProfileDto?> GetCustomerProfileAsync(Guid userId)
    {
        return await _repository.GetCustomerProfileAsync(userId);
    }
}
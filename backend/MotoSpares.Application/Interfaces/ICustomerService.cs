using MotoSpares.Application.DTOs.Customers;

namespace MotoSpares.Application.Interfaces;

public interface ICustomerService
{
    // Lightweight list — for staff dashboard
    Task<List<CustomerListDto>> GetAllCustomersAsync();

    // Full profile — details + vehicles + history
    Task<CustomerProfileDto?> GetCustomerProfileAsync(Guid userId);
}
using MotoSpares.Application.DTOs.Customer;


namespace MotoSpares.Application.Interfaces;

public interface ICustomerService
{
    Task<List<CustomerListDto>> GetAllCustomersAsync();
    
    // Feature 8 endpoints
    Task<CustomerDetailsDto?> GetCustomerDetailsAsync(Guid userId);
    Task<CustomerHistoryDto?> GetCustomerHistoryAsync(Guid userId);
    Task<CustomerVehiclesDto?> GetCustomerVehiclesAsync(Guid userId);
}
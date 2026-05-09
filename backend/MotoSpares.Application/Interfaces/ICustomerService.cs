using MotoSpares.Application.DTOs;
using MotoSpares.Application.DTOs.Auth;
using MotoSpares.Application.DTOs.Customer;

namespace MotoSpares.Application.Interfaces;

public interface ICustomerService
{
    Task<List<CustomerListDto>> GetAllCustomersAsync();
    Task<CustomerDetailsDto?> GetCustomerDetailsAsync(Guid userId);
    Task<CustomerHistoryDto?> GetCustomerHistoryAsync(Guid userId);
    Task<CustomerVehiclesDto?> GetCustomerVehiclesAsync(Guid userId);
    Task<ApiResponse<IEnumerable<CustomerSearchResponseDto>>> SearchCustomersAsync(string query);
    Task<ApiResponse<AuthResponseDto>> RegisterCustomerWithVehicleAsync(RegisterCustomerWithVehicleDto dto);
}

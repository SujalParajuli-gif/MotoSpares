using MotoSpares.Application.DTOs;
using MotoSpares.Application.DTOs.Customer;

namespace MotoSpares.Application.Interfaces;

public interface ICustomerService
{
    Task<ApiResponse<IEnumerable<CustomerSearchResponseDto>>> SearchCustomersAsync(string query);
}

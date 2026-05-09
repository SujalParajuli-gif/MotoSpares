using MotoSpares.Application.DTOs;
using MotoSpares.Application.DTOs.Customer;
using MotoSpares.Application.Interfaces;
using MotoSpares.Application.Interfaces.Repositories;

namespace MotoSpares.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<ApiResponse<IEnumerable<CustomerSearchResponseDto>>> SearchCustomersAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return ApiResponse<IEnumerable<CustomerSearchResponseDto>>.Fail("Search query cannot be empty.");

        var customers = await _customerRepository.SearchCustomersAsync(query.Trim());

        var dtos = customers.Select(u => new CustomerSearchResponseDto
        {
            UserId = u.Id,
            FullName = u.FullName,
            Email = u.Email ?? "",
            PhoneNumber = u.PhoneNumber,
            Address = u.Address,
            Vehicles = u.UserVehicles
                .Where(uv => uv.Vehicle != null)
                .Select(uv => new VehicleDto
                {
                    VehicleId = uv.Vehicle!.VehicleId,
                    VehicleNumber = uv.Vehicle.VehicleNumber,
                    Make = uv.Vehicle.Make,
                    Model = uv.Vehicle.Model,
                    Year = uv.Vehicle.Year
                }).ToList()
        }).ToList();

        return ApiResponse<IEnumerable<CustomerSearchResponseDto>>.Success(dtos);
    }
}

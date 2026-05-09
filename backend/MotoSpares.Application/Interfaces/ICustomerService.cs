using System.Threading.Tasks;
using MotoSpares.Application.DTOs;
using MotoSpares.Application.DTOs.Auth;
using MotoSpares.Application.DTOs.Customer;

namespace MotoSpares.Application.Interfaces;

public interface ICustomerService
{
    Task<ApiResponse<AuthResponseDto>> RegisterCustomerWithVehicleAsync(RegisterCustomerWithVehicleDto dto);
}

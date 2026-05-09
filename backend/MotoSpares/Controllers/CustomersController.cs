using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotoSpares.Application.DTOs.Customer;
using MotoSpares.Application.Interfaces;
using System.Threading.Tasks;

namespace MotoSpares.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpPost("register-with-vehicle")]
    [Authorize(Roles = "Staff,Admin")]
    public async Task<IActionResult> RegisterWithVehicle([FromBody] RegisterCustomerWithVehicleDto dto)
    {
        var result = await _customerService.RegisterCustomerWithVehicleAsync(dto);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}

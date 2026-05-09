using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotoSpares.Application.DTOs;
using MotoSpares.Application.DTOs.Customer;
using MotoSpares.Application.Interfaces;

namespace MotoSpares.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Staff,Admin")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet("search")]
    public async Task<ActionResult<ApiResponse<IEnumerable<CustomerSearchResponseDto>>>> Search([FromQuery] string q)
    {
        var result = await _customerService.SearchCustomersAsync(q);
        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }
}

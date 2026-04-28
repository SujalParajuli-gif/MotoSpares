using Microsoft.AspNetCore.Mvc;
using MotoSpares.Application.DTOs.Customers;
using MotoSpares.Application.Services;

namespace MotoSpares.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly CustomerService _customerService;

    public CustomerController(CustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerDto dto)
    {
        var result = await _customerService.CreateCustomerAsync(dto);
        return CreatedAtAction(nameof(GetCustomerById), new { id = result.Id }, result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCustomers()
    {
        var result = await _customerService.GetAllCustomersAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomerById(Guid id)
    {
        var result = await _customerService.GetCustomerByIdAsync(id);

        if (result == null)
            return NotFound();

        return Ok(result);
    }
}
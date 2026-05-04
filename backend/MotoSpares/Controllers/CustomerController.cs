using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotoSpares.Application.Interfaces;

namespace MotoSpares.Controllers;

[ApiController]
[Route("api/customers")]
[Authorize(Roles = "Admin,Staff")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCustomers()
    {
        var customers = await _customerService.GetAllCustomersAsync();
        return Ok(new { isSuccess = true, count = customers.Count, data = customers });
    }

    [HttpGet("{id:guid}/details")]
    public async Task<IActionResult> GetCustomerDetails(Guid id)
    {
        var details = await _customerService.GetCustomerDetailsAsync(id);
        if (details == null)
            return NotFound(new { isSuccess = false, message = "Customer not found." });

        return Ok(new { isSuccess = true, data = details });
    }

    [HttpGet("{id:guid}/history")]
    public async Task<IActionResult> GetCustomerHistory(Guid id)
    {
        var history = await _customerService.GetCustomerHistoryAsync(id);
        if (history == null)
            return NotFound(new { isSuccess = false, message = "Customer not found." });

        return Ok(new { isSuccess = true, data = history });
    }

    [HttpGet("{id:guid}/vehicles")]
    public async Task<IActionResult> GetCustomerVehicles(Guid id)
    {
        var vehicles = await _customerService.GetCustomerVehiclesAsync(id);
        if (vehicles == null)
            return NotFound(new { isSuccess = false, message = "Customer not found." });

        return Ok(new { isSuccess = true, data = vehicles });
    }
}
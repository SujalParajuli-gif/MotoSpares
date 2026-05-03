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

    /// <summary>
    /// GET api/customers
    /// Returns all customers — lightweight list for staff dashboard
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllCustomers()
    {
        var customers = await _customerService.GetAllCustomersAsync();

        return Ok(new
        {
            isSuccess = true,
            count = customers.Count,
            data = customers
        });
    }

    /// <summary>
    /// GET api/customers/{id}
    /// Returns full profile — details + vehicles + purchase history
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCustomerProfile(Guid id)
    {
        var profile = await _customerService.GetCustomerProfileAsync(id);

        if (profile is null)
            return NotFound(new
            {
                isSuccess = false,
                message = "Customer not found."
            });

        return Ok(new
        {
            isSuccess = true,
            data = profile
        });
    }
}
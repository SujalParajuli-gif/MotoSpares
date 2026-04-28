using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotoSpares.Application.DTOs.Vendors;
using MotoSpares.Application.Services;

namespace MotoSpares.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class VendorController : ControllerBase
{
    private readonly VendorService _vendorService;

    public VendorController(VendorService vendorService)
    {
        _vendorService = vendorService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateVendor([FromBody] CreateVendorDto dto)
    {
        var result = await _vendorService.CreateVendorAsync(dto);
        return CreatedAtAction(nameof(GetVendorById), new { id = result.Id }, result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllVendors()
    {
        var result = await _vendorService.GetAllVendorsAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetVendorById(Guid id)
    {
        var result = await _vendorService.GetVendorByIdAsync(id);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateVendor(Guid id, [FromBody] UpdateVendorDto dto)
    {
        var result = await _vendorService.UpdateVendorAsync(id, dto);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteVendor(Guid id)
    {
        var result = await _vendorService.DeleteVendorAsync(id);

        if (!result)
            return NotFound();

        return NoContent();
    }
}
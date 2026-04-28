using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotoSpares.Application.DTOs.Staff;
using MotoSpares.Application.Services;

namespace MotoSpares.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class StaffController : ControllerBase
{
    private readonly StaffService _staffService;

    public StaffController(StaffService staffService)
    {
        _staffService = staffService;
    }

    // POST /api/staff
    [HttpPost]
    public async Task<IActionResult> CreateStaff([FromBody] CreateStaffDto dto)
    {
        var result = await _staffService.CreateStaffAsync(dto);
        return CreatedAtAction(nameof(GetStaffById), new { id = result.Id }, result);
    }

    // GET /api/staff
    [HttpGet]
    public async Task<IActionResult> GetAllStaff()
    {
        var result = await _staffService.GetAllStaffAsync();
        return Ok(result);
    }

    // GET /api/staff/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetStaffById(Guid id)
    {
        var result = await _staffService.GetStaffByIdAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    // PUT /api/staff/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStaff(Guid id, [FromBody] UpdateStaffDto dto)
    {
        var result = await _staffService.UpdateStaffAsync(id, dto);
        if (result == null) return NotFound();
        return Ok(result);
    }

    // DELETE /api/staff/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStaff(Guid id)
    {
        var result = await _staffService.DeleteStaffAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        throw new Exception("This is a test error");
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotoSpares.Application.DTOs.Auth;
using MotoSpares.Application.DTOs.Staff;
using MotoSpares.Application.Interfaces;

namespace MotoSpares.Controllers;

[ApiController]
[Route("api/staff")]
[Authorize(Roles = "Admin")]
public class StaffController : ControllerBase
{
    private readonly IStaffService _staffService;
    private readonly IAuthService _authService;

    public StaffController(IStaffService staffService, IAuthService authService)
    {
        _staffService = staffService;
        _authService = authService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllStaff()
    {
        var result = await _staffService.GetAllStaffAsync();
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetStaffById(string id)
    {
        var result = await _staffService.GetStaffByIdAsync(id);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddStaff([FromBody] RegisterStaffDto dto)
    {
        // Re-use the logic from AuthService for consistency in staff creation
        var result = await _authService.RegisterStaffAsync(dto);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStaff(string id, [FromBody] UpdateStaffDto dto)
    {
        var result = await _staffService.UpdateStaffAsync(id, dto);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStaff(string id)
    {
        var result = await _staffService.DeleteStaffAsync(id);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotoSpares.Application.DTOs.Vehicle;
using MotoSpares.Application.Interfaces;

namespace MotoSpares.Controllers;

[ApiController]
[Route("api/vehicles")]
[Authorize(Roles = "Customer")]
public class VehiclesController : ControllerBase
{
    private readonly IVehicleService _vehicleService;

    public VehiclesController(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }

    private string GetUserId() =>
        User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    [HttpGet]
    public async Task<IActionResult> GetMyVehicles()
    {
        var result = await _vehicleService.GetMyVehiclesAsync(GetUserId());
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddVehicle([FromBody] CreateVehicleDto dto)
    {
        var result = await _vehicleService.AddVehicleAsync(GetUserId(), dto);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{vehicleId:int}")]
    public async Task<IActionResult> UpdateVehicle(int vehicleId, [FromBody] UpdateVehicleDto dto)
    {
        var result = await _vehicleService.UpdateVehicleAsync(GetUserId(), vehicleId, dto);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{vehicleId:int}")]
    public async Task<IActionResult> DeleteVehicle(int vehicleId)
    {
        var result = await _vehicleService.DeleteVehicleAsync(GetUserId(), vehicleId);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}

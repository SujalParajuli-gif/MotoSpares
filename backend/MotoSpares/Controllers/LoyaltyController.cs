using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotoSpares.Application.DTOs.Loyalty;
using MotoSpares.Application.Interfaces;

namespace MotoSpares.Controllers;

[ApiController]
[Route("api/loyalty")]
[Authorize]
public class LoyaltyController : ControllerBase
{
    private readonly ILoyaltyService _loyaltyService;

    public LoyaltyController(ILoyaltyService loyaltyService)
    {
        _loyaltyService = loyaltyService;
    }

    /// <summary>
    /// Check own loyalty status (Customer)
    /// </summary>
    [HttpGet("status")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> GetMyLoyaltyStatus()
    {
        var userId = GetUserId();
        var result = await _loyaltyService.GetLoyaltyStatusAsync(userId);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    /// <summary>
    /// Check a customer's loyalty status (Admin/Staff)
    /// </summary>
    [HttpGet("status/{customerId:guid}")]
    [Authorize(Roles = "Admin,Staff")]
    public async Task<IActionResult> GetCustomerLoyaltyStatus(Guid customerId)
    {
        var result = await _loyaltyService.GetLoyaltyStatusAsync(customerId);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    /// <summary>
    /// Calculate loyalty discount for a sale (Admin/Staff)
    /// </summary>
    [HttpPost("calculate-discount")]
    [Authorize(Roles = "Admin,Staff")]
    public async Task<IActionResult> CalculateDiscount([FromBody] CalculateDiscountDto dto)
    {
        var result = await _loyaltyService.CalculateDiscountAsync(dto);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.Parse(userIdClaim!);
    }
}

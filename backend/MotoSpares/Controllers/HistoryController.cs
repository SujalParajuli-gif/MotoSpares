using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotoSpares.Application.Interfaces;

namespace MotoSpares.Controllers;

[ApiController]
[Route("api/history")]
[Authorize(Roles = "Customer")]
public class HistoryController : ControllerBase
{
    private readonly IHistoryService _historyService;

    public HistoryController(IHistoryService historyService)
    {
        _historyService = historyService;
    }

    /// <summary>
    /// Get full purchase and service history for the logged-in customer
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetFullHistory()
    {
        var userId = GetUserId();
        var result = await _historyService.GetFullHistoryAsync(userId);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    /// <summary>
    /// Get purchase history only
    /// </summary>
    [HttpGet("purchases")]
    public async Task<IActionResult> GetPurchaseHistory()
    {
        var userId = GetUserId();
        var result = await _historyService.GetPurchaseHistoryAsync(userId);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    /// <summary>
    /// Get service/appointment history only
    /// </summary>
    [HttpGet("services")]
    public async Task<IActionResult> GetServiceHistory()
    {
        var userId = GetUserId();
        var result = await _historyService.GetServiceHistoryAsync(userId);
        return Ok(result);
    }

    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.Parse(userIdClaim!);
    }
}

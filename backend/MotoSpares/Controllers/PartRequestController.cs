using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotoSpares.Application.DTOs.PartRequest;
using MotoSpares.Application.Interfaces;

namespace MotoSpares.Controllers;

[ApiController]
[Route("api/part-requests")]
[Authorize]
public class PartRequestController : ControllerBase
{
    private readonly IPartRequestService _partRequestService;

    public PartRequestController(IPartRequestService partRequestService)
    {
        _partRequestService = partRequestService;
    }

    /// <summary>
    /// Submit a new part request (Customer only)
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> CreatePartRequest([FromBody] CreatePartRequestDto dto)
    {
        var userId = GetUserId();
        var result = await _partRequestService.CreatePartRequestAsync(dto, userId);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Get the logged-in customer's part requests
    /// </summary>
    [HttpGet("my")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> GetMyPartRequests()
    {
        var userId = GetUserId();
        var result = await _partRequestService.GetMyPartRequestsAsync(userId);
        return Ok(result);
    }

    /// <summary>
    /// Get all part requests (Admin/Staff only)
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin,Staff")]
    public async Task<IActionResult> GetAllPartRequests()
    {
        var result = await _partRequestService.GetAllPartRequestsAsync();
        return Ok(result);
    }

    /// <summary>
    /// Update part request status (Admin/Staff only)
    /// </summary>
    [HttpPut("{id:int}/status")]
    [Authorize(Roles = "Admin,Staff")]
    public async Task<IActionResult> UpdatePartRequestStatus(int id, [FromBody] UpdatePartRequestStatusDto dto)
    {
        var result = await _partRequestService.UpdatePartRequestStatusAsync(id, dto);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.Parse(userIdClaim!);
    }
}

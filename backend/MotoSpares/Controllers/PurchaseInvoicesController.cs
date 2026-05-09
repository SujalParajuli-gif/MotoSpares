using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotoSpares.Application.DTOs;
using MotoSpares.Application.DTOs.Purchase;
using MotoSpares.Application.Interfaces;

namespace MotoSpares.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class PurchaseInvoicesController : ControllerBase
{
    private readonly IPurchaseInvoiceService _invoiceService;

    public PurchaseInvoicesController(IPurchaseInvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<PurchaseInvoiceResponseDto>>> Create(CreatePurchaseInvoiceDto dto)
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userIdString, out Guid userId))
            return Unauthorized(ApiResponse<PurchaseInvoiceResponseDto>.Fail("Invalid user token."));

        var result = await _invoiceService.CreateAsync(dto, userId);
        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<PurchaseInvoiceResponseDto>>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _invoiceService.GetAllAsync(page, pageSize);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<PurchaseInvoiceResponseDto>>> GetById(int id)
    {
        var result = await _invoiceService.GetByIdAsync(id);
        if (!result.IsSuccess)
            return NotFound(result);

        return Ok(result);
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotoSpares.Application.Interfaces;

namespace MotoSpares.Controllers;

[ApiController]
[Route("api/invoices")]
public class InvoiceController : ControllerBase
{
    private readonly IInvoiceService _invoiceService;

    public InvoiceController(IInvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
    }

    [HttpPost("{invoiceId:int}/send-email")]
    [Authorize(Roles = "Admin,Staff")]
    public async Task<IActionResult> SendEmail(int invoiceId)
    {
        try
        {
            await _invoiceService.SendInvoiceEmailAsync(invoiceId);
            return Ok(new { isSuccess = true, message = "Email sent successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { isSuccess = false, message = ex.Message });
        }
    }
}
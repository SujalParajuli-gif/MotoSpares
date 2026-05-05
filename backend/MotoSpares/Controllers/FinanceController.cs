using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotoSpares.Application.DTOs.Finance;
using MotoSpares.Application.Interfaces;

namespace MotoSpares.Controllers;

[ApiController]
[Route("api/finance")]
[Authorize(Roles = "Admin,Staff")]
public class FinanceController : ControllerBase
{
    private readonly IFinanceService _financeService;

    public FinanceController(IFinanceService financeService)
    {
        _financeService = financeService;
    }

    [HttpGet("daily")]
    public async Task<ActionResult<FinancialSummaryDto>> GetDailyReport([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        var report = await _financeService.GetDailyReportAsync(startDate, endDate);
        return Ok(report);
    }

    [HttpGet("monthly")]
    public async Task<ActionResult<FinancialSummaryDto>> GetMonthlyReport([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        var report = await _financeService.GetMonthlyReportAsync(startDate, endDate);
        return Ok(report);
    }

    [HttpGet("yearly")]
    public async Task<ActionResult<FinancialSummaryDto>> GetYearlyReport([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        var report = await _financeService.GetYearlyReportAsync(startDate, endDate);
        return Ok(report);
    }
}

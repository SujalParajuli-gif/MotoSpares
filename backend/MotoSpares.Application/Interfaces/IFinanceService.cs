using MotoSpares.Application.DTOs.Finance;

namespace MotoSpares.Application.Interfaces;

public interface IFinanceService
{
    Task<FinancialSummaryDto> GetDailyReportAsync(DateTime? startDate, DateTime? endDate);
    Task<FinancialSummaryDto> GetMonthlyReportAsync(DateTime? startDate, DateTime? endDate);
    Task<FinancialSummaryDto> GetYearlyReportAsync(DateTime? startDate, DateTime? endDate);
}

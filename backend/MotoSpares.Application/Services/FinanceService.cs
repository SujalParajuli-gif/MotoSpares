using MotoSpares.Application.DTOs.Finance;
using MotoSpares.Application.Interfaces;
using MotoSpares.Application.Interfaces.Repositories;

namespace MotoSpares.Application.Services;

public class FinanceService : IFinanceService
{
    private readonly IFinanceRepository _financeRepository;

    public FinanceService(IFinanceRepository financeRepository)
    {
        _financeRepository = financeRepository;
    }

    public async Task<FinancialSummaryDto> GetDailyReportAsync(DateTime? startDate, DateTime? endDate)
    {
        // Default: Last 7 days
        var end = endDate ?? DateTime.UtcNow.Date.AddDays(1).AddTicks(-1);
        var start = startDate ?? end.Date.AddDays(-6);

        var sales = await _financeRepository.GetSalesInDateRangeAsync(start, end);
        var purchases = await _financeRepository.GetPurchasesInDateRangeAsync(start, end);

        var groupedSales = sales.GroupBy(s => s.SaleDate.Date).ToDictionary(g => g.Key, g => g.ToList());
        var groupedPurchases = purchases.GroupBy(p => p.PurchaseDate.Date).ToDictionary(g => g.Key, g => g.ToList());

        var reports = new List<FinancialReportDto>();

        for (var date = start.Date; date <= end.Date; date = date.AddDays(1))
        {
            var dailySales = groupedSales.GetValueOrDefault(date, new());
            var dailyPurchases = groupedPurchases.GetValueOrDefault(date, new());

            var totalSales = dailySales.Sum(s => s.TotalAmount);
            var totalPurchases = dailyPurchases.Sum(p => p.PurchaseTotal);
            var totalDiscounts = dailySales.Sum(s => s.DiscountAmount);

            reports.Add(new FinancialReportDto
            {
                PeriodLabel = date.ToString("yyyy-MM-dd"),
                TotalSales = totalSales,
                TotalPurchases = totalPurchases,
                TotalDiscounts = totalDiscounts,
                NetProfit = totalSales - totalPurchases - totalDiscounts
            });
        }

        return CreateSummary("Daily", start, end, reports);
    }

    public async Task<FinancialSummaryDto> GetMonthlyReportAsync(DateTime? startDate, DateTime? endDate)
    {
        // Default: Current Year
        var currentYear = DateTime.UtcNow.Year;
        var start = startDate ?? new DateTime(currentYear, 1, 1);
        var end = endDate ?? new DateTime(currentYear, 12, 31, 23, 59, 59);

        var sales = await _financeRepository.GetSalesInDateRangeAsync(start, end);
        var purchases = await _financeRepository.GetPurchasesInDateRangeAsync(start, end);

        var groupedSales = sales.GroupBy(s => new { s.SaleDate.Year, s.SaleDate.Month })
                                .ToDictionary(g => g.Key, g => g.ToList());
        var groupedPurchases = purchases.GroupBy(p => new { p.PurchaseDate.Year, p.PurchaseDate.Month })
                                        .ToDictionary(g => g.Key, g => g.ToList());

        var reports = new List<FinancialReportDto>();

        for (var date = new DateTime(start.Year, start.Month, 1); date <= end; date = date.AddMonths(1))
        {
            var key = new { date.Year, date.Month };
            var monthlySales = groupedSales.GetValueOrDefault(key, new());
            var monthlyPurchases = groupedPurchases.GetValueOrDefault(key, new());

            var totalSales = monthlySales.Sum(s => s.TotalAmount);
            var totalPurchases = monthlyPurchases.Sum(p => p.PurchaseTotal);
            var totalDiscounts = monthlySales.Sum(s => s.DiscountAmount);

            reports.Add(new FinancialReportDto
            {
                PeriodLabel = date.ToString("yyyy-MMM"),
                TotalSales = totalSales,
                TotalPurchases = totalPurchases,
                TotalDiscounts = totalDiscounts,
                NetProfit = totalSales - totalPurchases - totalDiscounts
            });
        }

        return CreateSummary("Monthly", start, end, reports);
    }

    public async Task<FinancialSummaryDto> GetYearlyReportAsync(DateTime? startDate, DateTime? endDate)
    {
        // Default: Last 5 Years
        var currentYear = DateTime.UtcNow.Year;
        var start = startDate ?? new DateTime(currentYear - 4, 1, 1);
        var end = endDate ?? new DateTime(currentYear, 12, 31, 23, 59, 59);

        var sales = await _financeRepository.GetSalesInDateRangeAsync(start, end);
        var purchases = await _financeRepository.GetPurchasesInDateRangeAsync(start, end);

        var groupedSales = sales.GroupBy(s => s.SaleDate.Year).ToDictionary(g => g.Key, g => g.ToList());
        var groupedPurchases = purchases.GroupBy(p => p.PurchaseDate.Year).ToDictionary(g => g.Key, g => g.ToList());

        var reports = new List<FinancialReportDto>();

        for (var year = start.Year; year <= end.Year; year++)
        {
            var yearlySales = groupedSales.GetValueOrDefault(year, new());
            var yearlyPurchases = groupedPurchases.GetValueOrDefault(year, new());

            var totalSales = yearlySales.Sum(s => s.TotalAmount);
            var totalPurchases = yearlyPurchases.Sum(p => p.PurchaseTotal);
            var totalDiscounts = yearlySales.Sum(s => s.DiscountAmount);

            reports.Add(new FinancialReportDto
            {
                PeriodLabel = year.ToString(),
                TotalSales = totalSales,
                TotalPurchases = totalPurchases,
                TotalDiscounts = totalDiscounts,
                NetProfit = totalSales - totalPurchases - totalDiscounts
            });
        }

        return CreateSummary("Yearly", start, end, reports);
    }

    private static FinancialSummaryDto CreateSummary(string reportType, DateTime start, DateTime end, List<FinancialReportDto> reports)
    {
        return new FinancialSummaryDto
        {
            ReportType = reportType,
            DateRange = $"{start:yyyy-MM-dd} to {end:yyyy-MM-dd}",
            AggregateSales = reports.Sum(r => r.TotalSales),
            AggregatePurchases = reports.Sum(r => r.TotalPurchases),
            AggregateDiscounts = reports.Sum(r => r.TotalDiscounts),
            AggregateNetProfit = reports.Sum(r => r.NetProfit),
            Reports = reports
        };
    }
}

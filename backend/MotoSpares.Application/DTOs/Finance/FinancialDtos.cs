namespace MotoSpares.Application.DTOs.Finance;

public class FinancialReportDto
{
    public string PeriodLabel { get; set; } = string.Empty;
    public decimal TotalSales { get; set; }
    public decimal TotalPurchases { get; set; }
    public decimal TotalDiscounts { get; set; }
    public decimal NetProfit { get; set; }
}

public class FinancialSummaryDto
{
    public DateTime ReportGeneratedAt { get; set; } = DateTime.UtcNow;
    public string ReportType { get; set; } = string.Empty; // "Daily", "Monthly", "Yearly"
    public string DateRange { get; set; } = string.Empty;
    
    public decimal AggregateSales { get; set; }
    public decimal AggregatePurchases { get; set; }
    public decimal AggregateDiscounts { get; set; }
    public decimal AggregateNetProfit { get; set; }

    public List<FinancialReportDto> Reports { get; set; } = new();
}

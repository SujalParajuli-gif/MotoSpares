using System.ComponentModel.DataAnnotations;

namespace MotoSpares.Application.DTOs.Loyalty;

public class LoyaltyStatusDto
{
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public decimal TotalSpend { get; set; }
    public bool IsLoyal { get; set; }
    public int DiscountPercent { get; set; }
    public string Tier { get; set; } = string.Empty;
}

public class CalculateDiscountDto
{
    [Required]
    public Guid CustomerId { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Subtotal { get; set; }
}

public class DiscountResultDto
{
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public bool IsLoyal { get; set; }
    public string Tier { get; set; } = string.Empty;
    public decimal Subtotal { get; set; }
    public int DiscountPercent { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal FinalTotal { get; set; }
}

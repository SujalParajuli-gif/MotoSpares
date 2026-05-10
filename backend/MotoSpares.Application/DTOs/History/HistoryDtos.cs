namespace MotoSpares.Application.DTOs.History;

public class FullHistoryDto
{
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public List<PurchaseHistoryItemDto> Purchases { get; set; } = new();
    public List<ServiceHistoryItemDto> Services { get; set; } = new();
}

public class PurchaseHistoryItemDto
{
    public int SaleInvoiceId { get; set; }
    public DateTime SaleDate { get; set; }
    public decimal Subtotal { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public string PaymentStatus { get; set; } = string.Empty;
    public List<PurchaseLineItemDto> Items { get; set; } = new();
}

public class PurchaseLineItemDto
{
    public string PartName { get; set; } = string.Empty;
    public string PartNumber { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal LineTotal { get; set; }
}

public class ServiceHistoryItemDto
{
    public int AppointmentId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string ServiceType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? Notes { get; set; }
}

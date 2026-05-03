namespace MotoSpares.Application.DTOs.Customers;

public class CustomerVehicleDto
{
    public int VehicleId { get; set; }
    public string VehicleNumber { get; set; } = string.Empty;
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
}

public class CustomerSaleItemDto
{
    public string PartName { get; set; } = string.Empty;
    public string PartSKU { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal LineTotal { get; set; }
}

public class CustomerInvoiceDto
{
    public int SaleInvoiceId { get; set; }
    public DateTime SaleDate { get; set; }
    public decimal Subtotal { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public string PaymentStatus { get; set; } = string.Empty;
    public DateTime? CreditDueDate { get; set; }
    public List<CustomerSaleItemDto> Items { get; set; } = [];
}

public class CustomerProfileDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public string Role { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    // Vehicles owned by this customer
    public List<CustomerVehicleDto> Vehicles { get; set; } = [];

    // Full purchase history
    public List<CustomerInvoiceDto> PurchaseHistory { get; set; } = [];
}

public class CustomerListDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public DateTime CreatedAt { get; set; }
    public int TotalVehicles { get; set; }
    public int TotalInvoices { get; set; }
}
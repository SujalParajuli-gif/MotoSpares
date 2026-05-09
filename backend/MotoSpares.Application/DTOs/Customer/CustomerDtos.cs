namespace MotoSpares.Application.DTOs.Customer;

public class CustomerDetailsDto
{
    public Guid CustomerId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public decimal TotalSpend { get; set; }
    public decimal CreditBalance { get; set; }
    public string LoyaltyStatus { get; set; } = string.Empty;
}

public class CustomerHistoryDto
{
    public Guid CustomerId { get; set; }
    public List<SaleInvoiceDto> PurchaseHistory { get; set; } = new();
    public List<AppointmentDto> ServiceHistory { get; set; } = new();
}

public class SaleInvoiceDto
{
    public int SaleInvoiceId { get; set; }
    public DateTime SaleDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string PaymentStatus { get; set; } = string.Empty;
    public List<SaleItemDto> Items { get; set; } = new();
}

public class SaleItemDto
{
    public string PartName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}

public class AppointmentDto
{
    public int AppointmentId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string ServiceType { get; set; } = string.Empty;
    public string AppointmentStatus { get; set; } = string.Empty;
}

public class CustomerVehiclesDto
{
    public Guid CustomerId { get; set; }
    public List<VehicleDto> Vehicles { get; set; } = new();
}

public class VehicleDto
{
    public int VehicleId { get; set; }
    public string VehicleNumber { get; set; } = string.Empty;
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
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

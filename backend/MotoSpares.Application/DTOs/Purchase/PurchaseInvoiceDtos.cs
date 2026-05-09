using System.ComponentModel.DataAnnotations;

namespace MotoSpares.Application.DTOs.Purchase;

public class CreatePurchaseInvoiceDto
{
    [Required]
    public int VendorId { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "At least one item is required in the invoice.")]
    public List<CreatePurchaseItemDto> Items { get; set; } = new();
}

public class CreatePurchaseItemDto
{
    [Required]
    public int PartId { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Purchase quantity must be at least 1.")]
    public int PurchaseQuantity { get; set; }

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Unit cost must be greater than zero.")]
    public decimal PurchaseUnitCost { get; set; }
}

public class PurchaseInvoiceResponseDto
{
    public int PurchaseInvoiceId { get; set; }
    public DateTime PurchaseDate { get; set; }
    public decimal PurchaseTotal { get; set; }
    public int VendorId { get; set; }
    public string VendorName { get; set; } = string.Empty;
    public Guid CreatedBy { get; set; }
    public List<PurchaseItemResponseDto> Items { get; set; } = new();
}

public class PurchaseItemResponseDto
{
    public int PurchaseItemId { get; set; }
    public int PartId { get; set; }
    public string PartName { get; set; } = string.Empty;
    public int PurchaseQuantity { get; set; }
    public decimal PurchaseUnitCost { get; set; }
    public decimal TotalCost => PurchaseQuantity * PurchaseUnitCost;
}

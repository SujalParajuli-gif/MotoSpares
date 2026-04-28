using System.ComponentModel.DataAnnotations;

namespace MotoSpares.Domain.Entities;

public class Part
{
    public int PartId { get; set; }

    [Required]
    [StringLength(100)]
    public string PartName { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string PartNumber { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    [StringLength(100)]
    public string? Category { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal UnitPrice { get; set; }

    [Range(0, int.MaxValue)]
    public int StockQuantity { get; set; }

    [Range(0, int.MaxValue)]
    public int ReorderLevel { get; set; }

    public ICollection<PurchaseItem> PurchaseItems { get; set; } = new List<PurchaseItem>();
    public ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
    public ICollection<UserPart> UserParts { get; set; } = new List<UserPart>();
}

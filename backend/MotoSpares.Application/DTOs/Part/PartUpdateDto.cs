using System.ComponentModel.DataAnnotations;

namespace MotoSpares.Application.DTOs.Part;

public class PartUpdateDto
{
    [Required]
    public int PartId { get; set; }

    [Required(ErrorMessage = "Part name is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Part name must be between 2 and 100 characters")]
    public string PartName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Part number is required")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Part number must be between 2 and 50 characters")]
    public string PartNumber { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    [StringLength(100)]
    public string? Category { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "Unit price must be greater than zero")]
    public decimal UnitPrice { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Stock quantity cannot be negative")]
    public int StockQuantity { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Reorder level cannot be negative")]
    public int ReorderLevel { get; set; }
}

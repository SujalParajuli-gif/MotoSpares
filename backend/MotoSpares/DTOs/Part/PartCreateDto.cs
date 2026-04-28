using System.ComponentModel.DataAnnotations;

namespace MotoSpares.DTOs.Part
{
    public class PartCreateDto
    {
        [Required(ErrorMessage = "Part name is required")]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string PartNumber { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        [MaxLength(50)]
        public string Category { get; set; } = string.Empty;

        [Range(0, double.MaxValue)]
        public decimal PurchasePrice { get; set; }

        [Range(0, double.MaxValue)]
        public decimal SellingPrice { get; set; }

        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }

        [Range(1, int.MaxValue)]
        public int ReorderLevel { get; set; } = 10;

        public int? VendorId { get; set; }
    }
}

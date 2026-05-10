using System.ComponentModel.DataAnnotations;

namespace MotoSpares.Application.DTOs.Review;

public class CreateReviewDto
{
    [Required]
    [Range(1, 5)]
    public int Rating { get; set; }

    [StringLength(1000)]
    public string? ReviewText { get; set; }
}

public class ReviewResponseDto
{
    public int ReviewId { get; set; }
    public int Rating { get; set; }
    public string? ReviewText { get; set; }
    public DateTime ReviewDate { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public Guid CustomerId { get; set; }
}

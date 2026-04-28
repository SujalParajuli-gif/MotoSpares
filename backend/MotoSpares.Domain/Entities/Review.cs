using System.ComponentModel.DataAnnotations;

namespace MotoSpares.Domain.Entities;

public class Review
{
    public int ReviewId { get; set; }

    [Range(1, 5)]
    public int Rating { get; set; }

    [StringLength(1000)]
    public string? ReviewText { get; set; }

    public DateTime ReviewDate { get; set; }

    public ICollection<UserReview> UserReviews { get; set; } = new List<UserReview>();
}

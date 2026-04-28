namespace MotoSpares.Domain.Entities;

public class UserReview
{
    public Guid UserId { get; set; }
    public int ReviewId { get; set; }

    public ApplicationUser? User { get; set; }
    public Review? Review { get; set; }
}

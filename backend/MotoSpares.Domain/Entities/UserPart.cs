namespace MotoSpares.Domain.Entities;

public class UserPart
{
    public Guid UserId { get; set; }
    public int PartId { get; set; }

    public ApplicationUser? User { get; set; }
    public Part? Part { get; set; }
}

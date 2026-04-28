namespace MotoSpares.Domain.Entities;

public class UserPartRequest
{
    public Guid UserId { get; set; }
    public int RequestId { get; set; }

    public ApplicationUser? User { get; set; }
    public PartRequest? PartRequest { get; set; }
}

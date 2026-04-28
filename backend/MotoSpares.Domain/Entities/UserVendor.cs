namespace MotoSpares.Domain.Entities;

public class UserVendor
{
    public Guid UserId { get; set; }
    public int VendorId { get; set; }

    public ApplicationUser? User { get; set; }
    public Vendor? Vendor { get; set; }
}

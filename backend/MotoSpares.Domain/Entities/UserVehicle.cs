namespace MotoSpares.Domain.Entities;

public class UserVehicle
{
    public Guid UserId { get; set; }
    public int VehicleId { get; set; }

    public ApplicationUser? User { get; set; }
    public Vehicle? Vehicle { get; set; }
}

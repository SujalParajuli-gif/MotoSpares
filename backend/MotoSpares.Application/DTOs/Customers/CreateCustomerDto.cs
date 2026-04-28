namespace MotoSpares.Application.DTOs.Customers;

public class CreateCustomerDto
{
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;

    public List<CreateVehicleDto> Vehicles { get; set; } = new();
}
namespace MotoSpares.Domain.Entities;

public class Vehicle
{
    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }

    public string VehicleNumber { get; private set; } = string.Empty;
    public string Brand { get; private set; } = string.Empty;
    public string Model { get; private set; } = string.Empty;
    public int Year { get; private set; }

    public Customer? Customer { get; private set; }

    private Vehicle() { }

    public Vehicle(Guid customerId, string vehicleNumber, string brand, string model, int year)
    {
        Id = Guid.NewGuid();
        CustomerId = customerId;
        VehicleNumber = vehicleNumber.Trim().ToUpper();
        Brand = brand.Trim();
        Model = model.Trim();
        Year = year;
    }

    public void UpdateDetails(string vehicleNumber, string brand, string model, int year)
    {
        VehicleNumber = vehicleNumber.Trim().ToUpper();
        Brand = brand.Trim();
        Model = model.Trim();
        Year = year;
    }
}
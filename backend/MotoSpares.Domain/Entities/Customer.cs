namespace MotoSpares.Domain.Entities;

public class Customer
{
    public Guid Id { get; private set; }
    public string FullName { get; private set; } = string.Empty;
    public string Phone { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Address { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }

    public ICollection<Vehicle> Vehicles { get; private set; } = new List<Vehicle>();

    private Customer() { }

    public Customer(string fullName, string phone, string email, string address)
    {
        Id = Guid.NewGuid();
        FullName = fullName.Trim();
        Phone = phone.Trim();
        Email = email.Trim().ToLower();
        Address = address.Trim();
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateDetails(string fullName, string phone, string email, string address)
    {
        FullName = fullName.Trim();
        Phone = phone.Trim();
        Email = email.Trim().ToLower();
        Address = address.Trim();
    }
}
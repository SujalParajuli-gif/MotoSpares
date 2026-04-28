namespace MotoSpares.Domain.Entities;

public class Vendor
{
    public Guid Id { get; private set; }
    public string VendorName { get; private set; } = string.Empty;
    public string ContactPerson { get; private set; } = string.Empty;
    public string Phone { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Address { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }

    private Vendor() { }

    public Vendor(string vendorName, string contactPerson, string phone, string email, string address)
    {
        Id = Guid.NewGuid();
        VendorName = vendorName.Trim();
        ContactPerson = contactPerson.Trim();
        Phone = phone.Trim();
        Email = email.Trim().ToLower();
        Address = address.Trim();
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateDetails(string vendorName, string contactPerson, string phone, string email, string address)
    {
        VendorName = vendorName.Trim();
        ContactPerson = contactPerson.Trim();
        Phone = phone.Trim();
        Email = email.Trim().ToLower();
        Address = address.Trim();
    }
}
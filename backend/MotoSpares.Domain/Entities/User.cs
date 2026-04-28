using MotoSpares.Domain.Enums;

namespace MotoSpares.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string FullName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public UserRole Role { get; private set; }
    public string Phone { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }

    // EF Core constructor
    private User() { }

    public User(string fullName, string email, string passwordHash, UserRole role, string phone)
    {
        Id = Guid.NewGuid();
        FullName = fullName.Trim();
        Email = email.Trim().ToLower();
        PasswordHash = passwordHash;
        Role = role;
        Phone = phone.Trim();
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateDetails(string fullName, string phone)
    {
        FullName = fullName.Trim();
        Phone = phone.Trim();
    }

    public void UpdatePassword(string passwordHash)
    {
        PasswordHash = passwordHash;
    }
}
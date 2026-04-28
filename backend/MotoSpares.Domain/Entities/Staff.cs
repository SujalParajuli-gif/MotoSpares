namespace MotoSpares.Domain.Entities;

public class Staff
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string EmployeeCode { get; private set; } = string.Empty;
    public string Department { get; private set; } = string.Empty;
    public DateTime JoinedAt { get; private set; }

    // Navigation property
    public User? User { get; private set; }

    // EF Core constructor
    private Staff() { }

    public Staff(Guid userId, string employeeCode, string department)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        EmployeeCode = employeeCode.Trim();
        Department = department.Trim();
        JoinedAt = DateTime.UtcNow;
    }

    public void UpdateDetails(string employeeCode, string department)
    {
        EmployeeCode = employeeCode.Trim();
        Department = department.Trim();
    }
}
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MotoSpares.Domain.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    [Required]
    [StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    [StringLength(200)]
    public string? Address { get; set; }

    [Required]
    [StringLength(20)]
    public string Role { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<UserVehicle> UserVehicles { get; set; } = new List<UserVehicle>();
    public ICollection<UserVendor> UserVendors { get; set; } = new List<UserVendor>();
    public ICollection<UserPart> UserParts { get; set; } = new List<UserPart>();
    public ICollection<UserPurchaseInvoice> UserPurchaseInvoices { get; set; } = new List<UserPurchaseInvoice>();
    public ICollection<UserSaleInvoice> UserSaleInvoices { get; set; } = new List<UserSaleInvoice>();
    public ICollection<UserAppointment> UserAppointments { get; set; } = new List<UserAppointment>();
    public ICollection<UserPartRequest> UserPartRequests { get; set; } = new List<UserPartRequest>();
    public ICollection<UserReview> UserReviews { get; set; } = new List<UserReview>();
    public ICollection<UserNotification> UserNotifications { get; set; } = new List<UserNotification>();
    public ICollection<PurchaseInvoice> CreatedPurchaseInvoices { get; set; } = new List<PurchaseInvoice>();
    public ICollection<SaleInvoice> StaffSaleInvoices { get; set; } = new List<SaleInvoice>();
}

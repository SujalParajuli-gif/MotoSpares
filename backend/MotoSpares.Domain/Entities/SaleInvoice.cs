using System.ComponentModel.DataAnnotations;
using MotoSpares.Domain.Enums;

namespace MotoSpares.Domain.Entities;

public class SaleInvoice
{
    public int SaleInvoiceId { get; set; }

    public DateTime SaleDate { get; set; }

    [Range(0, double.MaxValue)]
    public decimal Subtotal { get; set; }

    [Range(0, double.MaxValue)]
    public decimal DiscountAmount { get; set; }

    [Range(0, double.MaxValue)]
    public decimal TotalAmount { get; set; }

    public PaymentStatus PaymentStatus { get; set; }

    public DateTime? CreditDueDate { get; set; }

    public Guid StaffId { get; set; }

    public ApplicationUser? StaffUser { get; set; }
    public ICollection<SaleInvoiceItem> SaleInvoiceItems { get; set; } = new List<SaleInvoiceItem>();
    public ICollection<UserSaleInvoice> UserSaleInvoices { get; set; } = new List<UserSaleInvoice>();
}

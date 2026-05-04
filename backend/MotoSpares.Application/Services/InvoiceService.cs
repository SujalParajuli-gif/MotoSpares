using System.Text;
using MotoSpares.Application.Interfaces;
using MotoSpares.Application.Interfaces.Repositories;

namespace MotoSpares.Application.Services;

public class InvoiceService : IInvoiceService
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IEmailService _emailService;

    public InvoiceService(
        IInvoiceRepository invoiceRepository,
        IEmailService emailService)
    {
        _invoiceRepository = invoiceRepository;
        _emailService = emailService;
    }

    public async Task SendInvoiceEmailAsync(int saleInvoiceId)
    {
        var invoice = await _invoiceRepository.GetSaleInvoiceByIdAsync(saleInvoiceId);

        if (invoice == null)
            throw new Exception("Invoice not found");

        var customerLink = invoice.UserSaleInvoices.FirstOrDefault();
        var customer = customerLink?.User;

        if (customer == null || string.IsNullOrWhiteSpace(customer.Email))
            throw new Exception("Customer not found or email missing for this invoice");

        var sb = new StringBuilder();
        sb.AppendLine("<html><body style='font-family: Arial, sans-serif; color: #333;'>");
        sb.AppendLine($"<h2 style='color: #2c3e50;'>MotoSpares - Invoice #{invoice.SaleInvoiceId}</h2>");
        sb.AppendLine($"<p><strong>Date:</strong> {invoice.SaleDate:yyyy-MM-dd}</p>");
        sb.AppendLine($"<p><strong>Customer:</strong> {customer.FullName}</p>");
        sb.AppendLine("<hr style='border: 1px solid #eee;'/>");
        
        sb.AppendLine("<table style='width: 100%; border-collapse: collapse; margin-top: 20px;'>");
        sb.AppendLine("<tr style='background-color: #f8f9fa; text-align: left;'>");
        sb.AppendLine("<th style='padding: 10px; border-bottom: 2px solid #dee2e6;'>Part</th>");
        sb.AppendLine("<th style='padding: 10px; border-bottom: 2px solid #dee2e6;'>SKU</th>");
        sb.AppendLine("<th style='padding: 10px; border-bottom: 2px solid #dee2e6;'>Qty</th>");
        sb.AppendLine("<th style='padding: 10px; border-bottom: 2px solid #dee2e6;'>Price</th>");
        sb.AppendLine("<th style='padding: 10px; border-bottom: 2px solid #dee2e6;'>Total</th>");
        sb.AppendLine("</tr>");

        foreach (var item in invoice.SaleInvoiceItems)
        {
            if (item.SaleItem != null && item.SaleItem.Part != null)
            {
                var lineTotal = item.SaleItem.SaleQuantity * item.SaleItem.SaleUnitPrice;
                sb.AppendLine("<tr>");
                sb.AppendLine($"<td style='padding: 10px; border-bottom: 1px solid #eee;'>{item.SaleItem.Part.PartName}</td>");
                sb.AppendLine($"<td style='padding: 10px; border-bottom: 1px solid #eee;'>{item.SaleItem.Part.PartNumber}</td>");
                sb.AppendLine($"<td style='padding: 10px; border-bottom: 1px solid #eee;'>{item.SaleItem.SaleQuantity}</td>");
                sb.AppendLine($"<td style='padding: 10px; border-bottom: 1px solid #eee;'>${item.SaleItem.SaleUnitPrice:F2}</td>");
                sb.AppendLine($"<td style='padding: 10px; border-bottom: 1px solid #eee;'>${lineTotal:F2}</td>");
                sb.AppendLine("</tr>");
            }
        }

        sb.AppendLine("</table>");
        
        sb.AppendLine("<div style='margin-top: 20px; text-align: right;'>");
        sb.AppendLine($"<p><strong>Subtotal:</strong> ${invoice.Subtotal:F2}</p>");
        if (invoice.DiscountAmount > 0)
        {
            sb.AppendLine($"<p style='color: #e74c3c;'><strong>Discount:</strong> -${invoice.DiscountAmount:F2}</p>");
        }
        sb.AppendLine($"<h3 style='color: #27ae60;'><strong>Total Amount:</strong> ${invoice.TotalAmount:F2}</h3>");
        sb.AppendLine($"<p><strong>Payment Status:</strong> {invoice.PaymentStatus}</p>");
        sb.AppendLine("</div>");
        
        sb.AppendLine("<p style='margin-top: 30px; font-size: 0.9em; color: #7f8c8d;'>Thank you for choosing MotoSpares!</p>");
        sb.AppendLine("</body></html>");

        await _emailService.SendEmailAsync(
            customer.Email,
            $"Invoice #{invoice.SaleInvoiceId} from MotoSpares",
            sb.ToString()
        );
    }
}
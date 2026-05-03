using Microsoft.EntityFrameworkCore;
using MotoSpares.Application.DTOs.Customers;
using MotoSpares.Application.Interfaces.Repositories;
using MotoSpares.Infrastructure.Data;

namespace MotoSpares.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _context;

    public CustomerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<CustomerListDto>> GetAllCustomersAsync()
    {
        // Only return users with Role = "Customer"
        return await _context.Users
            .AsNoTracking()
            .Where(u => u.Role == "Customer")
            .Select(u => new CustomerListDto
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                Address = u.Address,
                CreatedAt = u.CreatedAt,
                TotalVehicles = u.UserVehicles.Count,
                TotalInvoices = u.UserSaleInvoices.Count
            })
            .OrderBy(u => u.FullName)
            .ToListAsync();
    }

    public async Task<CustomerProfileDto?> GetCustomerProfileAsync(Guid userId)
    {
        var customer = await _context.Users
            .AsNoTracking()
            .Where(u => u.Id == userId && u.Role == "Customer")
            .Select(u => new CustomerProfileDto
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                Address = u.Address,
                Role = u.Role,
                CreatedAt = u.CreatedAt,

                // Vehicles via UserVehicle junction
                Vehicles = u.UserVehicles
                    .Select(uv => new CustomerVehicleDto
                    {
                        VehicleId = uv.Vehicle!.VehicleId,
                        VehicleNumber = uv.Vehicle.VehicleNumber,
                        Make = uv.Vehicle.Make,
                        Model = uv.Vehicle.Model,
                        Year = uv.Vehicle.Year
                    })
                    .ToList(),

                // Invoices via UserSaleInvoice junction
                // Newest invoices come first
                PurchaseHistory = u.UserSaleInvoices
                    .Select(usi => usi.SaleInvoice!)
                    .OrderByDescending(si => si.SaleDate)
                    .Select(si => new CustomerInvoiceDto
                    {
                        SaleInvoiceId = si.SaleInvoiceId,
                        SaleDate = si.SaleDate,
                        Subtotal = si.Subtotal,
                        DiscountAmount = si.DiscountAmount,
                        TotalAmount = si.TotalAmount,
                        PaymentStatus = si.PaymentStatus.ToString(),
                        CreditDueDate = si.CreditDueDate,

                        // Line items via SaleInvoiceItem → SaleItem → Part
                        Items = si.SaleInvoiceItems
    .Select(sii => new CustomerSaleItemDto
    {
        PartName = sii.SaleItem!.Part!.PartName,
        PartSKU = sii.SaleItem.Part.PartNumber,
        Quantity = sii.SaleItem.SaleQuantity,
        UnitPrice = sii.SaleItem.SaleUnitPrice,
        LineTotal = sii.SaleItem.SaleQuantity
                    * sii.SaleItem.SaleUnitPrice
    })
    .ToList()
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync();

        return customer;
    }
}
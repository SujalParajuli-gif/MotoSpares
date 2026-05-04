using MotoSpares.Application.DTOs.Customers;
using MotoSpares.Application.Interfaces;
using MotoSpares.Application.Interfaces.Repositories;
using MotoSpares.Domain.Entities;

namespace MotoSpares.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _repository;

    public CustomerService(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<CustomerListDto>> GetAllCustomersAsync()
    {
        var customers = await _repository.GetAllCustomersAsync();

        return customers.Select(u => new CustomerListDto
        {
            Id = u.Id,
            FullName = u.FullName,
            Email = u.Email,
            PhoneNumber = u.PhoneNumber,
            Address = u.Address,
            CreatedAt = u.CreatedAt,
            TotalVehicles = u.UserVehicles.Count,
            TotalInvoices = u.UserSaleInvoices.Count
        }).ToList();
    }

    public async Task<CustomerProfileDto?> GetCustomerProfileAsync(Guid userId)
    {
        var customer = await _repository.GetCustomerByIdAsync(userId);

        if (customer is null)
            return null;

        return MapToProfileDto(customer);
    }

    private static CustomerProfileDto MapToProfileDto(ApplicationUser user)
    {
        return new CustomerProfileDto
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Address = user.Address,
            Role = user.Role,
            CreatedAt = user.CreatedAt,

            // Vehicles via UserVehicle junction
            Vehicles = user.UserVehicles
                .Where(uv => uv.Vehicle != null)
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
            PurchaseHistory = user.UserSaleInvoices
                .Where(usi => usi.SaleInvoice != null)
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
                        .Where(sii => sii.SaleItem != null)
                        .Select(sii => new CustomerSaleItemDto
                        {
                            PartName = sii.SaleItem!.Part?.PartName ?? string.Empty,
                            PartSKU = sii.SaleItem.Part?.PartNumber ?? string.Empty,
                            Quantity = sii.SaleItem.SaleQuantity,
                            UnitPrice = sii.SaleItem.SaleUnitPrice,
                            LineTotal = sii.SaleItem.SaleQuantity * sii.SaleItem.SaleUnitPrice
                        })
                        .ToList()
                })
                .ToList()
        };
    }
}
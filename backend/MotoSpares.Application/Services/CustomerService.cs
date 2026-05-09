using MotoSpares.Application.DTOs;
using MotoSpares.Application.DTOs.Customer;
using MotoSpares.Application.Interfaces;
using MotoSpares.Application.Interfaces.Repositories;
using MotoSpares.Domain.Entities;

namespace MotoSpares.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<List<CustomerListDto>> GetAllCustomersAsync()
    {
        var customers = await _customerRepository.GetAllCustomersAsync();

        return customers.Select(u => new CustomerListDto
        {
            Id = u.Id,
            FullName = u.FullName,
            Email = u.Email,
            PhoneNumber = u.PhoneNumber,
            Address = u.Address,
            CreatedAt = u.CreatedAt,
            TotalVehicles = u.UserVehicles?.Count ?? 0,
            TotalInvoices = u.UserSaleInvoices?.Count ?? 0
        }).ToList();
    }

    public async Task<CustomerDetailsDto?> GetCustomerDetailsAsync(Guid userId)
    {
        var customer = await _customerRepository.GetCustomerByIdAsync(userId);

        if (customer is null)
            return null;

        var invoices = customer.UserSaleInvoices?
            .Where(usi => usi.SaleInvoice != null)
            .Select(usi => usi.SaleInvoice!)
            .ToList() ?? new List<SaleInvoice>();

        return new CustomerDetailsDto
        {
            CustomerId = customer.Id,
            FullName = customer.FullName,
            Email = customer.Email ?? string.Empty,
            PhoneNumber = customer.PhoneNumber ?? string.Empty,
            Address = customer.Address ?? string.Empty,
            TotalSpend = invoices.Sum(i => i.TotalAmount),
            CreditBalance = invoices
                .Where(i => i.PaymentStatus.ToString().Equals("Credit", StringComparison.OrdinalIgnoreCase))
                .Sum(i => i.TotalAmount),
            LoyaltyStatus = invoices.Sum(i => i.TotalAmount) >= 50000 ? "Loyal" : "Regular"
        };
    }

    public async Task<CustomerHistoryDto?> GetCustomerHistoryAsync(Guid userId)
    {
        var customer = await _customerRepository.GetCustomerByIdAsync(userId);

        if (customer is null)
            return null;

        var purchaseHistory = customer.UserSaleInvoices?
            .Where(usi => usi.SaleInvoice != null)
            .Select(usi => usi.SaleInvoice!)
            .OrderByDescending(si => si.SaleDate)
            .Select(si => new SaleInvoiceDto
            {
                SaleInvoiceId = si.SaleInvoiceId,
                SaleDate = si.SaleDate,
                TotalAmount = si.TotalAmount,
                PaymentStatus = si.PaymentStatus.ToString(),
                Items = si.SaleInvoiceItems?
                    .Where(sii => sii.SaleItem != null)
                    .Select(sii => new SaleItemDto
                    {
                        PartName = sii.SaleItem!.Part?.PartName ?? string.Empty,
                        Quantity = sii.SaleItem.SaleQuantity,
                        UnitPrice = sii.SaleItem.SaleUnitPrice
                    })
                    .ToList() ?? new List<SaleItemDto>()
            })
            .ToList() ?? new List<SaleInvoiceDto>();

        return new CustomerHistoryDto
        {
            CustomerId = customer.Id,
            PurchaseHistory = purchaseHistory,
            ServiceHistory = new List<AppointmentDto>()
        };
    }

    public async Task<CustomerVehiclesDto?> GetCustomerVehiclesAsync(Guid userId)
    {
        var customer = await _customerRepository.GetCustomerByIdAsync(userId);

        if (customer is null)
            return null;

        var vehicles = customer.UserVehicles?
            .Where(uv => uv.Vehicle != null)
            .Select(uv => new VehicleDto
            {
                VehicleId = uv.Vehicle!.VehicleId,
                VehicleNumber = uv.Vehicle.VehicleNumber,
                Make = uv.Vehicle.Make,
                Model = uv.Vehicle.Model,
                Year = uv.Vehicle.Year
            })
            .ToList() ?? new List<VehicleDto>();

        return new CustomerVehiclesDto
        {
            CustomerId = customer.Id,
            Vehicles = vehicles
        };
    }

    public async Task<ApiResponse<IEnumerable<CustomerSearchResponseDto>>> SearchCustomersAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return ApiResponse<IEnumerable<CustomerSearchResponseDto>>.Fail("Search query cannot be empty.");

        var customers = await _customerRepository.SearchCustomersAsync(query.Trim());

        var dtos = customers.Select(u => new CustomerSearchResponseDto
        {
            UserId = u.Id,
            FullName = u.FullName,
            Email = u.Email ?? string.Empty,
            PhoneNumber = u.PhoneNumber,
            Address = u.Address,
            Vehicles = u.UserVehicles
                .Where(uv => uv.Vehicle != null)
                .Select(uv => new CustomerSearchVehicleDto
                {
                    VehicleId = uv.Vehicle!.VehicleId,
                    VehicleNumber = uv.Vehicle.VehicleNumber,
                    Make = uv.Vehicle.Make,
                    Model = uv.Vehicle.Model,
                    Year = uv.Vehicle.Year
                }).ToList()
        }).ToList();

        return ApiResponse<IEnumerable<CustomerSearchResponseDto>>.Success(dtos);
    }
}
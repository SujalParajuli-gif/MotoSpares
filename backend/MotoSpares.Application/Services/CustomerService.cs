using MotoSpares.Application.DTOs.Customer;

using MotoSpares.Application.Interfaces;
using MotoSpares.Application.Interfaces.Repositories;

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
        var users = await _repository.GetAllCustomersAsync();
        
        return users.Select(u => new CustomerListDto
        {
            Id = u.Id,
            FullName = u.FullName,
            Email = u.Email,
            PhoneNumber = u.PhoneNumber ?? "",
            Address = u.Address ?? "",
            CreatedAt = u.CreatedAt,
            TotalVehicles = u.UserVehicles?.Count ?? 0,
            TotalInvoices = u.UserSaleInvoices?.Count ?? 0
        }).ToList();
    }

    public async Task<CustomerDetailsDto?> GetCustomerDetailsAsync(Guid userId)
    {
        var user = await _repository.GetCustomerByIdAsync(userId);
        if (user == null) return null;

        var invoices = user.UserSaleInvoices?.Select(x => x.SaleInvoice).Where(x => x != null).ToList() ?? new();

        var totalSpend = invoices.Where(i => i!.PaymentStatus.ToString() == "Paid").Sum(i => i!.TotalAmount);
        var creditBalance = invoices.Where(i => i!.PaymentStatus.ToString() != "Paid").Sum(i => i!.TotalAmount);
        
        string loyaltyStatus = "Regular";
        if (totalSpend > 5000)
            loyaltyStatus = "Gold";
        else if (totalSpend > 2000)
            loyaltyStatus = "Silver";

        return new CustomerDetailsDto
        {
            CustomerId = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber ?? "",
            Address = user.Address ?? "",
            TotalSpend = totalSpend,
            CreditBalance = creditBalance,
            LoyaltyStatus = loyaltyStatus
        };
    }

    public async Task<CustomerHistoryDto?> GetCustomerHistoryAsync(Guid userId)
    {
        var user = await _repository.GetCustomerByIdAsync(userId);
        if (user == null) return null;

        var historyDto = new CustomerHistoryDto { CustomerId = user.Id };

        if (user.UserSaleInvoices != null)
        {
            historyDto.PurchaseHistory = user.UserSaleInvoices
                .Select(usi => usi.SaleInvoice)
                .Where(si => si != null)
                .OrderByDescending(si => si!.SaleDate)
                .Select(si => new SaleInvoiceDto
                {
                    SaleInvoiceId = si!.SaleInvoiceId,
                    SaleDate = si.SaleDate,
                    TotalAmount = si.TotalAmount,
                    PaymentStatus = si.PaymentStatus.ToString(),
                    Items = si.SaleInvoiceItems?.Select(sii => new SaleItemDto
                    {
                        PartName = sii.SaleItem?.Part?.PartName ?? "Unknown Part",
                        Quantity = sii.SaleItem?.SaleQuantity ?? 0,
                        UnitPrice = sii.SaleItem?.SaleUnitPrice ?? 0
                    }).ToList() ?? new()
                }).ToList();
        }

        if (user.UserAppointments != null)
        {
            historyDto.ServiceHistory = user.UserAppointments
                .Select(ua => ua.Appointment)
                .Where(a => a != null)
                .OrderByDescending(a => a!.AppointmentDate)
                .Select(a => new AppointmentDto
                {
                    AppointmentId = a!.AppointmentId,
                    AppointmentDate = a.AppointmentDate,
                    ServiceType = a.ServiceType,
                    AppointmentStatus = a.AppointmentStatus.ToString()
                }).ToList();
        }

        return historyDto;
    }

    public async Task<CustomerVehiclesDto?> GetCustomerVehiclesAsync(Guid userId)
    {
        var user = await _repository.GetCustomerByIdAsync(userId);
        if (user == null) return null;

        var vehiclesDto = new CustomerVehiclesDto { CustomerId = user.Id };

        if (user.UserVehicles != null)
        {
            vehiclesDto.Vehicles = user.UserVehicles
                .Select(uv => uv.Vehicle)
                .Where(v => v != null)
                .Select(v => new VehicleDto
                {
                    VehicleId = v!.VehicleId,
                    VehicleNumber = v.VehicleNumber,
                    Make = v.Make,
                    Model = v.Model,
                    Year = v.Year
                }).ToList();
        }

        return vehiclesDto;
    }
}
using MotoSpares.Application.DTOs;
using MotoSpares.Application.DTOs.History;
using MotoSpares.Application.Interfaces;
using MotoSpares.Application.Interfaces.Repositories;

namespace MotoSpares.Application.Services;

public class HistoryService : IHistoryService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IAppointmentRepository _appointmentRepository;

    public HistoryService(
        ICustomerRepository customerRepository,
        IAppointmentRepository appointmentRepository)
    {
        _customerRepository = customerRepository;
        _appointmentRepository = appointmentRepository;
    }

    public async Task<ApiResponse<FullHistoryDto>> GetFullHistoryAsync(Guid userId)
    {
        var customer = await _customerRepository.GetCustomerByIdAsync(userId);
        if (customer == null)
            return ApiResponse<FullHistoryDto>.Fail("Customer not found.");

        var purchases = BuildPurchaseHistory(customer);
        var services = await BuildServiceHistory(userId);

        var dto = new FullHistoryDto
        {
            CustomerId = customer.Id,
            CustomerName = customer.FullName,
            Purchases = purchases,
            Services = services
        };

        return ApiResponse<FullHistoryDto>.Success(dto);
    }

    public async Task<ApiResponse<List<PurchaseHistoryItemDto>>> GetPurchaseHistoryAsync(Guid userId)
    {
        var customer = await _customerRepository.GetCustomerByIdAsync(userId);
        if (customer == null)
            return ApiResponse<List<PurchaseHistoryItemDto>>.Fail("Customer not found.");

        var purchases = BuildPurchaseHistory(customer);
        return ApiResponse<List<PurchaseHistoryItemDto>>.Success(purchases);
    }

    public async Task<ApiResponse<List<ServiceHistoryItemDto>>> GetServiceHistoryAsync(Guid userId)
    {
        var services = await BuildServiceHistory(userId);
        return ApiResponse<List<ServiceHistoryItemDto>>.Success(services);
    }

    private static List<PurchaseHistoryItemDto> BuildPurchaseHistory(Domain.Entities.ApplicationUser customer)
    {
        return customer.UserSaleInvoices?
            .Where(usi => usi.SaleInvoice != null)
            .Select(usi => usi.SaleInvoice!)
            .OrderByDescending(si => si.SaleDate)
            .Select(si => new PurchaseHistoryItemDto
            {
                SaleInvoiceId = si.SaleInvoiceId,
                SaleDate = si.SaleDate,
                Subtotal = si.Subtotal,
                DiscountAmount = si.DiscountAmount,
                TotalAmount = si.TotalAmount,
                PaymentStatus = si.PaymentStatus.ToString(),
                Items = si.SaleInvoiceItems?
                    .Where(sii => sii.SaleItem?.Part != null)
                    .Select(sii => new PurchaseLineItemDto
                    {
                        PartName = sii.SaleItem!.Part!.PartName,
                        PartNumber = sii.SaleItem.Part.PartNumber,
                        Quantity = sii.SaleItem.SaleQuantity,
                        UnitPrice = sii.SaleItem.SaleUnitPrice,
                        LineTotal = sii.SaleItem.SaleQuantity * sii.SaleItem.SaleUnitPrice
                    }).ToList() ?? new List<PurchaseLineItemDto>()
            }).ToList() ?? new List<PurchaseHistoryItemDto>();
    }

    private async Task<List<ServiceHistoryItemDto>> BuildServiceHistory(Guid userId)
    {
        var appointments = await _appointmentRepository.GetByUserIdAsync(userId);

        return appointments.Select(a => new ServiceHistoryItemDto
        {
            AppointmentId = a.AppointmentId,
            AppointmentDate = a.AppointmentDate,
            ServiceType = a.ServiceType,
            Status = a.AppointmentStatus.ToString(),
            Notes = a.AppointmentNotes
        }).ToList();
    }
}

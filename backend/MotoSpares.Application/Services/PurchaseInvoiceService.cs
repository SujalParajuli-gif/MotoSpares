using MotoSpares.Application.DTOs;
using MotoSpares.Application.DTOs.Purchase;
using MotoSpares.Application.Interfaces;
using MotoSpares.Application.Interfaces.Repositories;
using MotoSpares.Domain.Entities;

namespace MotoSpares.Application.Services;

public class PurchaseInvoiceService : IPurchaseInvoiceService
{
    private readonly IPurchaseInvoiceRepository _invoiceRepository;
    private readonly IPartRepository _partRepository;

    public PurchaseInvoiceService(IPurchaseInvoiceRepository invoiceRepository, IPartRepository partRepository)
    {
        _invoiceRepository = invoiceRepository;
        _partRepository = partRepository;
    }

    public async Task<ApiResponse<PurchaseInvoiceResponseDto>> CreateAsync(CreatePurchaseInvoiceDto dto, Guid createdBy)
    {
        var invoice = new PurchaseInvoice
        {
            PurchaseDate = DateTime.UtcNow,
            VendorId = dto.VendorId,
            CreatedBy = createdBy,
            PurchaseTotal = 0
        };

        foreach (var itemDto in dto.Items)
        {
            var part = await _partRepository.GetByIdAsync(itemDto.PartId);
            if (part == null)
                return ApiResponse<PurchaseInvoiceResponseDto>.Fail($"Part with ID {itemDto.PartId} not found or inactive.");

            // Update part stock
            part.StockQuantity += itemDto.PurchaseQuantity;
            await _partRepository.UpdateAsync(part);

            var purchaseItem = new PurchaseItem
            {
                PartId = itemDto.PartId,
                PurchaseQuantity = itemDto.PurchaseQuantity,
                PurchaseUnitCost = itemDto.PurchaseUnitCost
            };

            var invoiceItem = new PurchaseInvoiceItem
            {
                PurchaseInvoice = invoice,
                PurchaseItem = purchaseItem
            };

            invoice.PurchaseInvoiceItems.Add(invoiceItem);
            invoice.PurchaseTotal += itemDto.PurchaseQuantity * itemDto.PurchaseUnitCost;
        }

        await _invoiceRepository.AddAsync(invoice);

        // Fetch again to get fully populated relations like Vendor name
        var createdInvoice = await _invoiceRepository.GetByIdAsync(invoice.PurchaseInvoiceId);
        return ApiResponse<PurchaseInvoiceResponseDto>.Success(MapToDto(createdInvoice!), "Purchase invoice created successfully.");
    }

    public async Task<ApiResponse<PurchaseInvoiceResponseDto>> GetByIdAsync(int id)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(id);
        if (invoice == null)
            return ApiResponse<PurchaseInvoiceResponseDto>.Fail("Purchase invoice not found.");

        return ApiResponse<PurchaseInvoiceResponseDto>.Success(MapToDto(invoice));
    }

    public async Task<ApiResponse<IEnumerable<PurchaseInvoiceResponseDto>>> GetAllAsync(int page = 1, int pageSize = 10)
    {
        var invoices = await _invoiceRepository.GetAllAsync(page, pageSize);
        var dtos = invoices.Select(MapToDto).ToList();
        return ApiResponse<IEnumerable<PurchaseInvoiceResponseDto>>.Success(dtos);
    }

    private static PurchaseInvoiceResponseDto MapToDto(PurchaseInvoice invoice)
    {
        return new PurchaseInvoiceResponseDto
        {
            PurchaseInvoiceId = invoice.PurchaseInvoiceId,
            PurchaseDate = invoice.PurchaseDate,
            PurchaseTotal = invoice.PurchaseTotal,
            VendorId = invoice.VendorId,
            VendorName = invoice.Vendor?.VendorName ?? "Unknown",
            CreatedBy = invoice.CreatedBy,
            Items = invoice.PurchaseInvoiceItems.Select(pi => new PurchaseItemResponseDto
            {
                PurchaseItemId = pi.PurchaseItemId,
                PartId = pi.PurchaseItem!.PartId,
                PartName = pi.PurchaseItem.Part?.PartName ?? "Unknown",
                PurchaseQuantity = pi.PurchaseItem.PurchaseQuantity,
                PurchaseUnitCost = pi.PurchaseItem.PurchaseUnitCost
            }).ToList()
        };
    }
}

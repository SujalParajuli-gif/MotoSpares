using MotoSpares.Application.DTOs;
using MotoSpares.Application.DTOs.Purchase;

namespace MotoSpares.Application.Interfaces;

public interface IPurchaseInvoiceService
{
    Task<ApiResponse<PurchaseInvoiceResponseDto>> CreateAsync(CreatePurchaseInvoiceDto dto, Guid createdBy);
    Task<ApiResponse<PurchaseInvoiceResponseDto>> GetByIdAsync(int id);
    Task<ApiResponse<IEnumerable<PurchaseInvoiceResponseDto>>> GetAllAsync(int page = 1, int pageSize = 10);
}

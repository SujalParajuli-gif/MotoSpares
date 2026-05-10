using MotoSpares.Application.DTOs;
using MotoSpares.Application.DTOs.Loyalty;
using MotoSpares.Application.Interfaces;
using MotoSpares.Application.Interfaces.Repositories;

namespace MotoSpares.Application.Services;

public class LoyaltyService : ILoyaltyService
{
    private readonly ICustomerRepository _customerRepository;

    // Loyalty threshold: customers who have spent 50,000 or more are "Loyal"
    private const decimal LoyaltyThreshold = 50_000m;
    // Loyal customers get a 10% discount on future purchases
    private const int LoyaltyDiscountPercent = 10;

    public LoyaltyService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<ApiResponse<LoyaltyStatusDto>> GetLoyaltyStatusAsync(Guid customerId)
    {
        var customer = await _customerRepository.GetCustomerByIdAsync(customerId);
        if (customer == null)
            return ApiResponse<LoyaltyStatusDto>.Fail("Customer not found.");

        var totalSpend = customer.UserSaleInvoices?
            .Where(usi => usi.SaleInvoice != null)
            .Sum(usi => usi.SaleInvoice!.TotalAmount) ?? 0m;

        var isLoyal = totalSpend >= LoyaltyThreshold;

        var dto = new LoyaltyStatusDto
        {
            CustomerId = customer.Id,
            CustomerName = customer.FullName,
            TotalSpend = totalSpend,
            IsLoyal = isLoyal,
            DiscountPercent = isLoyal ? LoyaltyDiscountPercent : 0,
            Tier = isLoyal ? "Loyal" : "Regular"
        };

        return ApiResponse<LoyaltyStatusDto>.Success(dto);
    }

    public async Task<ApiResponse<DiscountResultDto>> CalculateDiscountAsync(CalculateDiscountDto dto)
    {
        var customer = await _customerRepository.GetCustomerByIdAsync(dto.CustomerId);
        if (customer == null)
            return ApiResponse<DiscountResultDto>.Fail("Customer not found.");

        var totalSpend = customer.UserSaleInvoices?
            .Where(usi => usi.SaleInvoice != null)
            .Sum(usi => usi.SaleInvoice!.TotalAmount) ?? 0m;

        var isLoyal = totalSpend >= LoyaltyThreshold;
        var discountPercent = isLoyal ? LoyaltyDiscountPercent : 0;
        var discountAmount = isLoyal ? Math.Round(dto.Subtotal * LoyaltyDiscountPercent / 100m, 2) : 0m;
        var finalTotal = dto.Subtotal - discountAmount;

        var result = new DiscountResultDto
        {
            CustomerId = customer.Id,
            CustomerName = customer.FullName,
            IsLoyal = isLoyal,
            Tier = isLoyal ? "Loyal" : "Regular",
            Subtotal = dto.Subtotal,
            DiscountPercent = discountPercent,
            DiscountAmount = discountAmount,
            FinalTotal = finalTotal
        };

        return ApiResponse<DiscountResultDto>.Success(result,
            isLoyal
                ? $"10% loyalty discount applied. Customer saved Rs.{discountAmount:F2}!"
                : "No loyalty discount. Customer has not reached the loyalty threshold yet.");
    }
}

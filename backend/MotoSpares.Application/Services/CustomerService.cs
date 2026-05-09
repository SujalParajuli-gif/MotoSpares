using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MotoSpares.Application.DTOs;
using MotoSpares.Application.DTOs.Auth;
using MotoSpares.Application.DTOs.Customer;
using MotoSpares.Application.Interfaces;
using MotoSpares.Application.Interfaces.Repositories;
using MotoSpares.Domain.Entities;

namespace MotoSpares.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly ILogger<CustomerService> _logger;

    public CustomerService(
        ICustomerRepository customerRepository,
        UserManager<ApplicationUser> userManager,
        IConfiguration configuration,
        ILogger<CustomerService> logger)
    {
        _customerRepository = customerRepository;
        _userManager = userManager;
        _configuration = configuration;
        _logger = logger;
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

    public async Task<ApiResponse<AuthResponseDto>> RegisterCustomerWithVehicleAsync(RegisterCustomerWithVehicleDto dto)
    {
        var existingUser = await _userManager.FindByEmailAsync(dto.Email);
        if (existingUser != null)
            return ApiResponse<AuthResponseDto>.Fail("A user with this email already exists.");

        var user = new ApplicationUser
        {
            FullName = dto.FullName.Trim(),
            Email = dto.Email.Trim().ToLower(),
            UserName = dto.Email.Trim().ToLower(),
            PhoneNumber = dto.Phone?.Trim(),
            Address = dto.Address?.Trim(),
            Role = "Customer",
            CreatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            _logger.LogWarning("Customer registration failed for {Email}: {Errors}", dto.Email, errors);
            return ApiResponse<AuthResponseDto>.Fail(errors);
        }

        await _userManager.AddToRoleAsync(user, "Customer");
        _logger.LogInformation("Customer registered by Staff: {Email}", user.Email);

        // Add Vehicle
        var vehicle = new Vehicle
        {
            VehicleNumber = dto.VehicleNumber.Trim(),
            Make = dto.Make.Trim(),
            Model = dto.Model.Trim(),
            Year = dto.Year
        };

        var userVehicle = new UserVehicle
        {
            UserId = user.Id,
            Vehicle = vehicle
        };

        await _customerRepository.AddVehicleForUserAsync(userVehicle);
        _logger.LogInformation("Vehicle {VehicleNumber} added for Customer {Email}", vehicle.VehicleNumber, user.Email);

        // Generate token
        var token = GenerateJwtToken(user);

        var response = new AuthResponseDto
        {
            UserId = user.Id.ToString(),
            FullName = user.FullName,
            Email = user.Email!,
            Role = user.Role,
            Token = token
        };

        return ApiResponse<AuthResponseDto>.Success(response, "Customer and Vehicle registered successfully.");
    }

    private string GenerateJwtToken(ApplicationUser user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpirationMinutes"]!)),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

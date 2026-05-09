using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
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
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ICustomerRepository _customerRepository;
    private readonly IConfiguration _configuration;
    private readonly ILogger<CustomerService> _logger;

    public CustomerService(
        UserManager<ApplicationUser> userManager,
        ICustomerRepository customerRepository,
        IConfiguration configuration,
        ILogger<CustomerService> logger)
    {
        _userManager = userManager;
        _customerRepository = customerRepository;
        _configuration = configuration;
        _logger = logger;
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

        // Generate token just to match the signature, though Staff won't use it to login as the customer
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

        var claims = new System.Collections.Generic.List<Claim>
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

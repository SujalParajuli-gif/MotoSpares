using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MotoSpares.Application.DTOs;
using MotoSpares.Application.DTOs.Auth;
using MotoSpares.Application.Interfaces;
using MotoSpares.Domain.Entities;

namespace MotoSpares.Application.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IConfiguration configuration,
        ILogger<AuthService> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<ApiResponse<AuthResponseDto>> RegisterAsync(RegisterDto dto)
    {
        // Check if user already exists
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
            _logger.LogWarning("Registration failed for {Email}: {Errors}", dto.Email, errors);
            return ApiResponse<AuthResponseDto>.Fail(errors);
        }

        await _userManager.AddToRoleAsync(user, "Customer");
        _logger.LogInformation("Customer registered: {Email}", user.Email);

        var token = GenerateJwtToken(user);

        var response = new AuthResponseDto
        {
            UserId = user.Id.ToString(),
            FullName = user.FullName,
            Email = user.Email!,
            Role = user.Role,
            Token = token
        };

        return ApiResponse<AuthResponseDto>.Success(response, "Registration successful.");
    }

    public async Task<ApiResponse<AuthResponseDto>> RegisterStaffAsync(RegisterStaffDto dto)
    {
        // Check if user already exists
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
            Role = "Staff",
            CreatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            _logger.LogWarning("Staff registration failed for {Email}: {Errors}", dto.Email, errors);
            return ApiResponse<AuthResponseDto>.Fail(errors);
        }

        await _userManager.AddToRoleAsync(user, "Staff");
        _logger.LogInformation("Staff registered by admin: {Email}", user.Email);

        var response = new AuthResponseDto
        {
            UserId = user.Id.ToString(),
            FullName = user.FullName,
            Email = user.Email!,
            Role = user.Role,
            Token = string.Empty
        };

        return ApiResponse<AuthResponseDto>.Success(response, "Staff registered successfully.");
    }

    public async Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null)
            return ApiResponse<AuthResponseDto>.Fail("Invalid email or password.");

        var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, lockoutOnFailure: false);
        if (!result.Succeeded)
        {
            _logger.LogWarning("Failed login attempt for {Email}", dto.Email);
            return ApiResponse<AuthResponseDto>.Fail("Invalid email or password.");
        }

        _logger.LogInformation("User logged in: {Email}", user.Email);

        var token = GenerateJwtToken(user);

        var response = new AuthResponseDto
        {
            UserId = user.Id.ToString(),
            FullName = user.FullName,
            Email = user.Email!,
            Role = user.Role,
            Token = token
        };

        return ApiResponse<AuthResponseDto>.Success(response, "Login successful.");
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

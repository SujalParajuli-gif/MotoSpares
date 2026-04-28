using MotoSpares.Application.DTOs.Auth;
using MotoSpares.Application.Interfaces.Repositories;
using MotoSpares.Domain.Entities;
using MotoSpares.Domain.Enums;

namespace MotoSpares.Application.Services;

public class AuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly JwtService _jwtService;

    public AuthService(
        IUserRepository userRepository,
        ICustomerRepository customerRepository,
        JwtService jwtService)
    {
        _userRepository = userRepository;
        _customerRepository = customerRepository;
        _jwtService = jwtService;
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
    {
        var user = await _userRepository.GetByEmailAsync(dto.Email);

        if (user == null)
            return null;

        var isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);

        if (!isPasswordValid)
            return null;

        var token = _jwtService.GenerateToken(user);

        return new AuthResponseDto
        {
            Token = token,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role.ToString()
        };
    }

    public async Task<AuthResponseDto?> RegisterCustomerAsync(RegisterCustomerDto dto)
    {
        var existingUser = await _userRepository.GetByEmailAsync(dto.Email);

        if (existingUser != null)
            return null;

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        var user = new User(
            dto.FullName,
            dto.Email,
            passwordHash,
            UserRole.Customer,
            dto.Phone
        );

        await _userRepository.AddAsync(user);

        var customer = new Customer(
            dto.FullName,
            dto.Phone,
            dto.Email,
            dto.Address
        );

        await _customerRepository.AddAsync(customer);

        var token = _jwtService.GenerateToken(user);

        return new AuthResponseDto
        {
            Token = token,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role.ToString()
        };
    }
}
using MotoSpares.Application.DTOs.Staff;
using MotoSpares.Application.Interfaces.Repositories;
using MotoSpares.Domain.Entities;
using MotoSpares.Domain.Enums;

namespace MotoSpares.Application.Services;

public class StaffService
{
    private readonly IStaffRepository _staffRepository;
    private readonly IUserRepository _userRepository;

    public StaffService(IStaffRepository staffRepository, IUserRepository userRepository)
    {
        _staffRepository = staffRepository;
        _userRepository = userRepository;
    }

    public async Task<StaffResponseDto> CreateStaffAsync(CreateStaffDto dto)
    {
        // Hash the password
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        // Create user first
        var user = new User(dto.FullName, dto.Email, passwordHash, UserRole.Staff, dto.Phone);
        await _userRepository.AddAsync(user);

        // Create staff record
        var staff = new Staff(user.Id, dto.EmployeeCode, dto.Department);
        await _staffRepository.AddAsync(staff);

        return MapToResponseDto(staff, user);
    }

    public async Task<StaffResponseDto?> GetStaffByIdAsync(Guid id)
    {
        var staff = await _staffRepository.GetByIdAsync(id);
        if (staff == null) return null;

        return MapToResponseDto(staff, staff.User!);
    }

    public async Task<IEnumerable<StaffResponseDto>> GetAllStaffAsync()
    {
        var staffList = await _staffRepository.GetAllAsync();
        return staffList.Select(s => MapToResponseDto(s, s.User!));
    }

    public async Task<StaffResponseDto?> UpdateStaffAsync(Guid id, UpdateStaffDto dto)
    {
        var staff = await _staffRepository.GetByIdAsync(id);
        if (staff == null) return null;

        var user = await _userRepository.GetByIdAsync(staff.UserId);
        if (user == null) return null;

        user.UpdateDetails(dto.FullName, dto.Phone);
        staff.UpdateDetails(dto.EmployeeCode, dto.Department);

        await _userRepository.UpdateAsync(user);
        await _staffRepository.UpdateAsync(staff);

        return MapToResponseDto(staff, user);
    }

    public async Task<bool> DeleteStaffAsync(Guid id)
    {
        var staff = await _staffRepository.GetByIdAsync(id);
        if (staff == null) return false;

        await _staffRepository.DeleteAsync(id);
        await _userRepository.DeleteAsync(staff.UserId);

        return true;
    }

    private StaffResponseDto MapToResponseDto(Staff staff, User user)
    {
        return new StaffResponseDto
        {
            Id = staff.Id,
            UserId = staff.UserId,
            FullName = user.FullName,
            Email = user.Email,
            Phone = user.Phone,
            EmployeeCode = staff.EmployeeCode,
            Department = staff.Department,
            JoinedAt = staff.JoinedAt
        };
    }
}
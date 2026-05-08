using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MotoSpares.Application.DTOs;
using MotoSpares.Application.DTOs.Staff;
using MotoSpares.Application.Interfaces;
using MotoSpares.Domain.Entities;

namespace MotoSpares.Application.Services;

public class StaffService : IStaffService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<StaffService> _logger;

    public StaffService(UserManager<ApplicationUser> userManager, ILogger<StaffService> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<ApiResponse<IEnumerable<StaffDto>>> GetAllStaffAsync()
    {
        var staffUsers = await _userManager.GetUsersInRoleAsync("Staff");

        var staffDtos = staffUsers.Select(u => new StaffDto
        {
            Id = u.Id.ToString(),
            FullName = u.FullName,
            Email = u.Email!,
            Phone = u.PhoneNumber,
            Address = u.Address,
            CreatedAt = u.CreatedAt
        }).OrderByDescending(s => s.CreatedAt).ToList();

        return ApiResponse<IEnumerable<StaffDto>>.Success(staffDtos);
    }

    public async Task<ApiResponse<StaffDto>> GetStaffByIdAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user == null || !await _userManager.IsInRoleAsync(user, "Staff"))
        {
            return ApiResponse<StaffDto>.Fail("Staff member not found.");
        }

        var staffDto = new StaffDto
        {
            Id = user.Id.ToString(),
            FullName = user.FullName,
            Email = user.Email!,
            Phone = user.PhoneNumber,
            Address = user.Address,
            CreatedAt = user.CreatedAt
        };

        return ApiResponse<StaffDto>.Success(staffDto);
    }

    public async Task<ApiResponse<StaffDto>> UpdateStaffAsync(string id, UpdateStaffDto dto)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user == null || !await _userManager.IsInRoleAsync(user, "Staff"))
        {
            return ApiResponse<StaffDto>.Fail("Staff member not found.");
        }

        user.FullName = dto.FullName.Trim();
        user.PhoneNumber = dto.Phone?.Trim();
        user.Address = dto.Address?.Trim();

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            _logger.LogWarning("Failed to update staff {Id}: {Errors}", id, errors);
            return ApiResponse<StaffDto>.Fail(errors);
        }

        var staffDto = new StaffDto
        {
            Id = user.Id.ToString(),
            FullName = user.FullName,
            Email = user.Email!,
            Phone = user.PhoneNumber,
            Address = user.Address,
            CreatedAt = user.CreatedAt
        };

        return ApiResponse<StaffDto>.Success(staffDto, "Staff updated successfully.");
    }

    public async Task<ApiResponse<bool>> DeleteStaffAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user == null || !await _userManager.IsInRoleAsync(user, "Staff"))
        {
            return ApiResponse<bool>.Fail("Staff member not found.");
        }

        var result = await _userManager.DeleteAsync(user);

        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            _logger.LogWarning("Failed to delete staff {Id}: {Errors}", id, errors);
            return ApiResponse<bool>.Fail(errors);
        }

        return ApiResponse<bool>.Success(true, "Staff removed successfully.");
    }
}

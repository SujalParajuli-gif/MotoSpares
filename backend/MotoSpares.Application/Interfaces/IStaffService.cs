using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MotoSpares.Application.DTOs;
using MotoSpares.Application.DTOs.Staff;

namespace MotoSpares.Application.Interfaces;

public interface IStaffService
{
    Task<ApiResponse<IEnumerable<StaffDto>>> GetAllStaffAsync();
    Task<ApiResponse<StaffDto>> GetStaffByIdAsync(string id);
    Task<ApiResponse<StaffDto>> UpdateStaffAsync(string id, UpdateStaffDto dto);
    Task<ApiResponse<bool>> DeleteStaffAsync(string id);
}

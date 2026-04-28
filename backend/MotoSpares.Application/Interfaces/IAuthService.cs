using MotoSpares.Application.DTOs;
using MotoSpares.Application.DTOs.Auth;

namespace MotoSpares.Application.Interfaces;

public interface IAuthService
{
    Task<ApiResponse<AuthResponseDto>> RegisterAsync(RegisterDto dto);
    Task<ApiResponse<AuthResponseDto>> RegisterStaffAsync(RegisterStaffDto dto);
    Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginDto dto);
}

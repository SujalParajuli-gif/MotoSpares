using MotoSpares.Application.DTOs;
using MotoSpares.Application.DTOs.PartRequest;

namespace MotoSpares.Application.Interfaces;

public interface IPartRequestService
{
    Task<ApiResponse<PartRequestResponseDto>> CreatePartRequestAsync(CreatePartRequestDto dto, Guid userId);
    Task<ApiResponse<List<PartRequestResponseDto>>> GetMyPartRequestsAsync(Guid userId);
    Task<ApiResponse<List<PartRequestResponseDto>>> GetAllPartRequestsAsync();
    Task<ApiResponse<PartRequestResponseDto>> UpdatePartRequestStatusAsync(int requestId, UpdatePartRequestStatusDto dto);
}

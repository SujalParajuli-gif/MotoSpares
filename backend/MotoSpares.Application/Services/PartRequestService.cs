using MotoSpares.Application.DTOs;
using MotoSpares.Application.DTOs.PartRequest;
using MotoSpares.Application.Interfaces;
using MotoSpares.Application.Interfaces.Repositories;
using MotoSpares.Domain.Entities;
using MotoSpares.Domain.Enums;

namespace MotoSpares.Application.Services;

public class PartRequestService : IPartRequestService
{
    private readonly IPartRequestRepository _partRequestRepository;

    public PartRequestService(IPartRequestRepository partRequestRepository)
    {
        _partRequestRepository = partRequestRepository;
    }

    public async Task<ApiResponse<PartRequestResponseDto>> CreatePartRequestAsync(CreatePartRequestDto dto, Guid userId)
    {
        var partRequest = new PartRequest
        {
            RequestedPartName = dto.RequestedPartName.Trim(),
            RequestDescription = dto.Description?.Trim(),
            RequestDate = DateTime.UtcNow,
            RequestStatus = PartRequestStatus.Pending
        };

        _partRequestRepository.Create(partRequest);
        await _partRequestRepository.SaveChangesAsync();

        // Link the request to the customer via the join table
        var userPartRequest = new UserPartRequest
        {
            UserId = userId,
            RequestId = partRequest.RequestId
        };
        await _partRequestRepository.AddUserPartRequestAsync(userPartRequest);

        // Reload with user info
        var created = await _partRequestRepository.GetByIdWithUsersAsync(partRequest.RequestId);

        return ApiResponse<PartRequestResponseDto>.Success(MapToDto(created!), "Part request submitted successfully.");
    }

    public async Task<ApiResponse<List<PartRequestResponseDto>>> GetMyPartRequestsAsync(Guid userId)
    {
        var requests = await _partRequestRepository.GetByUserIdAsync(userId);
        var dtos = requests.Select(MapToDto).ToList();
        return ApiResponse<List<PartRequestResponseDto>>.Success(dtos);
    }

    public async Task<ApiResponse<List<PartRequestResponseDto>>> GetAllPartRequestsAsync()
    {
        var requests = await _partRequestRepository.GetAllWithUsersAsync();
        var dtos = requests.Select(MapToDto).ToList();
        return ApiResponse<List<PartRequestResponseDto>>.Success(dtos);
    }

    public async Task<ApiResponse<PartRequestResponseDto>> UpdatePartRequestStatusAsync(int requestId, UpdatePartRequestStatusDto dto)
    {
        var partRequest = await _partRequestRepository.GetByIdWithUsersAsync(requestId);

        if (partRequest == null)
            return ApiResponse<PartRequestResponseDto>.Fail("Part request not found.");

        if (!Enum.TryParse<PartRequestStatus>(dto.Status, true, out var newStatus))
            return ApiResponse<PartRequestResponseDto>.Fail($"Invalid status. Valid values: {string.Join(", ", Enum.GetNames<PartRequestStatus>())}");

        partRequest.RequestStatus = newStatus;
        _partRequestRepository.Update(partRequest);
        await _partRequestRepository.SaveChangesAsync();

        return ApiResponse<PartRequestResponseDto>.Success(MapToDto(partRequest), "Part request status updated.");
    }

    private static PartRequestResponseDto MapToDto(PartRequest request)
    {
        var user = request.UserPartRequests?.FirstOrDefault()?.User;
        return new PartRequestResponseDto
        {
            RequestId = request.RequestId,
            RequestedPartName = request.RequestedPartName,
            Description = request.RequestDescription,
            RequestDate = request.RequestDate,
            Status = request.RequestStatus.ToString(),
            CustomerName = user?.FullName ?? string.Empty,
            CustomerId = user?.Id ?? Guid.Empty
        };
    }
}

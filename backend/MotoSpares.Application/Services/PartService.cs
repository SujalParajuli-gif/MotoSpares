using MotoSpares.Application.DTOs;
using MotoSpares.Application.DTOs.Part;
using MotoSpares.Application.Interfaces;
using MotoSpares.Application.Interfaces.Repositories;
using MotoSpares.Domain.Entities;

namespace MotoSpares.Application.Services;

public class PartService : IPartService
{
    private readonly IPartRepository _partRepository;

    public PartService(IPartRepository partRepository)
    {
        _partRepository = partRepository;
    }

    public async Task<ApiResponse<IEnumerable<PartResponseDto>>> GetAllAsync(int page, int pageSize)
    {
        var parts = await _partRepository.GetAllAsync(page, pageSize);
        var dtos = parts.Select(MapToDto).ToList();
        return ApiResponse<IEnumerable<PartResponseDto>>.Success(dtos);
    }

    public async Task<ApiResponse<PartResponseDto>> GetByIdAsync(int id)
    {
        var part = await _partRepository.GetByIdAsync(id);
        if (part == null)
            return ApiResponse<PartResponseDto>.Fail("Part not found.");

        return ApiResponse<PartResponseDto>.Success(MapToDto(part));
    }

    public async Task<ApiResponse<IEnumerable<PartResponseDto>>> GetLowStockAsync()
    {
        var parts = await _partRepository.GetLowStockAsync();
        var dtos = parts.Select(MapToDto).ToList();
        return ApiResponse<IEnumerable<PartResponseDto>>.Success(dtos);
    }

    public async Task<ApiResponse<PartResponseDto>> CreateAsync(PartCreateDto dto)
    {
        if (await _partRepository.ExistsByPartNumberAsync(dto.PartNumber))
            return ApiResponse<PartResponseDto>.Fail("Part number already exists.");

        var part = new Part
        {
            PartName = dto.PartName,
            PartNumber = dto.PartNumber,
            Description = dto.Description,
            Category = dto.Category,
            UnitPrice = dto.UnitPrice,
            StockQuantity = dto.StockQuantity,
            ReorderLevel = dto.ReorderLevel,
            IsActive = true
        };

        await _partRepository.AddAsync(part);
        return ApiResponse<PartResponseDto>.Success(MapToDto(part), "Part created successfully.");
    }

    public async Task<ApiResponse<PartResponseDto>> UpdateAsync(int id, PartUpdateDto dto)
    {
        if (id != dto.PartId)
            return ApiResponse<PartResponseDto>.Fail("ID mismatch.");

        var part = await _partRepository.GetByIdAsync(id);
        if (part == null)
            return ApiResponse<PartResponseDto>.Fail("Part not found.");

        if (await _partRepository.ExistsByPartNumberAsync(dto.PartNumber, id))
            return ApiResponse<PartResponseDto>.Fail("Part number already used by another part.");

        part.PartName = dto.PartName;
        part.PartNumber = dto.PartNumber;
        part.Description = dto.Description;
        part.Category = dto.Category;
        part.UnitPrice = dto.UnitPrice;
        part.StockQuantity = dto.StockQuantity;
        part.ReorderLevel = dto.ReorderLevel;

        await _partRepository.UpdateAsync(part);
        return ApiResponse<PartResponseDto>.Success(MapToDto(part), "Part updated successfully.");
    }

    public async Task<ApiResponse<bool>> DeleteAsync(int id)
    {
        var part = await _partRepository.GetByIdAsync(id);
        if (part == null)
            return ApiResponse<bool>.Fail("Part not found.");

        await _partRepository.DeleteAsync(id);
        return ApiResponse<bool>.Success(true, "Part deleted successfully.");
    }

    private static PartResponseDto MapToDto(Part part)
    {
        return new PartResponseDto
        {
            PartId = part.PartId,
            PartName = part.PartName,
            PartNumber = part.PartNumber,
            Description = part.Description,
            Category = part.Category,
            UnitPrice = part.UnitPrice,
            StockQuantity = part.StockQuantity,
            ReorderLevel = part.ReorderLevel,
            IsActive = part.IsActive
        };
    }
}

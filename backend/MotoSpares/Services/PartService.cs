using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MotoSpares.Common.Responses;
using MotoSpares.DTOs.Part;
using MotoSpares.Entities;
using MotoSpares.Interfaces.Repositories;
using MotoSpares.Interfaces.Services;

namespace MotoSpares.Services
{
    public class PartService : IPartService
    {
        private readonly IRepository<Part> _partRepository;
        private readonly IMapper _mapper;

        public PartService(IRepository<Part> partRepository, IMapper mapper)
        {
            _partRepository = partRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<IEnumerable<PartResponseDto>>> GetAllAsync(int page, int pageSize)
        {
            var query = _partRepository.Query()
                .Include(p => p.Vendor)
                .Where(p => p.IsActive);

            var totalCount = await query.CountAsync();
            var parts = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<PartResponseDto>>(parts);
            return ApiResponse<IEnumerable<PartResponseDto>>.Success(dtos, totalCount);
        }

        public async Task<ApiResponse<PartResponseDto>> GetByIdAsync(int id)
        {
            var part = await _partRepository.Query()
                .Include(p => p.Vendor)
                .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);

            if (part == null)
                return ApiResponse<PartResponseDto>.Failure("Part not found");

            var dto = _mapper.Map<PartResponseDto>(part);
            return ApiResponse<PartResponseDto>.Success(dto);
        }

        public async Task<ApiResponse<PartResponseDto>> CreateAsync(PartCreateDto dto)
        {
            var exists = await _partRepository.Query()
                .AnyAsync(p => p.PartNumber == dto.PartNumber);
            
            if (exists)
                return ApiResponse<PartResponseDto>.Failure("Part number already exists");

            var part = _mapper.Map<Part>(dto);
            part.CreatedAt = DateTime.UtcNow;

            await _partRepository.AddAsync(part);
            await _partRepository.SaveChangesAsync();

            // Reload to get Vendor name
            var createdPart = await _partRepository.Query()
                .Include(p => p.Vendor)
                .FirstOrDefaultAsync(p => p.Id == part.Id);

            var response = _mapper.Map<PartResponseDto>(createdPart);
            return ApiResponse<PartResponseDto>.Success(response, "Part created successfully");
        }

        public async Task<ApiResponse<PartResponseDto>> UpdateAsync(PartUpdateDto dto)
        {
            var part = await _partRepository.GetByIdAsync(dto.Id);
            if (part == null || !part.IsActive)
                return ApiResponse<PartResponseDto>.Failure("Part not found");

            var duplicate = await _partRepository.Query()
                .AnyAsync(p => p.PartNumber == dto.PartNumber && p.Id != dto.Id);
            
            if (duplicate)
                return ApiResponse<PartResponseDto>.Failure("Part number already used by another part");

            _mapper.Map(dto, part);
            await _partRepository.UpdateAsync(part);
            await _partRepository.SaveChangesAsync();

            var updatedPart = await _partRepository.Query()
                .Include(p => p.Vendor)
                .FirstOrDefaultAsync(p => p.Id == part.Id);

            var response = _mapper.Map<PartResponseDto>(updatedPart);
            return ApiResponse<PartResponseDto>.Success(response, "Part updated successfully");
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            var part = await _partRepository.GetByIdAsync(id);
            if (part == null || !part.IsActive)
                return ApiResponse<bool>.Failure("Part not found");

            part.IsActive = false;
            await _partRepository.UpdateAsync(part);
            await _partRepository.SaveChangesAsync();

            return ApiResponse<bool>.Success(true, "Part deleted successfully");
        }

        public async Task<ApiResponse<IEnumerable<PartResponseDto>>> GetLowStockAsync()
        {
            var parts = await _partRepository.Query()
                .Include(p => p.Vendor)
                .Where(p => p.IsActive && p.StockQuantity < p.ReorderLevel)
                .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<PartResponseDto>>(parts);
            return ApiResponse<IEnumerable<PartResponseDto>>.Success(dtos);
        }
    }
}

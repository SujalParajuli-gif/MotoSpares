using Microsoft.EntityFrameworkCore;
using MotoSpares.Application.Interfaces.Repositories;
using MotoSpares.Domain.Entities;
using MotoSpares.Infrastructure.Data;

namespace MotoSpares.Infrastructure.Repositories;

public class PartRepository : IPartRepository
{
    private readonly AppDbContext _context;

    public PartRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Part?> GetByIdAsync(int id)
    {
        return await _context.Parts.FirstOrDefaultAsync(p => p.PartId == id && p.IsActive);
    }

    public async Task<IEnumerable<Part>> GetAllAsync(int page, int pageSize)
    {
        return await _context.Parts
            .Where(p => p.IsActive)
            .OrderBy(p => p.PartName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await _context.Parts.CountAsync(p => p.IsActive);
    }

    public async Task<IEnumerable<Part>> GetLowStockAsync()
    {
        return await _context.Parts
            .Where(p => p.IsActive && p.StockQuantity < p.ReorderLevel)
            .ToListAsync();
    }

    public async Task<bool> ExistsByPartNumberAsync(string partNumber, int? excludeId = null)
    {
        var query = _context.Parts.Where(p => p.PartNumber == partNumber && p.IsActive);
        if (excludeId.HasValue)
        {
            query = query.Where(p => p.PartId != excludeId.Value);
        }
        return await query.AnyAsync();
    }

    public async Task AddAsync(Part part)
    {
        await _context.Parts.AddAsync(part);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Part part)
    {
        _context.Parts.Update(part);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var part = await _context.Parts.FindAsync(id);
        if (part != null)
        {
            part.IsActive = false; // Soft delete
            _context.Parts.Update(part);
            await _context.SaveChangesAsync();
        }
    }
}

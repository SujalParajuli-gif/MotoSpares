using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MotoSpares.Application.DTOs;
using MotoSpares.Application.DTOs.Part;
using MotoSpares.Application.Interfaces;

namespace MotoSpares.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PartsController : ControllerBase
    {
        private readonly IPartService _partService;

        public PartsController(IPartService partService)
        {
            _partService = partService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<PartResponseDto>>>> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _partService.GetAllAsync(page, pageSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<PartResponseDto>>> GetById(int id)
        {
            var result = await _partService.GetByIdAsync(id);
            if (!result.IsSuccess)
                return NotFound(result);
            return Ok(result);
        }

        [HttpGet("low-stock")]
        public async Task<ActionResult<ApiResponse<IEnumerable<PartResponseDto>>>> GetLowStock()
        {
            var result = await _partService.GetLowStockAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<PartResponseDto>>> Create(PartCreateDto dto)
        {
            var result = await _partService.CreateAsync(dto);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<PartResponseDto>>> Update(int id, PartUpdateDto dto)
        {
            var result = await _partService.UpdateAsync(id, dto);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            var result = await _partService.DeleteAsync(id);
            if (!result.IsSuccess)
                return NotFound(result);
            return Ok(result);
        }
    }
}

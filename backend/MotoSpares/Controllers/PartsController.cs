using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MotoSpares.Common.Responses;
using MotoSpares.DTOs.Part;
using MotoSpares.Interfaces.Services;

namespace MotoSpares.Controllers
{
    public class PartsController : BaseApiController
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

        [HttpPut]
        public async Task<ActionResult<ApiResponse<PartResponseDto>>> Update(PartUpdateDto dto)
        {
            var result = await _partService.UpdateAsync(dto);
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

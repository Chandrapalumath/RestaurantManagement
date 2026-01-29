using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Api.Middlewares;
using RestaurantManagement.Backend.Services;
using RestaurantManagement.Backend.Services.Interfaces;
using RestaurantManagement.Dtos.Menu;
using RestaurantManagement.Dtos.Reviews;
using RestaurantManagement.Dtos.Tables;

namespace RestaurantManagement.Api.Controllers
{
    [Route("api/table")]
    [ApiController]
    public class TableController : ControllerBase
    {
        private readonly ITableService _tableService;

        public TableController(ITableService tableService)
        {
            _tableService = tableService;
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(List<TableResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllTablesAsync()
        {
            return Ok(await _tableService.GetAllAsync());
        }

        [Authorize]
        [HttpGet("{id}", Name = "GetTableById")]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(List<TableResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTableByIdAsync(Guid id)
        {
            return Ok(await _tableService.GetByIdAsync(id)); ;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(typeof(void), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddTableAsync(TableCreateRequestDto dto)
        {
            var table = await _tableService.CreateAsync(dto);
            return CreatedAtRoute("GetTableById", new { id = table.Id }, null);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteTableAsync(Guid id)
        {
            await _tableService.DeleteAsync(id);
            return NoContent();
        }
    }
}

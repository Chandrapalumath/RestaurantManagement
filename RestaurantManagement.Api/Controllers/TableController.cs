using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Api.Middlewares;
using RestaurantManagement.Backend.Services;
using RestaurantManagement.Backend.Services.Interfaces;
using RestaurantManagement.Dtos.Menu;
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
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllTablesAsync()
        {
            return Ok(await _tableService.GetAllAsync());
        }
        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTableByIdAsync(Guid id)
        {
            return Ok(await _tableService.GetByIdAsync(id)); ;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddTableAsync(TableCreateRequestDto dto)
        {
            return Ok(await _tableService.CreateAsync(dto));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteTableAsync(Guid id)
        {
            await _tableService.DeleteAsync(id);
            return Ok("Table deleted.");
        }
    }
}

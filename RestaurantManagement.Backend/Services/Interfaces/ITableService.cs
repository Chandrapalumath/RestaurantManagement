using RestaurantManagement.Dtos.Tables;

namespace RestaurantManagement.Backend.Services.Interfaces
{
    public interface ITableService
    {
        Task<TableResponseDto> CreateAsync(TableCreateRequestDto dto);
        Task DeleteAsync(Guid tableId);
        Task<List<TableResponseDto>> GetAllAsync();
        Task<TableResponseDto> GetByIdAsync(Guid tableId);
    }
}

using RestaurantManagement.Backend.Exceptions;
using RestaurantManagement.Backend.Services.Interfaces;
using RestaurantManagement.DataAccess.Models;
using RestaurantManagement.DataAccess.Repositories.Interfaces;
using RestaurantManagement.Dtos.Tables;

namespace RestaurantManagement.Backend.Services
{
    public class TableService : ITableService
    {
        private readonly IGenericRepository<Table> _tableRepo;

        public TableService(IGenericRepository<Table> tableRepo)
        {
            _tableRepo = tableRepo;
        }

        public async Task<TableResponseDto> CreateAsync(TableCreateRequestDto dto)
        {
            var table = new Table
            {
                TableName = dto.TableName.Trim(),
                Size = dto.Size,
                IsOccupied = false
            };

            await _tableRepo.AddAsync(table);
            await _tableRepo.SaveChangesAsync();

            return Map(table);
        }

        public async Task DeleteAsync(Guid tableId)
        {
            var table = await _tableRepo.GetByIdAsync(tableId)
                        ?? throw new NotFoundException("Table not found.");

            if (table.IsOccupied)
                throw new BadRequestException("Cannot delete an occupied table.");

            _tableRepo.Delete(table);
            await _tableRepo.SaveChangesAsync();
        }

        public async Task<List<TableResponseDto>> GetAllAsync()
        {
            var tables = await _tableRepo.GetAllAsync();
            return tables.Select(Map).ToList();
        }

        public async Task<TableResponseDto> GetByIdAsync(Guid tableId)
        {
            var table = await _tableRepo.GetByIdAsync(tableId)
                        ?? throw new NotFoundException("Table not found.");
            return Map(table);
        }

        public async Task<TableResponseDto> UpdateOccupiedAsync(Guid tableId, bool isOccupied)
        {
            var table = await _tableRepo.GetByIdAsync(tableId)
                        ?? throw new NotFoundException("Table not found.");

            table.IsOccupied = isOccupied;
            _tableRepo.Update(table);
            await _tableRepo.SaveChangesAsync();

            return Map(table);
        }

        private static TableResponseDto Map(Table t) => new()
        {
            Id = t.Id,
            TableName = t.TableName,
            Size = t.Size,
            IsOccupied = t.IsOccupied
        };
    }
}
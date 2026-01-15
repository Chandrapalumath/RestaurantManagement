using RestaurantManagement.Backend.Exceptions;
using RestaurantManagement.Backend.Services.Interfaces;
using RestaurantManagement.DataAccess.Models;
using RestaurantManagement.DataAccess.Repositories.Interfaces;
using RestaurantManagement.Dtos.Menu;

namespace RestaurantManagement.Backend.Services
{
    public class MenuService : IMenuService
    {
        private readonly IMenuRepository _menuRepo;

        public MenuService(IMenuRepository menuRepo)
        {
            _menuRepo = menuRepo;
        }

        public async Task<List<MenuItemResponseDto>> GetAllAsync()
        {
            var items = await _menuRepo.GetAllAsync();
            return items.Select(x => new MenuItemResponseDto
            {
                Id = x.Id,
                Name = x.Name,
                Price = x.Price,
                IsAvailable = x.IsAvailable
            }).ToList();
        }

        public async Task<MenuItemResponseDto> GetByIdAsync(int id)
        {
            var item = await _menuRepo.GetByIdAsync(id)
                       ?? throw new NotFoundException("Menu item not found.");

            return new MenuItemResponseDto
            {
                Id = item.Id,
                Name = item.Name,
                Price = item.Price,
                IsAvailable = item.IsAvailable
            };
        }

        public async Task<MenuItemResponseDto> CreateAsync(MenuItemCreateRequestDto dto)
        {
            var menuItem = _menuRepo.GetByNameAsync(dto.Name);
            if(menuItem == null)
            {
                throw new BadRequestException("Item Already Exists");
            }
            var entity = new MenuItem
            {
                Name = dto.Name.Trim(),
                Price = dto.Price,
                IsAvailable = dto.IsAvailable
            };

            await _menuRepo.AddAsync(entity);
            await _menuRepo.SaveChangesAsync();

            return new MenuItemResponseDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Price = entity.Price,
                IsAvailable = entity.IsAvailable
            };
        }

        public async Task<MenuItemResponseDto> UpdateAsync(int id, MenuItemUpdateRequestDto dto)
        {
            var entity = await _menuRepo.GetByIdAsync(id)
                         ?? throw new NotFoundException("Menu item not found.");
            if(dto.Name != null) 
                entity.Name = dto.Name.Trim();
            if(dto.Price.HasValue)
                entity.Price = dto.Price.Value;
            if (dto.IsAvailable.HasValue)
                entity.IsAvailable = dto.IsAvailable.Value;

            _menuRepo.Update(entity);
            await _menuRepo.SaveChangesAsync();

            return await GetByIdAsync(id);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _menuRepo.GetByIdAsync(id)
                         ?? throw new NotFoundException("Menu item not found.");

            _menuRepo.Delete(entity);
            await _menuRepo.SaveChangesAsync();
        }
    }

}

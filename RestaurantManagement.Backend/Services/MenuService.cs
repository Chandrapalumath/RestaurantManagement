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
            if(!items.Any()) throw new NotFoundException("No Items Found");
            return items.Select(x => new MenuItemResponseDto
            {
                Id = x.Id,
                Name = x.Name,
                Price = x.Price,
                IsAvailable = x.IsAvailable,
                Rating = x.Rating.Value
            }).ToList();
        }

        public async Task<MenuItemResponseDto> GetByIdAsync(Guid id)
        {
            var item = await _menuRepo.GetByIdAsync(id)
                       ?? throw new NotFoundException("Menu item not found.");

            return new MenuItemResponseDto
            {
                Id = item.Id,
                Name = item.Name,
                Price = item.Price,
                IsAvailable = item.IsAvailable,
                Rating = item.Rating.Value
            };
        }

        public async Task<MenuItemResponseDto> CreateAsync(MenuItemCreateRequestDto dto)
        {
            var menuItem = await _menuRepo.GetByNameAsync(dto.Name.Trim().ToLower());
            if(menuItem is not null)
            {
                throw new ConflictException("Item Already Exists");
            }
            var entity = new MenuItem
            {
                Id = Guid.NewGuid(),
                Name = dto.Name.Trim().ToLower(),
                Price = dto.Price,
                IsAvailable = dto.IsAvailable,
                Rating = 0,
                TotalReviews = 0
            };

            await _menuRepo.AddAsync(entity);
            await _menuRepo.SaveChangesAsync();

            return new MenuItemResponseDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Price = entity.Price,
                IsAvailable = entity.IsAvailable,
                Rating = entity.Rating ?? 0
            };
        }

        public async Task UpdateAsync(Guid id, MenuItemUpdateRequestDto dto)
        {
            var entity = await _menuRepo.GetByIdAsync(id)
                         ?? throw new NotFoundException("Menu item not found.");
            if(dto.Name != null) 
                entity.Name = dto.Name.Trim();
            if(dto.Price.HasValue)
                entity.Price = dto.Price.Value;
            if (dto.IsAvailable.HasValue)
                entity.IsAvailable = dto.IsAvailable.Value;

            await _menuRepo.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _menuRepo.GetByIdAsync(id) ?? throw new NotFoundException("Menu item not found.");

            _menuRepo.Delete(entity);
            await _menuRepo.SaveChangesAsync();
        }

        public async Task UpdateRatingAsync(List<UpdateMenuItemRating> dto)
        {
            foreach (var details in dto)
            {
                var menuItem = await _menuRepo.GetByIdAsync(details.Id);
                if (menuItem == null) throw new BadRequestException("Item Not Found");
                var oldAvg = menuItem.Rating;
                var oldCount = menuItem.TotalReviews;
                var newCount = oldCount + 1;
                menuItem.Rating = ((oldAvg * oldCount) + details.Rating) / newCount;
                menuItem.TotalReviews = newCount;
            }
            await _menuRepo.SaveChangesAsync();
        }
    }

}

using RestaurantManagement.Backend.Exceptions;
using RestaurantManagement.Backend.Services.Interfaces;
using RestaurantManagement.DataAccess.Models;
using RestaurantManagement.DataAccess.Repositories.Interfaces;
using RestaurantManagement.Dtos.Orders;
using RestaurantManagement.Models.Common.Enums;

namespace RestaurantManagement.Backend.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly ICustomerRepository _customerRepo;
        private readonly IMenuRepository _menuRepo;
        private readonly ITableRepository _tableRepo;
        public OrderService(
            IOrderRepository orderRepo,
            ICustomerRepository customerRepo,
            IMenuRepository menuRepo, ITableRepository tableRepo)
        {
            _orderRepo = orderRepo;
            _customerRepo = customerRepo;
            _menuRepo = menuRepo;
            _tableRepo = tableRepo;
        }

        public async Task<OrderResponseDto> CreateOrderAsync(OrderCreateRequestDto dto, Guid waiterId)
        {
            var table = await _tableRepo.GetByIdAsync(dto.TableId);
            if (table == null) throw new NotFoundException("Table not found");

            table.IsOccupied = true;

            foreach (var item in dto.Items)
            {
                if (item.Quantity <= 0)
                    throw new BadRequestException("Quantity must be greater than 0.");
            }

            var order = new Order
            {
                Id = Guid.NewGuid(),
                TableId = table.Id,
                WaiterId = waiterId,
                Status = OrderStatus.Pending,
                IsBilled = false,
                CreatedAt = DateTime.UtcNow
            };

            foreach (var item in dto.Items)
            {
                var menu = await _menuRepo.GetByIdAsync(item.MenuItemId);
                if (menu == null)
                    throw new NotFoundException($"Menu item not found: {item.MenuItemId}");

                if (!menu.IsAvailable)
                    throw new BadRequestException($"Menu item not available: {menu.Name}");

                var unitPrice = menu.Price;

                order.Items.Add(new OrderItem
                {
                    MenuItemId = menu.Id,
                    Quantity = item.Quantity,
                    UnitPrice = unitPrice
                });
            }

            await _orderRepo.AddAsync(order);
            await _orderRepo.SaveChangesAsync();
            await _tableRepo.SaveChangesAsync();
            var saved = await _orderRepo.GetOrderWithItemsAsync(order.Id)
                        ?? throw new InternalServerErrorException("Order saved but could not load details.");

            return MapOrderToDto(saved);
        }

        public async Task<OrderResponseDto> GetByIdAsync(Guid id)
        {
            var order = await _orderRepo.GetOrderWithItemsAsync(id)
                        ?? throw new NotFoundException("Order not found.");

            return MapOrderToDto(order);
        }
        private static OrderResponseDto MapOrderToDto(Order order)
        {
            return new OrderResponseDto
            {
                OrderId = order.Id,
                TableId = order.TableId,
                WaiterId = order.WaiterId,
                Status = order.Status,
                CreatedAt = order.CreatedAt,
                Items = order.Items.Select(i => new OrderItemResponseDto
                {
                    MenuItemId = i.MenuItemId,
                    MenuItemName = i.MenuItem?.Name ?? "",
                    UnitPrice = i.UnitPrice,
                    Quantity = i.Quantity,
                }).ToList()
            };
        }

        public async Task<List<OrderResponseDto>> GetOrdersAsync(OrderStatus status)
        {
            var list = await _orderRepo.GetOrdersForChefAsync(status);
            return list.Select(MapOrderToDto).ToList();
        }

        public async Task UpdateOrderAsync(Guid orderId, OrderUpdateRequestDto dto)
        {
            var order = await _orderRepo.GetByIdAsync(orderId)
                        ?? throw new NotFoundException("Order not found.");

            if (dto.Status != null)
            {
                order.Status = dto.Status.Value;
            }
            order.UpdatedAt = DateTime.UtcNow;

            _orderRepo.Update(order);
            await _orderRepo.SaveChangesAsync();
        }

        public async Task<List<OrderResponseDto>> GetOrdersByTableIdAsync(Guid id)
        {
            var tableOrders = await _orderRepo.GetOrderWithTableIdAsync(id);
            if (!tableOrders.Any()) throw new NotFoundException("No Orders found for the table");
            return tableOrders.Select(MapOrderToDto).ToList();
        }
    }
}

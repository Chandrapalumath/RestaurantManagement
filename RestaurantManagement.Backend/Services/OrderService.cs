using RestaurantManagement.Backend.Exceptions;
using RestaurantManagement.Backend.Services.Interfaces;
using RestaurantManagement.DataAccess.Models;
using RestaurantManagement.DataAccess.Models.Enums;
using RestaurantManagement.DataAccess.Repositories.Interfaces;
using RestaurantManagement.Dtos.Orders;

namespace RestaurantManagement.Backend.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly ICustomerRepository _customerRepo;
        private readonly IMenuRepository _menuRepo;

        public OrderService(
            IOrderRepository orderRepo,
            ICustomerRepository customerRepo,
            IMenuRepository menuRepo)
        {
            _orderRepo = orderRepo;
            _customerRepo = customerRepo;
            _menuRepo = menuRepo;
        }

        public async Task<OrderResponseDto> CreateOrderAsync(OrderCreateRequestDto dto, int waiterId)
        {
            var customer = await _customerRepo.GetByIdAsync(dto.CustomerId);
            if (customer == null)
                throw new NotFoundException("Customer not found.");

            if (dto.Items == null || dto.Items.Count == 0)
                throw new BadRequestException("Order must contain at least one item.");

            foreach (var item in dto.Items)
            {
                if (item.Quantity <= 0)
                    throw new BadRequestException("Quantity must be greater than 0.");
            }

            var order = new Order
            {
                CustomerId = dto.CustomerId,
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
                    throw new NotFoundException($"Menu item not available: {menu.Name}");

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
            var saved = await _orderRepo.GetOrderWithItemsAsync(order.Id)
                        ?? throw new InternalServerErrorException("Order saved but could not load details.");

            return MapOrderToDto(saved);
        }


        public async Task<OrderResponseDto> GetByIdAsync(int id)
        {
            var order = await _orderRepo.GetOrderWithItemsAsync(id)
                        ?? throw new NotFoundException("Order not found.");

            return MapOrderToDto(order);
        }

        public async Task<List<OrderResponseDto>> GetOrdersByCustomerIdAsync(int customerId)
        {
            var list = await _orderRepo.GetOrdersByCustomerIdAsync(customerId);
            return list.Select(MapOrderToDto).ToList();
        }
        private static OrderResponseDto MapOrderToDto(Order order)
        {
            return new OrderResponseDto
            {
                OrderId = order.Id,
                CustomerId = order.CustomerId,
                WaiterId = order.WaiterId,
                Status = order.Status.ToString(),
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
    }
}

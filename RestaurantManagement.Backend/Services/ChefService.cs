using RestaurantManagement.Backend.Exceptions;
using RestaurantManagement.Backend.Services.Interfaces;
using RestaurantManagement.DataAccess.Models;
using RestaurantManagement.DataAccess.Models.Enums;
using RestaurantManagement.DataAccess.Repositories.Interfaces;
using RestaurantManagement.Dtos.Orders;

namespace RestaurantManagement.Backend.Services
{
    public class ChefService : IChefService
    {
        private readonly IOrderRepository _orderRepo;

        public ChefService(IOrderRepository orderRepo)
        {
            _orderRepo = orderRepo;
        }

        public async Task<List<OrderResponseDto>> GetOrdersAsync(string? status)
        {
            var list = await _orderRepo.GetOrdersForChefAsync(status);
            return list.Select(MapOrderToDto).ToList();
        }

        public async Task<OrderResponseDto> GetOrderDetailsAsync(int orderId)
        {
            var order = await _orderRepo.GetOrderWithItemsAsync(orderId)
                        ?? throw new NotFoundException("Order not found.");

            return MapOrderToDto(order);
        }

        public async Task<OrderResponseDto> UpdateOrderStatusAsync(int orderId, OrderStatusUpdateRequestDto dto)
        {
            var order = await _orderRepo.GetByIdAsync(orderId)
                        ?? throw new NotFoundException("Order not found.");

            if (!Enum.TryParse<OrderStatus>(dto.Status, true, out var newStatus))
                throw new BadRequestException("Invalid status value.");

            order.Status = newStatus;
            order.UpdatedAt = DateTime.UtcNow;

            _orderRepo.Update(order);
            await _orderRepo.SaveChangesAsync();

            var updated = await _orderRepo.GetOrderWithItemsAsync(orderId)
                          ?? throw new InternalServerErrorException("Order updated but could not load details.");

            return MapOrderToDto(updated);
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
                    Quantity = i.Quantity
                }).ToList()
            };
        }
    }
}
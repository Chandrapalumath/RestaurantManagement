using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RestaurantManagement.Api.Controllers;
using RestaurantManagement.Backend.Services.Interfaces;
using RestaurantManagement.Dtos.Orders;
using RestaurantManagement.Models.Common.Enums;
using System.Security.Claims;

namespace RestaurantManagement.Api.Tests;

[TestClass]
public class OrderControllerTests
{
    private Mock<IOrderService> _orderServiceMock = null!;
    private OrderController _controller = null!;

    [TestInitialize]
    public void Setup()
    {
        _orderServiceMock = new Mock<IOrderService>();
        _controller = new OrderController(_orderServiceMock.Object);
    }
    private void SetUser(Guid userId, params string[] roles)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        };

        foreach (var role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role));

        var identity = new ClaimsIdentity(claims, "TestAuth");
        var user = new ClaimsPrincipal(identity);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
    }
    [TestMethod]
    public async Task CreateOrderAsync_ValidOrder_ReturnsCreatedAtRoute()
    {
        // Arrange
        var waiterId = Guid.NewGuid();
        SetUser(waiterId, "Waiter");

        var dto = new OrderCreateRequestDto
        {
            TableId = Guid.NewGuid(),
            Items = new List<OrderItemCreateRequestDto>
            {
                new OrderItemCreateRequestDto
                {
                    MenuItemId = Guid.NewGuid(),
                    Quantity = 2
                },
                new OrderItemCreateRequestDto
                {
                    MenuItemId = Guid.NewGuid(),
                    Quantity = 2
                }
            }
        };

        var response = new OrderResponseDto
        {
            OrderId = Guid.NewGuid(),
            TableId = dto.TableId,
            Status = OrderStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            Items = new List<OrderItemResponseDto>
            {
                new OrderItemResponseDto
                {
                    MenuItemId = Guid.NewGuid(),
                    Quantity = 2
                },
                new OrderItemResponseDto
                {
                    MenuItemId = Guid.NewGuid(),
                    Quantity = 2
                }
            }
        };

    _orderServiceMock
            .Setup(s => s.CreateOrderAsync(dto, waiterId))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.CreateOrderAsync(dto);

        // Assert
        var created = result as CreatedAtRouteResult;
        Assert.IsNotNull(created);

        Assert.AreEqual("GetOrderById", created.RouteName);
        Assert.IsNotNull(created.RouteValues);
        Assert.AreEqual(response.OrderId, created.RouteValues["id"]);
    }
    [TestMethod]
    public async Task GetOrderByIdAsync_OrderExists_ReturnsOk()
    {
        // Arrange
        var orderId = Guid.NewGuid();

        var order = new OrderResponseDto
        {
            OrderId = orderId,
            TableId = Guid.NewGuid(),
            Status = OrderStatus.Completed,
            CreatedAt = DateTime.UtcNow,
            Items = new List<OrderItemResponseDto>()
        };

        _orderServiceMock
            .Setup(s => s.GetByIdAsync(orderId))
            .ReturnsAsync(order);

        // Act
        var result = await _controller.GetOrderByIdAsync(orderId);

        // Assert
        var ok = result as OkObjectResult;
        Assert.IsNotNull(ok);

        var data = ok.Value as OrderResponseDto;
        Assert.IsNotNull(data);
        Assert.AreEqual(orderId, data.OrderId);
    }
    [TestMethod]
    public async Task GetOrdersByTableIdAsync_TableHasOrders_ReturnsOkWithOrders()
    {
        // Arrange
        var tableId = Guid.NewGuid();

        var orders = new List<OrderResponseDto>
        {
            new OrderResponseDto { OrderId = Guid.NewGuid(), TableId = tableId, Status = OrderStatus.Pending },
            new OrderResponseDto { OrderId = Guid.NewGuid(), TableId = tableId, Status = OrderStatus.Completed }
        };

        _orderServiceMock
            .Setup(s => s.GetOrdersByTableIdAsync(tableId))
            .ReturnsAsync(orders);

        // Act
        var result = await _controller.GetOrdersByTableIdAsync(tableId);

        // Assert
        var ok = result as OkObjectResult;
        Assert.IsNotNull(ok);

        var data = ok.Value as List<OrderResponseDto>;
        Assert.IsNotNull(data);
        Assert.AreEqual(2, data.Count);
    }
    [TestMethod]
    public async Task GetAllOrdersForStatusAsync_OrdersWithStatusExist_ReturnsOkWithOrders()
    {
        // Arrange
        var status = OrderStatus.Pending;

        var orders = new List<OrderResponseDto>
        {
            new OrderResponseDto { OrderId = Guid.NewGuid(), Status = status },
            new OrderResponseDto { OrderId = Guid.NewGuid(), Status = status }
        };

        _orderServiceMock
            .Setup(s => s.GetOrdersAsync(status))
            .ReturnsAsync(orders);

        // Act
        var result = await _controller.GetAllOrdersForStatusAsync(status);

        // Assert
        var ok = result as OkObjectResult;
        Assert.IsNotNull(ok);

        var data = ok.Value as List<OrderResponseDto>;
        Assert.IsNotNull(data);
        Assert.AreEqual(2, data.Count);
    }
    [TestMethod]
    public async Task UpdateOrderStatusAsync_ValidStatusUpdate_ReturnsNoContent()
    {
        // Arrange
        var orderId = Guid.NewGuid();

        var dto = new OrderUpdateRequestDto
        {
            Status = OrderStatus.Completed
        };

        _orderServiceMock
            .Setup(s => s.UpdateOrderAsync(orderId, It.IsAny<OrderUpdateRequestDto>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdateOrderStatusAsync(orderId, dto);

        // Assert
        Assert.IsInstanceOfType(result, typeof(NoContentResult));
    }
}
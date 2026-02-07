using Microsoft.EntityFrameworkCore;
using RestaurantManagement.DataAccess;
using RestaurantManagement.DataAccess.Models;
using RestaurantManagement.DataAccess.Repositories;
using RestaurantManagement.Models.Common.Enums;

namespace RestaurantManagement.Backend.Tests;

[TestClass]
public class OrderRepositoryTests
{
    private RestaurantDbContext _context = null!;
    private OrderRepository _repo = null!;

    [TestInitialize]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<RestaurantDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new RestaurantDbContext(options);
        _repo = new OrderRepository(_context);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
    
    [TestMethod]
    public async Task GetOrderWithItemsAsync_ValidOrderId_ReturnsOrderWithItemsAndMenuItem()
    {
        // Arrange
        var menuItem = new MenuItem
        {
            Id = Guid.NewGuid(),
            Name = "Pizza",
            Price = 400
        };

        var order = new Order
        {
            Id = Guid.NewGuid(),
            TableId = Guid.NewGuid(),
            WaiterId = Guid.NewGuid(),
            Status = OrderStatus.Pending
        };

        var orderItem = new OrderItem
        {
            Id = Guid.NewGuid(),
            OrderId = order.Id,
            Order = order,
            MenuItemId = menuItem.Id,
            MenuItem = menuItem,
            Quantity = 2,
            UnitPrice = 400
        };

        order.Items.Add(orderItem);

        _context.MenuItems.Add(menuItem);
        _context.Orders.Add(order);
        _context.OrderItems.Add(orderItem);

        await _context.SaveChangesAsync();

        // Act
        var result = await _repo.GetOrderWithItemsAsync(order.Id);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(order.Id, result.Id);
        Assert.AreEqual(1, result.Items.Count);
        Assert.AreEqual("Pizza", result.Items.First().MenuItem!.Name);
    }

    [TestMethod]
    public async Task GetOrderWithTableIdAsync_ValidTableId_ReturnsOnlyNotBilledOrders()
    {
        // Arrange
        var tableId = Guid.NewGuid();

        var order1 = new Order
        {
            Id = Guid.NewGuid(),
            TableId = tableId,
            WaiterId = Guid.NewGuid(),
            IsBilled = false
        };

        var order2 = new Order
        {
            Id = Guid.NewGuid(),
            TableId = tableId,
            WaiterId = Guid.NewGuid(),
            IsBilled = true
        };

        var orderOtherTable = new Order
        {
            Id = Guid.NewGuid(),
            TableId = Guid.NewGuid(),
            WaiterId = Guid.NewGuid(),
            IsBilled = false
        };

        _context.Orders.AddRange(order1, order2, orderOtherTable);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repo.GetOrderWithTableIdAsync(tableId);

        // Assert
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual(order1.Id, result.First().Id);
        Assert.IsFalse(result.First().IsBilled);
    }

    [TestMethod]
    public async Task GetNotBilledOrders_IsNotBilled_UnbilledOrders()
    {
        // Arrange
        var tableId = Guid.NewGuid();

        var unbilled = new Order
        {
            Id = Guid.NewGuid(),
            TableId = tableId,
            WaiterId = Guid.NewGuid(),
            IsBilled = false
        };

        var billed = new Order
        {
            Id = Guid.NewGuid(),
            TableId = tableId,
            WaiterId = Guid.NewGuid(),
            IsBilled = true
        };

        _context.Orders.AddRange(unbilled, billed);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repo.GetNotBilledOrders(tableId);

        // Assert
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual(unbilled.Id, result.First().Id);
    }
    
    [TestMethod]
    public async Task GetOrdersForChefAsync_WithStatus_ReturnsOnlyMatchingStatus()
    {
        // Arrange
        var pending = new Order
        {
            Id = Guid.NewGuid(),
            TableId = Guid.NewGuid(),
            WaiterId = Guid.NewGuid(),
            Status = OrderStatus.Pending,
            CreatedAt = DateTime.UtcNow.AddMinutes(-10)
        };

        var preparing = new Order
        {
            Id = Guid.NewGuid(),
            TableId = Guid.NewGuid(),
            WaiterId = Guid.NewGuid(),
            Status = OrderStatus.Preparing,
            CreatedAt = DateTime.UtcNow
        };

        _context.Orders.AddRange(pending, preparing);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repo.GetOrdersForChefAsync(OrderStatus.Pending);

        // Assert
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual(OrderStatus.Pending, result.First().Status);
    }

    [TestMethod]
    public async Task GetOrdersForChefAsync_Default_ReturnsPendingAndPreparing()
    {
        // Arrange
        var pending = new Order
        {
            Id = Guid.NewGuid(),
            TableId = Guid.NewGuid(),
            WaiterId = Guid.NewGuid(),
            Status = OrderStatus.Pending,
            CreatedAt = DateTime.UtcNow.AddMinutes(-30)
        };

        var preparing = new Order
        {
            Id = Guid.NewGuid(),
            TableId = Guid.NewGuid(),
            WaiterId = Guid.NewGuid(),
            Status = OrderStatus.Preparing,
            CreatedAt = DateTime.UtcNow.AddMinutes(-5)
        };

        var completed = new Order
        {
            Id = Guid.NewGuid(),
            TableId = Guid.NewGuid(),
            WaiterId = Guid.NewGuid(),
            Status = OrderStatus.Completed
        };

        _context.Orders.AddRange(pending, preparing, completed);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repo.GetOrdersForChefAsync(default);

        // Assert
        Assert.AreEqual(2, result.Count);
        Assert.IsTrue(result.All(o => o.Status == OrderStatus.Pending || o.Status == OrderStatus.Preparing));
    }
    [TestMethod]
    public async Task GetOrdersByBillId_ReturnsOrders()
    {
        var billId = Guid.NewGuid();

        _context.Orders.Add(new Order
        {
            Id = Guid.NewGuid(),
            BillingId = billId
        });

        await _context.SaveChangesAsync();

        var result = await _repo.GetOrdersByBillIdAsync(billId);

        Assert.AreEqual(1, result.Count);
    }
    [TestMethod]
    public async Task GetOrderWithWaiterId_ReturnsFilteredOrders()
    {
        var waiterId = Guid.NewGuid();

        _context.Orders.Add(new Order
        {
            Id = Guid.NewGuid(),
            WaiterId = waiterId,
            IsBilled = false,
            Status = OrderStatus.Pending
        });

        await _context.SaveChangesAsync();

        var result = await _repo.GetOrderWithWaiterIdAsync(waiterId);

        Assert.AreEqual(1, result.Count);
    }

}

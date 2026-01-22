using Moq;
using RestaurantManagement.Backend.Exceptions;
using RestaurantManagement.Backend.Services;
using RestaurantManagement.DataAccess.Models;
using RestaurantManagement.DataAccess.Repositories.Interfaces;
using RestaurantManagement.Dtos.Orders;
using RestaurantManagement.Models.Common.Enums;

namespace RestaurantManagement.Backend.Tests.Services
{
    [TestClass]
    public class OrderServiceTests
    {
        private Mock<IOrderRepository> _orderRepoMock = null!;
        private Mock<ICustomerRepository> _customerRepoMock = null!;
        private Mock<IMenuRepository> _menuRepoMock = null!;
        private Mock<ITableRepository> _tableRepoMock = null!;
        private OrderService _service = null!;

        [TestInitialize]
        public void Setup()
        {
            _orderRepoMock = new Mock<IOrderRepository>();
            _customerRepoMock = new Mock<ICustomerRepository>();
            _menuRepoMock = new Mock<IMenuRepository>();
            _tableRepoMock = new Mock<ITableRepository>();

            _service = new OrderService(
                _orderRepoMock.Object,
                _customerRepoMock.Object,
                _menuRepoMock.Object,
                _tableRepoMock.Object);
        }
        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task CreateOrderAsync_ShouldThrowNotFound_WhenTableNotFound()
        {
            // Arrange
            var dto = new OrderCreateRequestDto
            {
                TableId = Guid.NewGuid(),
                Items = new List<OrderItemCreateRequestDto>
                {
                    new OrderItemCreateRequestDto { MenuItemId = Guid.NewGuid(), Quantity = 1 }
                }
            };

            _tableRepoMock.Setup(r => r.GetByIdAsync(dto.TableId))
                          .ReturnsAsync((Table?)null);

            // Act + Assert
            try
            {
                await _service.CreateOrderAsync(dto, Guid.NewGuid());
            }
            catch (NotFoundException ex)
            {
                Assert.AreEqual("Table not found", ex.Message);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException (typeof(BadRequestException))]
        public async Task CreateOrderAsync_ShouldThrowBadRequest_WhenAnyItemQuantityIsZeroOrNegative()
        {
            // Arrange
            var table = new Table { Id = Guid.NewGuid(), IsOccupied = false };
            var dto = new OrderCreateRequestDto
            {
                TableId = table.Id,
                Items = new List<OrderItemCreateRequestDto>
                {
                    new OrderItemCreateRequestDto { MenuItemId = Guid.NewGuid(), Quantity = 0 }
                }
            };

            _tableRepoMock.Setup(r => r.GetByIdAsync(table.Id)).ReturnsAsync(table);

            // Act + Assert
            try
            {
                await _service.CreateOrderAsync(dto, Guid.NewGuid());
            } catch(BadRequestException ex)
            {
                Assert.AreEqual("Quantity must be greater than 0.", ex.Message);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException (typeof(NotFoundException))]
        public async Task CreateOrderAsync_ShouldThrowNotFound_WhenMenuItemNotFound()
        {
            // Arrange
            var table = new Table { Id = Guid.NewGuid(), IsOccupied = false };

            var menuItemId = Guid.NewGuid();

            var dto = new OrderCreateRequestDto
            {
                TableId = table.Id,
                Items = new List<OrderItemCreateRequestDto>
                {
                    new OrderItemCreateRequestDto { MenuItemId = menuItemId, Quantity = 2 }
                }
            };

            _tableRepoMock.Setup(r => r.GetByIdAsync(table.Id)).ReturnsAsync(table);

            _menuRepoMock.Setup(r => r.GetByIdAsync(menuItemId))
                         .ReturnsAsync((MenuItem?)null);

            // Act + Assert
            try
            {
                await _service.CreateOrderAsync(dto, Guid.NewGuid());
            }
            catch (NotFoundException ex)
            {
                Assert.AreEqual($"Menu item not found: {menuItemId}", ex.Message);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(BadRequestException))]
        public async Task CreateOrderAsync_ShouldThrowNotFound_WhenMenuItemIsNotAvailable()
        {
            // Arrange
            var table = new Table { Id = Guid.NewGuid(), IsOccupied = false };

            var menuItem = new MenuItem
            {
                Id = Guid.NewGuid(),
                Name = "Pizza",
                Price = 200,
                IsAvailable = false
            };

            var dto = new OrderCreateRequestDto
            {
                TableId = table.Id,
                Items = new List<OrderItemCreateRequestDto>
                {
                    new OrderItemCreateRequestDto { MenuItemId = menuItem.Id, Quantity = 1 }
                }
            };

            _tableRepoMock.Setup(r => r.GetByIdAsync(table.Id)).ReturnsAsync(table);

            _menuRepoMock.Setup(r => r.GetByIdAsync(menuItem.Id))
                         .ReturnsAsync(menuItem);

            // Act + Assert
            try
            {
                await _service.CreateOrderAsync(dto, Guid.NewGuid());
            } catch(BadRequestException ex)
            {
                Assert.AreEqual($"Menu item not available: {menuItem.Name}", ex.Message);
                throw;
            }
        }

        [TestMethod]
        public async Task CreateOrderAsync_ShouldCreateOrder_SaveAndReturnMappedDto()
        {
            // Arrange
            var table = new Table { Id = Guid.NewGuid(), IsOccupied = false };
            var waiterId = Guid.NewGuid();

            var menu1 = new MenuItem { Id = Guid.NewGuid(), Name = "Burger", Price = 100, IsAvailable = true };
            var menu2 = new MenuItem { Id = Guid.NewGuid(), Name = "Fries", Price = 60, IsAvailable = true };

            var dto = new OrderCreateRequestDto
            {
                TableId = table.Id,
                Items = new List<OrderItemCreateRequestDto>
                {
                    new OrderItemCreateRequestDto { MenuItemId = menu1.Id, Quantity = 2 },
                    new OrderItemCreateRequestDto { MenuItemId = menu2.Id, Quantity = 1 }
                }
            };

            _tableRepoMock.Setup(r => r.GetByIdAsync(table.Id)).ReturnsAsync(table);
            _menuRepoMock.Setup(r => r.GetByIdAsync(menu1.Id)).ReturnsAsync(menu1);
            _menuRepoMock.Setup(r => r.GetByIdAsync(menu2.Id)).ReturnsAsync(menu2);

            Order? capturedOrder = null;

            _orderRepoMock
                .Setup(r => r.AddAsync(It.IsAny<Order>()))
                .Callback<Order>(o => capturedOrder = o)
                .Returns(Task.CompletedTask);

            _orderRepoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            Order? savedOrder = null;
            _orderRepoMock
                .Setup(r => r.GetOrderWithItemsAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Guid orderId) =>
                {
                    savedOrder ??= new Order
                    {
                        Id = orderId,
                        TableId = table.Id,
                        WaiterId = waiterId,
                        Status = OrderStatus.Pending,
                        IsBilled = false,
                        CreatedAt = DateTime.UtcNow.AddSeconds(-1),
                        Items = new List<OrderItem>
                        {
                            new OrderItem
                            {
                                MenuItemId = menu1.Id,
                                Quantity = 2,
                                UnitPrice = menu1.Price,
                                MenuItem = menu1
                            },
                            new OrderItem
                            {
                                MenuItemId = menu2.Id,
                                Quantity = 1,
                                UnitPrice = menu2.Price,
                                MenuItem = menu2
                            }
                        }
                    };
                    return savedOrder;
                });

            // Act
            var result = await _service.CreateOrderAsync(dto, waiterId);
            
            // Assert
            Assert.IsTrue(table.IsOccupied);
            Assert.IsNotNull(capturedOrder);
            Assert.AreEqual(table.Id, capturedOrder.TableId);
            Assert.AreEqual(waiterId, capturedOrder.WaiterId);
            Assert.AreEqual(OrderStatus.Pending, capturedOrder.Status);
            Assert.IsFalse(capturedOrder.IsBilled);
            Assert.AreEqual(2, capturedOrder.Items.Count);
            Assert.IsNotNull(savedOrder);
            Assert.AreEqual(savedOrder.Id, result.OrderId);
            Assert.AreEqual(table.Id, result.TableId);
            Assert.AreEqual(waiterId, result.WaiterId);
            Assert.AreEqual(OrderStatus.Pending, result.Status);
            Assert.AreEqual(2, result.Items.Count);
            Assert.AreEqual("Burger", result.Items[0].MenuItemName);
            Assert.AreEqual("Fries", result.Items[1].MenuItemName);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task GetByIdAsync_ShouldThrowNotFound_WhenOrderNotExists()
        {
            // Arrange
            var id = Guid.NewGuid();

            _orderRepoMock.Setup(r => r.GetOrderWithItemsAsync(id)).ReturnsAsync((Order?)null);

            // Act + Assert
            try
            {
                await _service.GetByIdAsync(id);
            }
            catch (NotFoundException ex)
            {
                Assert.AreEqual("Order not found.", ex.Message);
                throw;
            }
        }

        [TestMethod]
        public async Task GetByIdAsync_ShouldReturnMappedDto_WhenOrderExists()
        {
            // Arrange
            var order = new Order
            {
                Id = Guid.NewGuid(),
                TableId = Guid.NewGuid(),
                WaiterId = Guid.NewGuid(),
                Status = OrderStatus.Pending,
                CreatedAt = DateTime.UtcNow.AddMinutes(-10),
                Items = new List<OrderItem>
                {
                    new OrderItem
                    {
                        MenuItemId = Guid.NewGuid(),
                        Quantity = 1,
                        UnitPrice = 99,
                        MenuItem = new MenuItem { Name = "Soup" }
                    }
                }
            };

            _orderRepoMock.Setup(r => r.GetOrderWithItemsAsync(order.Id)).ReturnsAsync(order);

            // Act
            var result = await _service.GetByIdAsync(order.Id);

            // Assert
            Assert.AreEqual(order.Id, result.OrderId);
            Assert.AreEqual(order.TableId, result.TableId);
            Assert.AreEqual(order.WaiterId, result.WaiterId);
            Assert.AreEqual(order.Status, result.Status);
            Assert.AreEqual(1, result.Items.Count);
            Assert.AreEqual("Soup", result.Items[0].MenuItemName);
        }

        [TestMethod]
        public async Task GetOrdersAsync_ShouldReturnMappedList()
        {
            // Arrange
            var list = new List<Order>
            {
                new Order
                {
                    Id = Guid.NewGuid(),
                    TableId = Guid.NewGuid(),
                    WaiterId = Guid.NewGuid(),
                    Status = OrderStatus.Pending,
                    CreatedAt = DateTime.UtcNow.AddMinutes(-30),
                    Items = new List<OrderItem>
                    {
                        new OrderItem
                        {
                            MenuItemId = Guid.NewGuid(),
                            Quantity = 2,
                            UnitPrice = 100,
                            MenuItem = new MenuItem { Name = "Pasta" }
                        }
                    }
                }
            };

            _orderRepoMock.Setup(r => r.GetOrdersForChefAsync(OrderStatus.Pending)).ReturnsAsync(list);

            // Act
            var result = await _service.GetOrdersAsync(OrderStatus.Pending);

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(list[0].Id, result[0].OrderId);
            Assert.AreEqual("Pasta", result[0].Items[0].MenuItemName);
        }
        
        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task UpdateOrderAsync_ShouldThrowNotFound_WhenOrderNotExists()
        {
            // Arrange
            var orderId = Guid.NewGuid();

            _orderRepoMock.Setup(r => r.GetByIdAsync(orderId)).ReturnsAsync((Order?)null);

            var dto = new OrderUpdateRequestDto { Status = OrderStatus.Completed };

            // Act + Assert
            try
            {
                await _service.UpdateOrderAsync(orderId, dto);
            } 
            catch(NotFoundException ex)
            {
                Assert.AreEqual("Order not found.", ex.Message);
                throw;
            }
        }

        [TestMethod]
        public async Task UpdateOrderAsync_ShouldUpdateStatus_SaveAndCallGetOrderWithItems()
        {
            // Arrange
            var orderId = Guid.NewGuid();

            var order = new Order
            {
                Id = orderId,
                Status = OrderStatus.Pending,
                UpdatedAt = null
            };

            var dto = new OrderUpdateRequestDto
            {
                Status = OrderStatus.Completed
            };

            _orderRepoMock.Setup(r => r.GetByIdAsync(orderId)).ReturnsAsync(order);
            _orderRepoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            _orderRepoMock.Setup(r => r.GetOrderWithItemsAsync(orderId))
                          .ReturnsAsync(new Order
                          {
                              Id = orderId,
                              Status = OrderStatus.Completed,
                              Items = new List<OrderItem>()
                          });

            // Act
            await _service.UpdateOrderAsync(orderId, dto);

            // Assert
            Assert.AreEqual(OrderStatus.Completed, order.Status);
            Assert.IsTrue(order.UpdatedAt.HasValue);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task GetOrdersByTableIdAsync_ShouldThrowNotFound_WhenNoOrdersFound()
        {
            // Arrange
            var tableId = Guid.NewGuid();

            _orderRepoMock.Setup(r => r.GetOrderWithTableIdAsync(tableId)).ReturnsAsync(new List<Order>());

            // Act + Assert
            try
            {
                await _service.GetOrdersByTableIdAsync(tableId);
            }
            catch (NotFoundException ex)
            {
                Assert.AreEqual("No Orders found for the table", ex.Message);
                throw;
            }
        }

        [TestMethod]
        public async Task GetOrdersByTableIdAsync_ShouldReturnMappedList_WhenOrdersExist()
        {
            // Arrange
            var tableId = Guid.NewGuid();

            var orders = new List<Order>
            {
                new Order
                {
                    Id = Guid.NewGuid(),
                    TableId = tableId,
                    WaiterId = Guid.NewGuid(),
                    Status = OrderStatus.Pending,
                    CreatedAt = DateTime.UtcNow.AddMinutes(-2),
                    Items = new List<OrderItem>
                    {
                        new OrderItem
                        {
                            MenuItemId = Guid.NewGuid(),
                            Quantity = 1,
                            UnitPrice = 50,
                            MenuItem = new MenuItem { Name = "Tea" }
                        }
                    }
                }
            };

            _orderRepoMock.Setup(r => r.GetOrderWithTableIdAsync(tableId))
                          .ReturnsAsync(orders);

            // Act
            var result = await _service.GetOrdersByTableIdAsync(tableId);

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(orders[0].Id, result[0].OrderId);
            Assert.AreEqual("Tea", result[0].Items[0].MenuItemName);
        }
    }
}
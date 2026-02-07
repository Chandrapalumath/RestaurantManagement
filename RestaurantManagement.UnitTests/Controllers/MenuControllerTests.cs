using Microsoft.AspNetCore.Mvc;
using Moq;
using RestaurantManagement.Api.Controllers;
using RestaurantManagement.Backend.Services.Interfaces;
using RestaurantManagement.Dtos.Menu;

namespace RestaurantManagement.Api.Tests;

[TestClass]
public class MenuControllerTests
{
    private Mock<IMenuService> _menuServiceMock = null!;
    private Mock<IBillingService> _billServiceMock = null!;
    private MenuController _controller = null!;

    [TestInitialize]
    public void Setup()
    {
        _menuServiceMock = new Mock<IMenuService>();
        _billServiceMock = new Mock<IBillingService>();
        _controller = new MenuController(_menuServiceMock.Object, _billServiceMock.Object);
    }

    [TestMethod]
    public async Task GetAllMenuItemsAsync_ItemsExist_ReturnsOkWithItems()
    {
        // Arrange
        var menu = new List<MenuItemResponseDto>
            {
                new MenuItemResponseDto { Id = Guid.NewGuid(), Name = "Burger", Price = 120, IsAvailable = true, Rating = 5, CustomerId= Guid.NewGuid() },
                new MenuItemResponseDto { Id = Guid.NewGuid(), Name = "Pizza", Price = 250, IsAvailable = true, Rating = 5, CustomerId= Guid.NewGuid()  }
            };

        _menuServiceMock
            .Setup(s => s.GetAllAsync())
            .ReturnsAsync(menu);

        // Act
        var result = await _controller.GetAllMenuItemsAsync();

        // Assert
        var ok = result as OkObjectResult;
        Assert.IsNotNull(ok);

        var data = ok.Value as List<MenuItemResponseDto>;
        Assert.IsNotNull(data);
        Assert.AreEqual(2, data.Count);
    }
    [TestMethod]
    public async Task GetMenuItemByIdAsync_MenuItemExists_ReturnsOk()
    {
        // Arrange
        var id = Guid.NewGuid();
        var item = new MenuItemResponseDto
        {
            Id = id,
            Name = "Burger",
            Price = 150,
            IsAvailable = true,
            Rating = 5,
            CustomerId = Guid.NewGuid()
        };

        _menuServiceMock
            .Setup(s => s.GetByIdAsync(id))
            .ReturnsAsync(item);

        // Act
        var result = await _controller.GetMenuItemByIdAsync(id);

        // Assert
        var ok = result as OkObjectResult;
        Assert.IsNotNull(ok);

        var data = ok.Value as MenuItemResponseDto;
        Assert.IsNotNull(data);
        Assert.AreEqual(id, data.Id);
    }
    [TestMethod]
    public async Task CreateMenuItemAsync_ValidMenuItem_ReturnsCreatedAtRoute()
    {
        // Arrange
        var dto = new MenuItemCreateRequestDto
        {
            Name = "Sandwich",
            Price = 80,
            IsAvailable = true
        };

        var created = new MenuItemResponseDto
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Price = dto.Price,
            IsAvailable = true,
            Rating = 4,
            CustomerId = Guid.NewGuid()
        };

        _menuServiceMock
            .Setup(s => s.CreateAsync(dto))
            .ReturnsAsync(created);

        // Act
        var result = await _controller.CreateMenuItemAsync(dto);

        // Assert
        var createdAt = result as CreatedAtRouteResult;
        Assert.IsNotNull(createdAt);
        Assert.IsNull(createdAt.Value);
        Assert.AreEqual("GetMenuItemById", createdAt.RouteName);
        Assert.IsNotNull(createdAt.RouteValues);
        Assert.AreEqual(created.Id, createdAt.RouteValues["id"]);
    }
    [TestMethod]
    public async Task GetMenuItemByBillIdAsync_ReturnsOk()
    {
        var billId = Guid.NewGuid();
        var list = new List<MenuItemResponseDto>
    {
        new MenuItemResponseDto { Id = Guid.NewGuid(), Name = "Burger", Price = 120 }
    };

        _billServiceMock
            .Setup(b => b.GetMenuItemByBillIdAsync(billId))
            .ReturnsAsync(list);

        var result = await _controller.GetMenuItemByBillIdAsync(billId);

        var ok = result as OkObjectResult;
        Assert.IsNotNull(ok);
    }
    [TestMethod]
    public async Task UpdateMenuItemAsync_ValidUpdate_ReturnsNoContent()
    {
        // Arrange
        var id = Guid.NewGuid();
        var dto = new MenuItemUpdateRequestDto
        {
            Name = "Updated Name",
            Price = 200,
            IsAvailable = true
        };

        _menuServiceMock
            .Setup(s => s.UpdateAsync(id, dto))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdateMenuItemAsync(id, dto);

        // Assert
        Assert.IsInstanceOfType(result, typeof(NoContentResult));
    }
    [TestMethod]
    public async Task DeleteMenuItemAsync_MenuItemExists_ReturnsNoContent()
    {
        // Arrange
        var id = Guid.NewGuid();

        _menuServiceMock
            .Setup(s => s.DeleteAsync(id))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteMenuItemAsync(id);

        // Assert
        Assert.IsInstanceOfType(result, typeof(NoContentResult));
    }
    [TestMethod]
    public async Task UpdateMenuItemRatingAsync_ValidRating_ReturnsNoContent()
    {
        // Arrange
        var dto = new List<UpdateMenuItemRating>
        {
            new UpdateMenuItemRating { Id = Guid.NewGuid(), Rating = 4 },
            new UpdateMenuItemRating { Id = Guid.NewGuid(), Rating = 5 }
        };

        _menuServiceMock
            .Setup(s => s.UpdateRatingAsync(dto))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdateMenuItemRatingAsync(dto);

        // Assert
        Assert.IsInstanceOfType(result, typeof(NoContentResult));
    }
}

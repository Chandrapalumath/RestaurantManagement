using Moq;
using RestaurantManagement.Backend.Exceptions;
using RestaurantManagement.Backend.Services;
using RestaurantManagement.DataAccess.Models;
using RestaurantManagement.DataAccess.Repositories.Interfaces;
using RestaurantManagement.Dtos.Menu;

namespace RestaurantManagement.Backend.Tests;

[TestClass]
public class MenuServiceTests
{
    private Mock<IMenuRepository> _menuRepoMock = null!;
    private MenuService _service = null!;

    [TestInitialize]
    public void Setup()
    {
        _menuRepoMock = new Mock<IMenuRepository>();
        _service = new MenuService(_menuRepoMock.Object);
    }

    [TestMethod]
    [ExpectedException(typeof(NotFoundException))]
    public async Task GetAllAsync_ShouldThrowNotFound_WhenNoItems()
    {
        // Arrange
        _menuRepoMock.Setup(r => r.GetAllAsync())
                     .ReturnsAsync(new List<MenuItem>());

        // Act + Assert
        try
        {
            await _service.GetAllAsync();
        } catch(NotFoundException ex)
        {
            Assert.AreEqual("No Items Found", ex.Message);
            throw;
        }
    }

    [TestMethod]
    public async Task GetAllAsync_ShouldReturnMappedList_WhenItemsExist()
    {
        // Arrange
        var items = new List<MenuItem>
            {
                new MenuItem
                {
                    Id = Guid.NewGuid(),
                    Name = "Pizza",
                    Price = 250,
                    IsAvailable = true,
                    Rating = 4
                }
            };

        _menuRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(items);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual("Pizza", result[0].Name);
        Assert.AreEqual(250, result[0].Price);
        Assert.IsTrue(result[0].IsAvailable);
        Assert.AreEqual(4, result[0].Rating);
    }

    [TestMethod]
    [ExpectedException(typeof(NotFoundException))]
    public async Task GetByIdAsync_ShouldThrowNotFound_WhenItemDoesNotExist()
    {
        // Arrange
        _menuRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                     .ReturnsAsync((MenuItem?)null);

        // Act + Assert
        try
        {
            await _service.GetByIdAsync(Guid.NewGuid());
        } catch(NotFoundException ex) {
            Assert.AreEqual("Menu item not found.", ex.Message);
            throw;
        }    
    }

    [TestMethod]
    public async Task GetByIdAsync_ShouldReturnMappedDto_WhenItemExists()
    {
        // Arrange
        var item = new MenuItem
        {
            Id = Guid.NewGuid(),
            Name = "Burger",
            Price = 120,
            IsAvailable = true,
            Rating = 3
        };

        _menuRepoMock.Setup(r => r.GetByIdAsync(item.Id))
                     .ReturnsAsync(item);

        // Act
        var result = await _service.GetByIdAsync(item.Id);

        // Assert
        Assert.AreEqual(item.Id, result.Id);
        Assert.AreEqual("Burger", result.Name);
        Assert.AreEqual(120, result.Price);
        Assert.AreEqual(3, result.Rating);
    }

    [TestMethod]
    [ExpectedException(typeof(ConflictException))]
    public async Task CreateAsync_ShouldThrowConflict_WhenItemAlreadyExists()
    {
        // Arrange
        var dto = new MenuItemCreateRequestDto
        {
            Name = "Pizza",
            Price = 200,
            IsAvailable = true
        };

        _menuRepoMock.Setup(r => r.GetByNameAsync("pizza"))
                     .ReturnsAsync(new MenuItem());

        // Act + Assert
        try
        {
            await  _service.CreateAsync(dto);
        }
        catch(ConflictException ex) 
        {
            Assert.AreEqual("Item Already Exists", ex.Message);
            throw;
        }
    }

    [TestMethod]
    public async Task CreateAsync_ShouldCreateItem_SaveAndReturnDto()
    {
        // Arrange
        var dto = new MenuItemCreateRequestDto
        {
            Name = "  Pasta ",
            Price = 180,
            IsAvailable = true
        };

        _menuRepoMock.Setup(r => r.GetByNameAsync("  Pasta "))
                     .ReturnsAsync((MenuItem?)null);

        MenuItem? capturedItem = null;

        _menuRepoMock
            .Setup(r => r.AddAsync(It.IsAny<MenuItem>()))
            .Callback<MenuItem>(m => capturedItem = m)
            .Returns(Task.CompletedTask);

        _menuRepoMock.Setup(r => r.SaveChangesAsync())
                     .Returns(Task.CompletedTask);

        // Act
        var result = await _service.CreateAsync(dto);

        // Assert
        Assert.IsNotNull(capturedItem);
        Assert.AreEqual("pasta", capturedItem!.Name);
        Assert.AreEqual(0, capturedItem.Rating);
        Assert.AreEqual(0, capturedItem.TotalReviews);

        Assert.AreEqual("pasta", result.Name);
        Assert.AreEqual(180, result.Price);
        Assert.AreEqual(0, result.Rating);
    }

    [TestMethod]
    [ExpectedException(typeof(NotFoundException))]
    public async Task UpdateAsync_ShouldThrowNotFound_WhenItemDoesNotExist()
    {
        // Arrange
        _menuRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                     .ReturnsAsync((MenuItem?)null);

        var dto = new MenuItemUpdateRequestDto { Name = "NewName" };

        // Act + Assert
        try
        {
            await _service.UpdateAsync(Guid.NewGuid(), dto);
        } catch(NotFoundException ex)
        {
            Assert.AreEqual("Menu item not found.", ex.Message);
            throw;
        } 
    }

    [TestMethod]
    public async Task UpdateAsync_ShouldUpdateProvidedFields()
    {
        // Arrange
        var item = new MenuItem
        {
            Id = Guid.NewGuid(),
            Name = "Old",
            Price = 100,
            IsAvailable = false
        };

        var dto = new MenuItemUpdateRequestDto
        {
            Name = " New ",
            Price = 150,
            IsAvailable = true
        };

        _menuRepoMock.Setup(r => r.GetByIdAsync(item.Id))
                     .ReturnsAsync(item);

        _menuRepoMock.Setup(r => r.SaveChangesAsync())
                     .Returns(Task.CompletedTask);

        // Act
        await _service.UpdateAsync(item.Id, dto);

        // Assert
        Assert.AreEqual("New", item.Name);
        Assert.AreEqual(150, item.Price);
        Assert.IsTrue(item.IsAvailable);
    }
    [TestMethod]
    [ExpectedException(typeof(NotFoundException))]
    public async Task DeleteAsync_ShouldThrowNotFound_WhenItemDoesNotExist()
    {
        _menuRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                     .ReturnsAsync((MenuItem?)null);
        try
        {
            await _service.DeleteAsync(Guid.NewGuid());
        }
        catch(NotFoundException ex)
        {
            Assert.AreEqual("Menu item not found.", ex.Message);
            throw;
        }
    }

    [TestMethod]
    public async Task DeleteAsync_ShouldDeleteItem_WhenExists()
    {
        var item = new MenuItem { Id = Guid.NewGuid() };

        _menuRepoMock.Setup(r => r.GetByIdAsync(item.Id))
                     .ReturnsAsync(item);

        _menuRepoMock.Setup(r => r.SaveChangesAsync())
                     .Returns(Task.CompletedTask);

        await _service.DeleteAsync(item.Id);
    }

    [TestMethod]
    [ExpectedException(typeof(BadRequestException))]
    public async Task UpdateRatingAsync_ShouldThrowBadRequest_WhenItemNotFound()
    {
        var dto = new List<UpdateMenuItemRating>
        {
            new UpdateMenuItemRating { Id = Guid.NewGuid(), Rating = 4 }
        };

        _menuRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                     .ReturnsAsync((MenuItem?)null);
        try
        {
            await _service.UpdateRatingAsync(dto);
        }
        catch (BadRequestException ex)
        {
            Assert.AreEqual("Item Not Found", ex.Message);
            throw;
        }
    }

    [TestMethod]
    public async Task UpdateRatingAsync_ShouldRecalculateAverageCorrectly()
    {
        var item = new MenuItem
        {
            Id = Guid.NewGuid(),
            Rating = 4,
            TotalReviews = 2
        };

        var dto = new List<UpdateMenuItemRating>
        {
            new UpdateMenuItemRating { Id = item.Id, Rating = 5 }
        };

        _menuRepoMock.Setup(r => r.GetByIdAsync(item.Id))
                     .ReturnsAsync(item);

        _menuRepoMock.Setup(r => r.SaveChangesAsync())
                     .Returns(Task.CompletedTask);

        // Act
        await _service.UpdateRatingAsync(dto);

        Assert.AreEqual(3, item.TotalReviews);
        Assert.AreEqual((4 * 2 + 5) / 3, item.Rating.Value);
    }

}

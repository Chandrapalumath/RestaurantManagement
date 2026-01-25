using Microsoft.EntityFrameworkCore;
using RestaurantManagement.DataAccess;
using RestaurantManagement.DataAccess.Models;
using RestaurantManagement.DataAccess.Repositories;

namespace RestaurantManagement.Backend.Tests;

[TestClass]
public class MenuRepositoryTests
{
    private RestaurantDbContext _context = null!;
    private MenuRepository _repo = null!;

    [TestInitialize]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<RestaurantDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new RestaurantDbContext(options);
        _repo = new MenuRepository(_context);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [TestMethod]
    public async Task GetByNameAsync_ExistingName_ReturnsMenuItem()
    {
        // Arrange
        var menuItem = new MenuItem
        {
            Id = Guid.NewGuid(),
            Name = "Paneer Butter Masala",
            Price = 300,
            IsAvailable = true
        };

        _context.MenuItems.Add(menuItem);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repo.GetByNameAsync("Paneer Butter Masala");

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(menuItem.Id, result.Id);
        Assert.AreEqual("Paneer Butter Masala", result.Name);
        Assert.AreEqual(300, result.Price);
    }

    [TestMethod]
    public async Task GetByNameAsync_NameDoesNotExist_ReturnsNull()
    {
        // Arrange
        var menuItem = new MenuItem
        {
            Id = Guid.NewGuid(),
            Name = "Veg Biryani",
            Price = 250
        };

        _context.MenuItems.Add(menuItem);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repo.GetByNameAsync("Chicken Biryani");

        // Assert
        Assert.IsNull(result);
    }

    [TestMethod]
    public async Task GetByNameAsync_CaseSensitiveSearch_ReturnsNull()
    {
        // Arrange
        var menuItem = new MenuItem
        {
            Id = Guid.NewGuid(),
            Name = "Masala Dosa",
            Price = 150
        };

        _context.MenuItems.Add(menuItem);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repo.GetByNameAsync("masala dosa");

        // Assert
        Assert.IsNull(result);
    }
}

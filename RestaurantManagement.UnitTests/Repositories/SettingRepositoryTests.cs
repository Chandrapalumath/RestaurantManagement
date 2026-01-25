using Microsoft.EntityFrameworkCore;
using RestaurantManagement.DataAccess;
using RestaurantManagement.DataAccess.Models;
using RestaurantManagement.DataAccess.Repositories;

namespace RestaurantManagement.Backend.Tests;

[TestClass]
public class SettingRepositoryTests
{
    private RestaurantDbContext _context = null!;
    private SettingsRepository _repo = null!;

    [TestInitialize]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<RestaurantDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new RestaurantDbContext(options);
        _repo = new SettingsRepository(_context);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [TestMethod]
    public async Task GetSettingsAsync_WhenSettingsExist_ReturnsSettings()
    {
        // Arrange
        var settings = new RestaurantSettings
        {
            Id = Guid.NewGuid(),
            TaxPercent = 5,
            DiscountPercent = 10,
            UpdatedAt = DateTime.UtcNow,
            UpdatedByAdminId = Guid.NewGuid()
        };

        _context.RestaurantSettings.Add(settings);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repo.GetSettingsAsync();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(settings.Id, result.Id);
        Assert.AreEqual(5, result.TaxPercent);
        Assert.AreEqual(10, result.DiscountPercent);
    }

    [TestMethod]
    public async Task GetSettingsAsync_WhenNoSettingsExist_ReturnsNull()
    {
        // Act
        var result = await _repo.GetSettingsAsync();

        // Assert
        Assert.IsNull(result);
    }
 
    [TestMethod]
    public async Task GetSettingsAsync_WhenMultipleSettingsExist_ReturnsFirst()
    {
        // Arrange
        var first = new RestaurantSettings
        {
            Id = Guid.NewGuid(),
            TaxPercent = 5,
            DiscountPercent = 5,
            UpdatedByAdminId = Guid.NewGuid()
        };

        var second = new RestaurantSettings
        {
            Id = Guid.NewGuid(),
            TaxPercent = 10,
            DiscountPercent = 15,
            UpdatedByAdminId = Guid.NewGuid()
        };

        _context.RestaurantSettings.AddRange(first, second);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repo.GetSettingsAsync();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(first.Id, result.Id);
    }

}

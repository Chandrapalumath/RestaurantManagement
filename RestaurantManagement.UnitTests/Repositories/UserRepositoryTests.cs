using Microsoft.EntityFrameworkCore;
using RestaurantManagement.DataAccess;
using RestaurantManagement.DataAccess.Models;
using RestaurantManagement.DataAccess.Repositories;
using RestaurantManagement.Models.Common.Enums;

namespace RestaurantManagement.Backend.Tests;

[TestClass]
public class UserRepositoryTests
{
    private RestaurantDbContext _context = null!;
    private UserRepository _repo = null!;

    [TestInitialize]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<RestaurantDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new RestaurantDbContext(options);
        _repo = new UserRepository(_context);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
    [TestMethod]
    public async Task GetByEmailAsync_ExistingEmail_ReturnsUser()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "Rahul",
            Email = "rahul@test.com",
            MobileNumber = "9876543210",
            Password = "Test@123",
            Role = UserRole.Waiter,
            IsActive = true
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repo.GetByEmailAsync("rahul@test.com");

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(user.Id, result.Id);
        Assert.AreEqual("Rahul", result.Name);
        Assert.AreEqual(UserRole.Waiter, result.Role);
    }
    
    [TestMethod]
    public async Task GetByEmailAsync_EmailDoesNotExist_ReturnsNull()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "Amit",
            Email = "amit@test.com",
            MobileNumber = "9999999999",
            Password = "Test@123",
            Role = UserRole.Admin
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repo.GetByEmailAsync("unknown@test.com");

        // Assert
        Assert.IsNull(result);
    }

    [TestMethod]
    public async Task GetByEmailAsync_CaseSensitiveSearch_ReturnsNull()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "Suresh",
            Email = "suresh@test.com",
            MobileNumber = "8888888888",
            Password = "Test@123",
            Role = UserRole.Chef
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repo.GetByEmailAsync("SURESH@test.com");

        // Assert
        Assert.IsNull(result);
    }
}

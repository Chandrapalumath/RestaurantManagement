using Microsoft.EntityFrameworkCore;
using RestaurantManagement.DataAccess;
using RestaurantManagement.DataAccess.Models;
using RestaurantManagement.DataAccess.Repositories;

namespace RestaurantManagement.Backend.Tests;

[TestClass]
public class GenericRepositoryTests
{
    private RestaurantDbContext _context = null!;
    private GenericRepository<Customer> _repo = null!;

    [TestInitialize]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<RestaurantDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new RestaurantDbContext(options);
        _repo = new GenericRepository<Customer>(_context);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
    
    [TestMethod]
    public async Task AddAsync_ValidEntity_AddsToDatabase()
    {
        // Arrange
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            Name = "Rohit",
            MobileNumber = "9999999999"
        };

        // Act
        await _repo.AddAsync(customer);
        await _repo.SaveChangesAsync();

        // Assert
        var result = await _context.Customers.FindAsync(customer.Id);
        Assert.IsNotNull(result);
        Assert.AreEqual("Rohit", result.Name);
    }

    [TestMethod]
    public async Task GetByIdAsync_ExistingId_ReturnsEntity()
    {
        // Arrange
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            Name = "Amit",
            MobileNumber = "8888888888"
        };

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repo.GetByIdAsync(customer.Id);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(customer.Id, result.Id);
    }

    [TestMethod]
    public async Task GetByIdAsync_InvalidId_ReturnsNull()
    {
        // Act
        var result = await _repo.GetByIdAsync(Guid.NewGuid());

        // Assert
        Assert.IsNull(result);
    }

    [TestMethod]
    public async Task GetAllAsync_IsValid_ReturnsAllEntities()
    {
        // Arrange
        _context.Customers.AddRange(
            new Customer { Id = Guid.NewGuid(), Name = "A", MobileNumber = "1111111111" },
            new Customer { Id = Guid.NewGuid(), Name = "B", MobileNumber = "2222222222" }
        );

        await _context.SaveChangesAsync();

        // Act
        var result = await _repo.GetAllAsync();

        // Assert
        Assert.AreEqual(2, result.Count);
    }

    [TestMethod]
    public async Task Update_ExistingEntity_UpdatesSuccessfully()
    {
        // Arrange
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            Name = "Old Name",
            MobileNumber = "7777777777"
        };

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        // Act
        customer.Name = "New Name";
        await _repo.SaveChangesAsync();

        // Assert
        var updated = await _context.Customers.FindAsync(customer.Id);
        Assert.AreEqual("New Name", updated!.Name);
    }
    [TestMethod]
    public async Task Delete_ExistingEntity_RemovesFromDatabase()
    {
        // Arrange
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            Name = "To Delete",
            MobileNumber = "6666666666"
        };

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        // Act
        _repo.Delete(customer);
        await _repo.SaveChangesAsync();

        // Assert
        var result = await _context.Customers.FindAsync(customer.Id);
        Assert.IsNull(result);
    }
}

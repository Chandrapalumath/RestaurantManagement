using Microsoft.EntityFrameworkCore;
using RestaurantManagement.DataAccess;
using RestaurantManagement.DataAccess.Models;
using RestaurantManagement.DataAccess.Repositories;

namespace RestaurantManagement.Backend.Tests;

[TestClass]
public class CustomerRepositoryTests
{
    private RestaurantDbContext _context = null!;
    private CustomerRepository _repo = null!;

    [TestInitialize]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<RestaurantDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new RestaurantDbContext(options);
        _repo = new CustomerRepository(_context);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [TestMethod]
    public async Task GetByMobileAsync_ReturnsCustomers_StartingWithGivenMobile()
    {
        // Arrange
        var c1 = new Customer
        {
            Id = Guid.NewGuid(),
            Name = "Ram",
            MobileNumber = "9876543210"
        };

        var c2 = new Customer
        {
            Id = Guid.NewGuid(),
            Name = "Shyam",
            MobileNumber = "9876000011"
        };

        var c3 = new Customer
        {
            Id = Guid.NewGuid(),
            Name = "Mohan",
            MobileNumber = "9123456789"
        };

        _context.Customers.AddRange(c1, c2, c3);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repo.GetByMobileAsync("9876");

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Count);

        var numbers = result.Select(x => x.MobileNumber).ToList();
        CollectionAssert.Contains(numbers, "9876543210");
        CollectionAssert.Contains(numbers, "9876000011");
    }

    [TestMethod]
    public async Task GetByMobileAsync_NoMatch_ReturnsEmptyList()
    {
        // Arrage
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            Name = "Aman",
            MobileNumber = "9999999999"
        };

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repo.GetByMobileAsync("888");

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    public async Task GetByMobileAsync_ExactMatch_ReturnsCustomer()
    {
        // Arrange
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            Name = "Ravi",
            MobileNumber = "9123456789"
        };

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repo.GetByMobileAsync("9123456789");

        // Assert
        Assert.AreEqual(1, result!.Count);
        Assert.AreEqual("Ravi", result!.First().Name);
    }
}

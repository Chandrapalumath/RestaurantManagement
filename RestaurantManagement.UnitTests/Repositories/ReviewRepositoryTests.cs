using Microsoft.EntityFrameworkCore;
using RestaurantManagement.DataAccess;
using RestaurantManagement.DataAccess.Models;
using RestaurantManagement.DataAccess.Repositories;

namespace RestaurantManagement.Backend.Tests;

[TestClass]
public class ReviewRepositoryTests
{
    private RestaurantDbContext _context = null!;
    private ReviewRepository _repo = null!;

    [TestInitialize]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<RestaurantDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new RestaurantDbContext(options);
        _repo = new ReviewRepository(_context);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [TestMethod]
    public async Task GetByCustomerIdAsync_ValidCustomerId_ReturnsReviewsOrderedByCreatedAtDesc()
    {
        // Arrange
        var customerId = Guid.NewGuid();

        var reviewOld = new Review
        {
            Id = Guid.NewGuid(),
            CustomerId = customerId,
            Rating = 4,
            Comment = "Good food",
            CreatedAt = DateTime.UtcNow.AddDays(-1)
        };

        var reviewNew = new Review
        {
            Id = Guid.NewGuid(),
            CustomerId = customerId,
            Rating = 5,
            Comment = "Excellent service",
            CreatedAt = DateTime.UtcNow
        };

        var otherCustomerReview = new Review
        {
            Id = Guid.NewGuid(),
            CustomerId = Guid.NewGuid(),
            Rating = 3,
            Comment = "Average"
        };

        _context.Reviews.AddRange(reviewOld, reviewNew, otherCustomerReview);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repo.GetByCustomerIdAsync(customerId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Count);
        Assert.AreEqual(reviewNew.Id, result[0].Id);
        Assert.AreEqual(reviewOld.Id, result[1].Id);
    }
    [TestMethod]
    public async Task GetByCustomerIdAsync_NoReviews_ReturnsEmptyList()
    {
        // Arrange
        var customerId = Guid.NewGuid();

        var otherReview = new Review
        {
            Id = Guid.NewGuid(),
            CustomerId = Guid.NewGuid(),
            Rating = 2
        };

        _context.Reviews.Add(otherReview);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repo.GetByCustomerIdAsync(customerId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    public async Task GetByCustomerIdAsync_SingleReview_ReturnsOne()
    {
        // Arrange
        var customerId = Guid.NewGuid();

        var review = new Review
        {
            Id = Guid.NewGuid(),
            CustomerId = customerId,
            Rating = 5,
            Comment = "Loved it"
        };

        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repo.GetByCustomerIdAsync(customerId);

        // Assert
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual(5, result.First().Rating);
    }
}

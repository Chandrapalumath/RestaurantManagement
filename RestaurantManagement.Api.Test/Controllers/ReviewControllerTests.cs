using Microsoft.AspNetCore.Mvc;
using Moq;
using RestaurantManagement.Api.Controllers;
using RestaurantManagement.Backend.Services.Interfaces;
using RestaurantManagement.Dtos.Reviews;

namespace RestaurantManagement.Api.Tests;

[TestClass]
public class ReviewControllerTests
{
    private Mock<IReviewService> _reviewServiceMock = null!;
    private ReviewController _controller = null!;

    [TestInitialize]
    public void Setup()
    {
        _reviewServiceMock = new Mock<IReviewService>();
        _controller = new ReviewController(_reviewServiceMock.Object);
    }
    [TestMethod]
    public async Task CreateReviewAsync_ValidReview_ReturnsCreatedAtRoute()
    {
        // Arrange
        var dto = new ReviewCreateRequestDto
        {
            CustomerId = Guid.NewGuid(),
            Rating = 4,
            Comment = "Very good service!"
        };

        var createdReview = new ReviewResponseDto
        {
            ReviewId = Guid.NewGuid(),
            CustomerId = dto.CustomerId,
            Rating = dto.Rating,
            Comment = dto.Comment,
            CreatedAt = DateTime.UtcNow
        };

        _reviewServiceMock
            .Setup(s => s.CreateAsync(It.IsAny<ReviewCreateRequestDto>()))
            .ReturnsAsync(createdReview);

        // Act
        var result = await _controller.CreateReviewAsync(dto);

        // Assert
        var created = result as CreatedAtRouteResult;
        Assert.IsNotNull(created);

        Assert.AreEqual("GetReviewById", created.RouteName);
        Assert.IsNotNull(created.RouteValues);
        Assert.AreEqual(createdReview.ReviewId, created.RouteValues["id"]);
    }
    [TestMethod]
    public async Task GetAllReviewsAsync_ReviewsExist_ReturnsOkWithReviews()
    {
        // Arrange
        var reviews = new List<ReviewResponseDto>
        {
            new ReviewResponseDto { ReviewId = Guid.NewGuid(), CustomerId = Guid.NewGuid(), Rating = 5 },
            new ReviewResponseDto { ReviewId = Guid.NewGuid(), CustomerId = Guid.NewGuid(), Rating = 3 }
        };

        _reviewServiceMock
            .Setup(s => s.GetAllAsync())
            .ReturnsAsync(reviews);

        // Act
        var result = await _controller.GetAllReviewsAsync();

        // Assert
        var ok = result as OkObjectResult;
        Assert.IsNotNull(ok);

        var data = ok.Value as List<ReviewResponseDto>;
        Assert.IsNotNull(data);
        Assert.AreEqual(2, data.Count);
    }
    [TestMethod]
    public async Task GetReviewByIdAsync_ReviewExists_ReturnsOk()
    {
        // Arrange
        var reviewId = Guid.NewGuid();

        var review = new ReviewResponseDto
        {
            ReviewId = reviewId,
            CustomerId = Guid.NewGuid(),
            Rating = 4,
            Comment = "Nice!"
        };

        _reviewServiceMock
            .Setup(s => s.GetByIdAsync(reviewId))
            .ReturnsAsync(review);

        // Act
        var result = await _controller.GetReviewByIdAsync(reviewId);

        // Assert
        var ok = result as OkObjectResult;
        Assert.IsNotNull(ok);

        var data = ok.Value as ReviewResponseDto;
        Assert.IsNotNull(data);
        Assert.AreEqual(reviewId, data.ReviewId);
    }
    [TestMethod]
    public async Task GetReviewsByCustomerIdAsync_CustomerHasReviews_ReturnsOkWithReviews()
    {
        // Arrange
        var customerId = Guid.NewGuid();

        var reviews = new List<ReviewResponseDto>
        {
            new ReviewResponseDto { ReviewId = Guid.NewGuid(), CustomerId = customerId, Rating = 5 },
            new ReviewResponseDto { ReviewId = Guid.NewGuid(), CustomerId = customerId, Rating = 4 }
        };

        _reviewServiceMock
            .Setup(s => s.GetByCustomerIdAsync(customerId))
            .ReturnsAsync(reviews);

        // Act
        var result = await _controller.GetReviewsByCustomerIdAsync(customerId);

        // Assert
        var ok = result as OkObjectResult;
        Assert.IsNotNull(ok);

        var data = ok.Value as List<ReviewResponseDto>;
        Assert.IsNotNull(data);
        Assert.AreEqual(2, data.Count);
    }
}

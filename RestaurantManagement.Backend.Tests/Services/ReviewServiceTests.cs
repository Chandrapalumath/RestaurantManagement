using Moq;
using RestaurantManagement.Backend.Exceptions;
using RestaurantManagement.Backend.Services;
using RestaurantManagement.DataAccess.Models;
using RestaurantManagement.DataAccess.Repositories.Interfaces;
using RestaurantManagement.Dtos.Reviews;

namespace RestaurantManagement.Backend.Tests.Services
{
    public class ReviewServiceTests
    {
        private Mock<IReviewRepository> _reviewRepoMock = null!;
        private Mock<ICustomerRepository> _customerRepoMock = null!;
        private ReviewService _service = null!;

        [TestInitialize]
        public void Setup()
        {
            _reviewRepoMock = new Mock<IReviewRepository>();
            _customerRepoMock = new Mock<ICustomerRepository>();

            _service = new ReviewService(_reviewRepoMock.Object, _customerRepoMock.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task CreateAsync_ShouldThrowNotFound_WhenCustomerDoesNotExist()
        {
            // Arrange
            var dto = new ReviewCreateRequestDto
            {
                CustomerId = Guid.NewGuid(),
                Rating = 4,
                Comment = "Nice"
            };

            _customerRepoMock
                .Setup(r => r.GetByIdAsync(dto.CustomerId))
                .ReturnsAsync((Customer?)null);

            // Act + Assert
            try
            {
                await _service.CreateAsync(dto);
            }
            catch (NotFoundException ex)
            {
                Assert.AreEqual("Customer not found.", ex.Message);
                throw;
            }
        }

        [TestMethod]
        public async Task CreateAsync_ShouldCreateReview_SaveAndReturnMappedDto()
        {
            // Arrange
            var customerId = Guid.NewGuid();

            var dto = new ReviewCreateRequestDto
            {
                CustomerId = customerId,
                Rating = 5,
                Comment = "   Excellent Service!   "
            };

            _customerRepoMock
                .Setup(r => r.GetByIdAsync(customerId))
                .ReturnsAsync(new Customer { Id = customerId });

            Review? capturedReview = null;

            _reviewRepoMock
                .Setup(r => r.AddAsync(It.IsAny<Review>()))
                .Callback<Review>(r => capturedReview = r)
                .Returns(Task.CompletedTask);

            _reviewRepoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _service.CreateAsync(dto);

            // Assert - check created review object
            Assert.IsNotNull(capturedReview);
            Assert.AreEqual(customerId, capturedReview.CustomerId);
            Assert.AreEqual(5, capturedReview.Rating);
            Assert.AreEqual("Excellent Service!", capturedReview.Comment); 

            // Ensure CreatedAt is set (recent time)
            Assert.IsTrue((DateTime.UtcNow - capturedReview.CreatedAt).TotalSeconds < 5);

            // Assert - dto mapping
            Assert.AreEqual(capturedReview.Id, result.ReviewId);
            Assert.AreEqual(customerId, result.CustomerId);
            Assert.AreEqual(5, result.Rating);
            Assert.AreEqual("Excellent Service!", result.Comment);
            Assert.AreEqual(capturedReview.CreatedAt, result.CreatedAt);
        }

        [TestMethod]
        public async Task CreateAsync_ShouldAllowNullComment_AndReturnNullCommentInDto()
        {
            // Arrange
            var customerId = Guid.NewGuid();

            var dto = new ReviewCreateRequestDto
            {
                CustomerId = customerId,
                Rating = 3,
                Comment = null
            };

            _customerRepoMock
                .Setup(r => r.GetByIdAsync(customerId))
                .ReturnsAsync(new Customer { Id = customerId });

            Review? capturedReview = null;

            _reviewRepoMock
                .Setup(r => r.AddAsync(It.IsAny<Review>()))
                .Callback<Review>(r => capturedReview = r)
                .Returns(Task.CompletedTask);

            _reviewRepoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _service.CreateAsync(dto);

            // Assert
            Assert.IsNotNull(capturedReview);
            Assert.IsNull(capturedReview.Comment);
            Assert.AreEqual(capturedReview.Comment, result.Comment);
        }
        
        [TestMethod]
        public async Task GetAllAsync_ShouldReturnMappedList()
        {
            // Arrange
            var reviews = new List<Review>
            {
                new Review
                {
                    Id = Guid.NewGuid(),
                    CustomerId = Guid.NewGuid(),
                    Rating = 4,
                    Comment = "Good",
                    CreatedAt = DateTime.UtcNow.AddDays(-1)
                },
                new Review
                {
                    Id = Guid.NewGuid(),
                    CustomerId = Guid.NewGuid(),
                    Rating = 2,
                    Comment = "Bad",
                    CreatedAt = DateTime.UtcNow.AddDays(-2)
                }
            };

            _reviewRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(reviews);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(reviews[0].Id, result[0].ReviewId);
            Assert.AreEqual(reviews[0].CustomerId, result[0].CustomerId);
            Assert.AreEqual(reviews[0].Rating, result[0].Rating);
            Assert.AreEqual(reviews[0].Comment, result[0].Comment);
            Assert.AreEqual(reviews[0].CreatedAt, result[0].CreatedAt);
        }

        [TestMethod]
        public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoReviews()
        {
            // Arrange
            _reviewRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Review>());

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }
        
        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task GetByIdAsync_ShouldThrowNotFound_WhenReviewNotExists()
        {
            // Arrange
            var id = Guid.NewGuid();

            _reviewRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Review?)null);

            // Act + Assert
            try
            {
                await _service.GetByIdAsync(id);
            }
            catch (NotFoundException ex)
            {
                Assert.AreEqual("Review not found.", ex.Message);
            }
        }

        [TestMethod]
        public async Task GetByIdAsync_ShouldReturnMappedDto_WhenReviewExists()
        {
            // Arrange
            var review = new Review
            {
                Id = Guid.NewGuid(),
                CustomerId = Guid.NewGuid(),
                Rating = 5,
                Comment = "Great",
                CreatedAt = DateTime.UtcNow.AddHours(-2)
            };

            _reviewRepoMock.Setup(r => r.GetByIdAsync(review.Id)).ReturnsAsync(review);

            // Act
            var result = await _service.GetByIdAsync(review.Id);

            // Assert
            Assert.AreEqual(review.Id, result.ReviewId);
            Assert.AreEqual(review.CustomerId, result.CustomerId);
            Assert.AreEqual(review.Rating, result.Rating);
            Assert.AreEqual(review.Comment, result.Comment);
            Assert.AreEqual(review.CreatedAt, result.CreatedAt);
        }
        
        [TestMethod]
        public async Task GetByCustomerIdAsync_ShouldReturnMappedList()
        {
            // Arrange
            var customerId = Guid.NewGuid();

            var reviews = new List<Review>
            {
                new Review
                {
                    Id = Guid.NewGuid(),
                    CustomerId = customerId,
                    Rating = 4,
                    Comment = "Nice",
                    CreatedAt = DateTime.UtcNow.AddDays(-1)
                },
                new Review
                {
                    Id = Guid.NewGuid(),
                    CustomerId = customerId,
                    Rating = 1,
                    Comment = "Worst",
                    CreatedAt = DateTime.UtcNow.AddDays(-5)
                }
            };

            _reviewRepoMock.Setup(r => r.GetByCustomerIdAsync(customerId)).ReturnsAsync(reviews);

            // Act
            var result = await _service.GetByCustomerIdAsync(customerId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.All(r => r.CustomerId == customerId));
            Assert.AreEqual(reviews[0].Id, result[0].ReviewId);
            Assert.AreEqual(reviews[1].Id, result[1].ReviewId);
        }

        [TestMethod]
        public async Task GetByCustomerIdAsync_ShouldReturnEmptyList_WhenNoReviewsForCustomer()
        {
            // Arrange
            var customerId = Guid.NewGuid();

            _reviewRepoMock.Setup(r => r.GetByCustomerIdAsync(customerId))
                           .ReturnsAsync(new List<Review>());

            // Act
            var result = await _service.GetByCustomerIdAsync(customerId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }
    }
}
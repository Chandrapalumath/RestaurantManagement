using Moq;
using RestaurantManagement.Backend.Exceptions;
using RestaurantManagement.Backend.Services;
using RestaurantManagement.DataAccess.Models;
using RestaurantManagement.DataAccess.Repositories.Interfaces;
using RestaurantManagement.Dtos.Settings;

namespace RestaurantManagement.Backend.Tests.Services
{
    [TestClass]
    public class SettingsServiceTests
    {
        private Mock<ISettingsRepository> _settingsRepoMock = null!;
        private Mock<IUserRepository> _userRepoMock = null!;
        private SettingsService _service = null!;

        [TestInitialize]
        public void Setup()
        {
            _settingsRepoMock = new Mock<ISettingsRepository>();
            _userRepoMock = new Mock<IUserRepository>();
            _service = new SettingsService(_settingsRepoMock.Object, _userRepoMock.Object);
        }
        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task GetSettingsAsync_SettingsDoNotExist_ThrowsNotFoundException()
        {
            // Arrange
            _settingsRepoMock
                .Setup(r => r.GetSettingsAsync())
                .ReturnsAsync((RestaurantSettings?)null);

            // Act + Assert
            try
            {
                await  _service.GetSettingsAsync();
            }
            catch (NotFoundException ex)
            {
                Assert.AreEqual("Settings not found.", ex.Message);
                throw;
            }
        }
        [TestMethod]
        public async Task GetSettingsAsync_SettingsExist_ReturnsSettingsDto()
        {
            // Arrange
            var settings = new RestaurantSettings
            {
                Id = Guid.NewGuid(),
                TaxPercent = 5,
                DiscountPercent = 10,
                UpdatedAt = DateTime.UtcNow.AddDays(-1)
            };

            _settingsRepoMock
                .Setup(r => r.GetSettingsAsync())
                .ReturnsAsync(settings);

            // Act
            var result = await _service.GetSettingsAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(settings.TaxPercent, result.TaxPercent);
            Assert.AreEqual(settings.DiscountPercent, result.DiscountPercent);
            Assert.AreEqual(settings.UpdatedAt, result.UpdatedAt);
        }
        [TestMethod]
        public async Task UpdateSettingsAsync_NoExistingSettings_CreatesAndReturnsSettingsDto()
        {
            // Arrange
            _settingsRepoMock
                .Setup(r => r.GetSettingsAsync())
                .ReturnsAsync((RestaurantSettings?)null);

            var dto = new SettingsUpdateRequestDto
            {
                TaxPercent = 6,
                DiscountPercent = 12
            };

            RestaurantSettings? createdSettings = null;

            _settingsRepoMock
                .Setup(r => r.AddAsync(It.IsAny<RestaurantSettings>()))
                .Callback<RestaurantSettings>(s => createdSettings = s)
                .Returns(Task.CompletedTask);

            _settingsRepoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            await _service.UpdateSettingsAsync(dto);

            // Assert
            Assert.IsNotNull(createdSettings);
            Assert.AreNotEqual(Guid.Empty, createdSettings!.Id);
        }
        [TestMethod]
        public async Task UpdateSettingsAsync_TaxPercentOnlyProvided_UpdatesTaxAndReturnsDto()
        {
            // Arrange
            var settings = new RestaurantSettings
            {
                Id = Guid.NewGuid(),
                TaxPercent = 5,
                DiscountPercent = 10,
                UpdatedAt = DateTime.UtcNow.AddDays(-2)
            };

            _settingsRepoMock.Setup(r => r.GetSettingsAsync()).ReturnsAsync(settings);
            _settingsRepoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            var dto = new SettingsUpdateRequestDto
            {
                TaxPercent = 8,
                DiscountPercent = null
            };

            var oldUpdatedAt = settings.UpdatedAt;

            // Act
            await _service.UpdateSettingsAsync(dto);

            // Assert
            Assert.AreEqual(8, settings.TaxPercent);
            Assert.AreEqual(10, settings.DiscountPercent);
            Assert.IsTrue(settings.UpdatedAt > oldUpdatedAt);
        }

        [TestMethod]
        public async Task UpdateSettingsAsync_DiscountPercentOnlyProvided_UpdatesDiscountAndReturnsDto()
        {
            // Arrange
            var settings = new RestaurantSettings
            {
                Id = Guid.NewGuid(),
                TaxPercent = 5,
                DiscountPercent = 10,
                UpdatedAt = DateTime.UtcNow.AddDays(-2)
            };

            _settingsRepoMock.Setup(r => r.GetSettingsAsync()).ReturnsAsync(settings);
            _settingsRepoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            var dto = new SettingsUpdateRequestDto
            {
                TaxPercent = null,
                DiscountPercent = 20
            };

            var oldUpdatedAt = settings.UpdatedAt;

            // Act
            await _service.UpdateSettingsAsync(dto);

            // Assert
            Assert.AreEqual(5, settings.TaxPercent);
            Assert.AreEqual(20, settings.DiscountPercent);
            Assert.IsTrue(settings.UpdatedAt > oldUpdatedAt);
        }

        [TestMethod]
        public async Task UpdateSettingsAsync_AllFieldsProvided_UpdatesAllAndReturnsDto()
        {
            // Arrange
            var settings = new RestaurantSettings
            {
                Id = Guid.NewGuid(),
                TaxPercent = 3,
                DiscountPercent = 5,
                UpdatedAt = DateTime.UtcNow.AddDays(-5)
            };

            _settingsRepoMock.Setup(r => r.GetSettingsAsync()).ReturnsAsync(settings);
            _settingsRepoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            var dto = new SettingsUpdateRequestDto
            {
                TaxPercent = 9,
                DiscountPercent = 30
            };

            var oldUpdatedAt = settings.UpdatedAt;

            // Act
            await _service.UpdateSettingsAsync(dto);

            // Assert
            Assert.AreEqual(9, settings.TaxPercent);
            Assert.AreEqual(30, settings.DiscountPercent);
            Assert.IsTrue(settings.UpdatedAt > oldUpdatedAt);
        }

        [TestMethod]
        public async Task UpdateSettingsAsync_NoValuesProvided_OnlyUpdatesTimestampAndReturnsDto()
        {
            // Arrange
            var settings = new RestaurantSettings
            {
                Id = Guid.NewGuid(),
                TaxPercent = 4,
                DiscountPercent = 15,
                UpdatedAt = DateTime.UtcNow.AddDays(-10)
            };

            _settingsRepoMock.Setup(r => r.GetSettingsAsync()).ReturnsAsync(settings);
            _settingsRepoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            var dto = new SettingsUpdateRequestDto
            {
                TaxPercent = null,
                DiscountPercent = null
            };

            var oldUpdatedAt = settings.UpdatedAt;

            // Act
            await _service.UpdateSettingsAsync(dto);

            // Assert (values unchanged)
            Assert.AreEqual(4, settings.TaxPercent);
            Assert.AreEqual(15, settings.DiscountPercent);

            // updatedAt must change
            Assert.IsTrue(settings.UpdatedAt > oldUpdatedAt);
        }
    }
}
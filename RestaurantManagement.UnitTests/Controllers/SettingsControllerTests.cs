using Microsoft.AspNetCore.Mvc;
using Moq;
using RestaurantManagement.Api.Controllers;
using RestaurantManagement.Backend.Services.Interfaces;
using RestaurantManagement.Dtos.Settings;
namespace RestaurantManagement.Api.Tests;

[TestClass]
public class SettingsControllerTests
{
    private Mock<ISettingsService> _settingsServiceMock = null!;
    private SettingsController _controller = null!;

    [TestInitialize]
    public void Setup()
    {
        _settingsServiceMock = new Mock<ISettingsService>();
        _controller = new SettingsController(_settingsServiceMock.Object);
    }

    [TestMethod]
    public async Task GetSettingsAsync_SettingsAreConfigured_ReturnsOk()
    {
        // Arrange
        var settings = new SettingsResponseDto
        {
            TaxPercent = 5,
            DiscountPercent = 10,
            UpdatedAt = DateTime.UtcNow
        };

        _settingsServiceMock
            .Setup(s => s.GetSettingsAsync())
            .ReturnsAsync(settings);

        // Act
        var result = await _controller.GetSettingsAsync();

        // Assert
        var ok = result as OkObjectResult;
        Assert.IsNotNull(ok);

        var data = ok.Value as SettingsResponseDto;
        Assert.IsNotNull(data);
        Assert.AreEqual(5, data.TaxPercent);
        Assert.AreEqual(10, data.DiscountPercent);
    }

    [TestMethod]
    public async Task UpdateSettingsAsync_ValidSettings_ReturnsNoContent()
    {
        // Arrange
        var dto = new SettingsUpdateRequestDto
        {
            TaxPercent = 12,
            DiscountPercent = 5
        };

        _settingsServiceMock
            .Setup(s => s.UpdateSettingsAsync(It.IsAny<SettingsUpdateRequestDto>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdateSettingsAsync(dto);

        // Assert
        Assert.IsInstanceOfType(result, typeof(NoContentResult));
    }

}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RestaurantManagement.Api.Controllers;
using RestaurantManagement.Api.Middlewares;
using RestaurantManagement.Backend.Services;
using RestaurantManagement.Backend.Services.Interfaces;
using RestaurantManagement.Dtos.Settings;
using RestaurantManagement.Dtos.Users;
using RestaurantManagement.Models.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Api.Tests;

[TestClass]
public class AdminControllerTests
{
    private Mock<IUserService> _userServiceMock = null!;
    private Mock<ISettingsService> _settingsServiceMock = null!;
    private AdminController _adminController = null!;

    [TestInitialize]
    public void Setup()
    {
        _userServiceMock = new Mock<IUserService>();
        _settingsServiceMock = new Mock<ISettingsService>();
        _adminController = new AdminController(_userServiceMock.Object, _settingsServiceMock.Object);
    }

    [TestMethod]
    public async Task CreateUserAsync_ValidModel_ReturnsCreatedAtRoute()
    {
        var dto = new CreateUserRequestDto
        {
            FullName = "Chandrapal",
            Email = "david@gmail.com",
            MobileNumber = "9876543210",
            Password = "Test@123",
            Role = UserRole.Waiter
        };

        var createdUser = new UserResponseDto
        {
            Id = Guid.NewGuid(),
            FullName = dto.FullName,
            Email = dto.Email,
            MobileNumber = dto.MobileNumber,
            Role = dto.Role,
            IsActive = true
        };
        _userServiceMock
            .Setup(s => s.CreateUserAsync(dto))
            .ReturnsAsync(createdUser);

        var result = await _adminController.CreateUserAsync(dto);
        Assert.IsNotNull(result);

        var createdAt = result as CreatedAtRouteResult;
        Assert.IsNotNull(createdAt);
        
        Assert.AreEqual("GetUserByIdAsync", createdAt.RouteName);
        Assert.IsNotNull(createdAt.RouteValues);
        Assert.AreEqual(createdUser.Id, createdAt.RouteValues["id"]);
    }

    [TestMethod]
    public async Task GetAllUsersAsync_WhenUsersExist_ReturnsOkWithUsers()
    {
        // Arrange
        var users = new List<UserResponseDto>
            {
                new UserResponseDto { Id = Guid.NewGuid(), FullName = "Sam", Email="sam@gmail.com", MobileNumber="9999999999" },
                new UserResponseDto { Id = Guid.NewGuid(), FullName = "Tom", Email="tom@gmail.com", MobileNumber="8888888888" }
            };

        _userServiceMock
            .Setup(s => s.GetAllUsersAsync())
            .ReturnsAsync(users);

        // Act
        var result = await _adminController.GetAllUsersAsync();

        // Assert
        Assert.IsNotNull(result);

        var ok = result as OkObjectResult;
        Assert.IsNotNull(ok);

        var data = ok.Value as IEnumerable<UserResponseDto>;
        Assert.IsNotNull(data);
        Assert.AreEqual(2, data.Count());
    }

    [TestMethod]
    public async Task GetUserByIdAsync_UserExists_ReturnsOk()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var user = new UserResponseDto
        {
            Id = userId,
            FullName = "Alex",
            Email = "alex@gmail.com",
            MobileNumber = "7777777777",
            Role = UserRole.Waiter,
            IsActive = true
        };

        _userServiceMock
            .Setup(s => s.GetUserByIdAsync(userId))
            .ReturnsAsync(user);

        // Act
        var result = await _adminController.GetUserByIdAsync(userId);

        // Assert

        var ok = result as OkObjectResult;
        Assert.IsNotNull(ok);

        var data = ok.Value as UserResponseDto;
        Assert.IsNotNull(data);
        Assert.AreEqual(userId, data.Id);
    }

    [TestMethod]
    public async Task UpdateUserAsync_ValidUser_ReturnsNoContent()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var dto = new UserUpdateRequestDto
        {
            FullName = "Updated Name",
            MobileNumber = "1234567890",
            Email = "UpdatedEmail@gmail.com",
            IsActive = true,
            Role = UserRole.Waiter
        };

        _userServiceMock
            .Setup(s => s.UpdateUserAsync(userId, It.IsAny<UserUpdateRequestDto>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _adminController.UpdateUserAsync(userId, dto);

        // Assert
        Assert.IsInstanceOfType(result, typeof(NoContentResult));
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
        var result = await _adminController.GetSettingsAsync();

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
        var result = await _adminController.UpdateSettingsAsync(dto);

        // Assert
        Assert.IsInstanceOfType(result, typeof(NoContentResult));
    }
}

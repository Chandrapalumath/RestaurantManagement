using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RestaurantManagement.Api.Controllers;
using RestaurantManagement.Backend.Exceptions;
using RestaurantManagement.Backend.Services.Interfaces;
using RestaurantManagement.Dtos.Settings;
using RestaurantManagement.Dtos.Users;
using RestaurantManagement.Models.Common.Enums;
using System.Security.Claims;

namespace RestaurantManagement.Api.Tests;

[TestClass]
public class UserControllerTests
{
    private Mock<IUserService> _userServiceMock = null!;
    private Mock<ISettingsService> _settingsServiceMock = null!;
    private UserController _controller = null!;

    [TestInitialize]
    public void Setup()
    {
        _userServiceMock = new Mock<IUserService>();
        _settingsServiceMock = new Mock<ISettingsService>();
        _controller = new UserController(_userServiceMock.Object, _settingsServiceMock.Object);
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

        var result = await _controller.CreateUserAsync(dto);
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
        var result = await _controller.GetAllUsersAsync();

        // Assert
        Assert.IsNotNull(result);

        var ok = result as OkObjectResult;
        Assert.IsNotNull(ok);

        var data = ok.Value as IEnumerable<UserResponseDto>;
        Assert.IsNotNull(data);
        Assert.AreEqual(2, data.Count());
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
        var result = await _controller.UpdateUserAsync(userId, dto);

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
    private void SetUserContext(Guid userId, bool isAdmin)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Role, isAdmin ? "Admin" : "User")
        };

        var identity = new ClaimsIdentity(claims, "TestAuth");
        var principal = new ClaimsPrincipal(identity);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = principal
            }
        };
    }

    [TestMethod]
    public async Task GetUserByIdAsync_ShouldReturnOk_WhenAdminRequestsAnyUser()
    {
        // Arrange
        var adminId = Guid.NewGuid();
        var targetUserId = Guid.NewGuid();

        SetUserContext(adminId, isAdmin: true);

        var responseDto = new UserResponseDto
        {
            Id = targetUserId,
            FullName = "Test User",
            Email = "test@test.com",
            IsActive = true
        };

        _userServiceMock
            .Setup(s => s.GetUserByIdAsync(targetUserId, true, adminId))
            .ReturnsAsync(responseDto);

        // Act
        var result = await _controller.GetUserByIdAsync(targetUserId);

        // Assert
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);

        var data = okResult.Value as UserResponseDto;
        Assert.IsNotNull(data);
        Assert.AreEqual(targetUserId, data.Id);
    }

    [TestMethod]
    public async Task GetUserByIdAsync_ShouldReturnOk_WhenUserRequestsOwnProfile()
    {
        // Arrange
        var userId = Guid.NewGuid();

        SetUserContext(userId, isAdmin: false);

        var responseDto = new UserResponseDto
        {
            Id = userId,
            FullName = "Self User",
            Email = "self@test.com",
            IsActive = true
        };

        _userServiceMock
            .Setup(s => s.GetUserByIdAsync(userId, false, userId))
            .ReturnsAsync(responseDto);

        // Act
        var result = await _controller.GetUserByIdAsync(userId);

        // Assert
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);

        var data = okResult.Value as UserResponseDto;
        Assert.IsNotNull(data);
        Assert.AreEqual(userId, data.Id);
    }

    [TestMethod]
    [ExpectedException(typeof(ForbiddenException))]
    public async Task GetUserByIdAsync_ShouldThrowForbidden_WhenUserAccessesOtherUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var otherUserId = Guid.NewGuid();

        SetUserContext(userId, isAdmin: false);

        _userServiceMock
            .Setup(s => s.GetUserByIdAsync(otherUserId, false, userId))
            .ThrowsAsync(new ForbiddenException("You are not allowed to view this user."));

        // Act + Assert
        try
        {
            await _controller.GetUserByIdAsync(otherUserId);
        }
        catch (ForbiddenException ex)
        {
            throw;
        }
    }

    [TestMethod]
    public async Task GetUserByIdAsync_ShouldThrowNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var adminId = Guid.NewGuid();
        var targetUserId = Guid.NewGuid();

        SetUserContext(adminId, isAdmin: true);

        _userServiceMock
            .Setup(s => s.GetUserByIdAsync(targetUserId, true, adminId))
            .ThrowsAsync(new NotFoundException("User not found or Invalid ID."));

        // Act + Assert
        await Assert.ThrowsExceptionAsync<NotFoundException>(() =>
            _controller.GetUserByIdAsync(targetUserId));
    }
}

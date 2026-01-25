using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RestaurantManagement.Api.Controllers;
using RestaurantManagement.Backend.Exceptions;
using RestaurantManagement.Backend.Services.Interfaces;
using RestaurantManagement.Dtos.Authentication;
using System.Security.Claims;

namespace RestaurantManagement.UnitTests;

[TestClass]
public class AuthControllerTests
{
    private Mock<IAuthService> _authServiceMock = null!;
    private AuthController _controller = null!;

    [TestInitialize]
    public void Setup()
    {
        _authServiceMock = new Mock<IAuthService>();
        _controller = new AuthController(_authServiceMock.Object);
        var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, "Admin")
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
    public async Task Login_ValidCredentials_ReturnsOk()
    {
        // Arrange
        var dto = new LoginRequestDto
        {
            Email = "test@test.com",
            Password = "password"
        };

        var response = new AuthResponseDto
        {
            UserId = Guid.NewGuid(),
            FullName = "Test User",
            Role = "Admin",
            Token = "FAKE_JWT_TOKEN"
        };

        _authServiceMock
            .Setup(s => s.LoginAsync(dto))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.Login(dto);

        // Assert
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);

        var data = okResult.Value as AuthResponseDto;
        Assert.IsNotNull(data);
        Assert.AreEqual(response.UserId, data.UserId);
        Assert.AreEqual("FAKE_JWT_TOKEN", data.Token);
    }

    [TestMethod]
    public async Task Login_InvalidCredentials_ThrowsUnauthorizedException()
    {
        // Arrange
        var dto = new LoginRequestDto
        {
            Email = "wrong@test.com",
            Password = "wrong"
        };

        _authServiceMock
            .Setup(s => s.LoginAsync(dto))
            .ThrowsAsync(new UnauthorizedException("Invalid email or password."));

        // Act + Assert
        await Assert.ThrowsExceptionAsync<UnauthorizedException>(() =>
            _controller.Login(dto));
    }
    [TestMethod]
    public async Task Login_ServiceThrowsException_ThrowsException()
    {
        // Arrange
        var dto = new LoginRequestDto
        {
            Email = "error@test.com",
            Password = "password"
        };

        _authServiceMock
            .Setup(s => s.LoginAsync(dto))
            .ThrowsAsync(new Exception("Unexpected error"));

        // Act + Assert
        await Assert.ThrowsExceptionAsync<Exception>(() =>
            _controller.Login(dto));
    }
    [TestMethod]
    public async Task ChangePassword_ValidRequest_ReturnsNoContent()
    {
        // Arrange
        var dto = new ChangePasswordRequestDto
        {
            CurrentPassword = "Old@123",
            NewPassword = "New@123"
        };

        _authServiceMock
            .Setup(s => s.ChangePasswordAsync(It.IsAny<Guid>(), dto))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.ChangePassword(dto);

        // Assert
        Assert.IsInstanceOfType(result, typeof(NoContentResult));
    }
    [TestMethod]
    public async Task ChangePassword_UserNotFound_ThrowsUnauthorizedException()
    {
        // Arrange
        var dto = new ChangePasswordRequestDto
        {
            CurrentPassword = "old",
            NewPassword = "new"
        };

        _authServiceMock
            .Setup(s => s.ChangePasswordAsync(It.IsAny<Guid>(), dto))
            .ThrowsAsync(new UnauthorizedException("User not found."));

        // Act + Assert
        await Assert.ThrowsExceptionAsync<UnauthorizedException>(() =>
            _controller.ChangePassword(dto));
    }
    [TestMethod]
    public async Task ChangePassword_InvalidPassword_ThrowsBadRequestException()
    {
        // Arrange
        var dto = new ChangePasswordRequestDto
        {
            CurrentPassword = "wrong",
            NewPassword = "new"
        };

        _authServiceMock
            .Setup(s => s.ChangePasswordAsync(It.IsAny<Guid>(), dto))
            .ThrowsAsync(new BadRequestException("Current password is incorrect."));

        // Act + Assert
        await Assert.ThrowsExceptionAsync<BadRequestException>(() =>
            _controller.ChangePassword(dto));
    }
}

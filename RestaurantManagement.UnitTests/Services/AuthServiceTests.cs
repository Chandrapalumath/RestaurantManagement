using Moq;
using RestaurantManagement.Backend.Exceptions;
using RestaurantManagement.Backend.Helper;
using RestaurantManagement.Backend.Services;
using RestaurantManagement.Backend.Utils;
using RestaurantManagement.DataAccess.Models;
using RestaurantManagement.DataAccess.Repositories.Interfaces;
using RestaurantManagement.Dtos.Authentication;
using RestaurantManagement.Models.Common.Enums;

namespace RestaurantManagement.UnitTests;

[TestClass]
public class AuthServiceTests
{
    private Mock<IUserRepository> _userRepoMock = null!;
    private Mock<IJwtTokenGenerator> _jwtMock = null!;
    private AuthService _service = null!;

    [TestInitialize]
    public void Setup()
    {
        _userRepoMock = new Mock<IUserRepository>();
        _jwtMock = new Mock<IJwtTokenGenerator>();
        _service = new AuthService(_userRepoMock.Object, _jwtMock.Object);
    }

    [TestMethod]
    [ExpectedException(typeof(UnauthorizedException))]
    public async Task LoginAsync_ShouldThrowUnauthorized_WhenUserNotFound()
    {
        // Arrange
        var dto = new LoginRequestDto
        {
            Email = "test@test.com",
            Password = "password"
        };

        _userRepoMock
            .Setup(r => r.GetByEmailAsync("test@test.com"))
            .ReturnsAsync((User?)null);

        // Act + Assert
        try
        {
            await _service.LoginAsync(dto);
        }
        catch (UnauthorizedException ex)
        {
            Assert.AreEqual("Invalid email or password.", ex.Message);
            throw;
        }
    }

    [TestMethod]
    [ExpectedException(typeof(UnauthorizedException))]
    public async Task LoginAsync_ShouldThrowUnauthorized_WhenUserIsInactive()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "test@test.com",
            Password = PasswordHasher.Hash("password"),
            IsActive = false
        };

        var dto = new LoginRequestDto
        {
            Email = " test@test.com ",
            Password = "password"
        };

        _userRepoMock
            .Setup(r => r.GetByEmailAsync("test@test.com"))
            .ReturnsAsync(user);

        // Act + Assert
        try
        {
            await _service.LoginAsync(dto);
        }
        catch (UnauthorizedException ex)
        {
            Assert.AreEqual("User is deactivated.", ex.Message);
            throw;
        }
    }

    [TestMethod]
    [ExpectedException(typeof(UnauthorizedException))]
    public async Task LoginAsync_ShouldThrowUnauthorized_WhenPasswordInvalid()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "test@test.com",
            Password = PasswordHasher.Hash("correct-password"),
            IsActive = true
        };

        var dto = new LoginRequestDto
        {
            Email = "test@test.com",
            Password = "wrong-password"
        };

        _userRepoMock
            .Setup(r => r.GetByEmailAsync("test@test.com"))
            .ReturnsAsync(user);

        // Act + Assert
        try
        {
            await _service.LoginAsync(dto);
        }
        catch(UnauthorizedException ex)
        {
            Assert.AreEqual("Invalid password.", ex.Message);
            throw;
        }
    }

    [TestMethod]
    public async Task LoginAsync_ShouldReturnToken_WhenCredentialsValid()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "Chandrapal",
            Email = "test@test.com",
            Password = PasswordHasher.Hash("password"),
            Role = UserRole.Admin,
            IsActive = true
        };

        var dto = new LoginRequestDto
        {
            Email = " TEST@test.com ",
            Password = "password"
        };

        _userRepoMock
            .Setup(r => r.GetByEmailAsync("test@test.com"))
            .ReturnsAsync(user);

        _jwtMock
            .Setup(j => j.GenerateToken(user))
            .Returns("FAKE_JWT_TOKEN");

        // Act
        var result = await _service.LoginAsync(dto);
        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("Chandrapal", result.FullName);
        Assert.AreEqual(UserRole.Admin.ToString(), result.Role);
        Assert.AreEqual("FAKE_JWT_TOKEN", result.Token);
    }
    [TestMethod]
    public async Task ChangePasswordAsync_ValidPassword_UpdatesPassword()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var oldPassword = "Old@123";
        var newPassword = "New@456";

        var user = new User
        {
            Id = userId,
            Password = PasswordHasher.Hash(oldPassword),
            IsActive = true,
            Role = UserRole.Waiter
        };

        var dto = new ChangePasswordRequestDto
        {
            CurrentPassword = oldPassword,
            NewPassword = newPassword
        };

        _userRepoMock.Setup(r => r.GetByIdAsync(userId))
                     .ReturnsAsync(user);

        _userRepoMock.Setup(r => r.SaveChangesAsync())
                     .Returns(Task.CompletedTask);

        // Act
        await _service.ChangePasswordAsync(userId, dto);

        // Assert
        Assert.IsTrue(
            PasswordHasher.Verify(newPassword, user.Password),
            "Password should be updated and hashed"
        );

        _userRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(UnauthorizedException))]
    public async Task ChangePasswordAsync_UserNotFound_ThrowsUnauthorizedException()
    {
        // Arrange
        _userRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                     .ReturnsAsync((User?)null);

        var dto = new ChangePasswordRequestDto
        {
            CurrentPassword = "old",
            NewPassword = "new"
        };

        // Act + Assert
        try
        {
            await _service.ChangePasswordAsync(Guid.NewGuid(), dto);
        }
        catch (UnauthorizedException ex)
        {
            Assert.AreEqual("User not found.", ex.Message);
            throw;
        }
    }

    [TestMethod]
    [ExpectedException(typeof(BadRequestException))]
    public async Task ChangePasswordAsync_WrongCurrentPassword_ThrowsBadRequestException()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var user = new User
        {
            Id = userId,
            Password = PasswordHasher.Hash("CorrectPassword")
        };

        _userRepoMock.Setup(r => r.GetByIdAsync(userId))
                     .ReturnsAsync(user);

        var dto = new ChangePasswordRequestDto
        {
            CurrentPassword = "WrongPassword",
            NewPassword = "NewPassword"
        };

        // Act + Assert
        try
        {
            await _service.ChangePasswordAsync(userId, dto);
        }
        catch (BadRequestException ex)
        {
            Assert.AreEqual("Current password is incorrect.", ex.Message);
            throw;
        }
    }

    [TestMethod]
    [ExpectedException(typeof(BadRequestException))]
    public async Task ChangePasswordAsync_SamePassword_ThrowsBadRequestException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var password = "SamePassword";

        var user = new User
        {
            Id = userId,
            Password = PasswordHasher.Hash(password)
        };

        _userRepoMock.Setup(r => r.GetByIdAsync(userId))
                     .ReturnsAsync(user);

        var dto = new ChangePasswordRequestDto
        {
            CurrentPassword = password,
            NewPassword = password
        };

        // Act + Assert
        try
        {
            await _service.ChangePasswordAsync(userId, dto);
        }
        catch (BadRequestException ex)
        {
            Assert.AreEqual("New password cannot be same as current password.", ex.Message);
            throw;
        }
    }
}

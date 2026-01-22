using Moq;
using RestaurantManagement.Backend.Exceptions;
using RestaurantManagement.Backend.Services;
using RestaurantManagement.DataAccess.Models;
using RestaurantManagement.DataAccess.Repositories.Interfaces;
using RestaurantManagement.Dtos.Users;
using RestaurantManagement.Models.Common.Enums;

namespace RestaurantManagement.Backend.Tests.Services
{
    [TestClass]
    public class UserServiceTests
    {
        private Mock<IUserRepository> _userRepoMock = null!;
        private UserService _service = null!;

        [TestInitialize]
        public void Setup()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _service = new UserService(_userRepoMock.Object);
        }
        [TestMethod]
        [ExpectedException(typeof(ConflictException))]
        public async Task CreateUserAsync_ShouldThrowConflict_WhenEmailAlreadyExists()
        {
            // Arrange
            var dto = new CreateUserRequestDto
            {
                Email = " test@abc.com ",
                FullName = "Test User",
                MobileNumber = "9999999999",
                Password = "password123",
                Role = UserRole.Waiter
            };

            _userRepoMock
                .Setup(r => r.GetByEmailAsync("test@abc.com"))
                .ReturnsAsync(new User());

            // Act + Assert
            try
            {
                await _service.CreateUserAsync(dto);
            }
            catch (ConflictException ex)
            {
                Assert.AreEqual("Email already exists.", ex.Message);
                throw;
            }
        }
        [TestMethod]
        public async Task CreateUserAsync_ShouldCreateUser_Save_AndReturnMappedDto()
        {
            // Arrange
            var dto = new CreateUserRequestDto
            {
                Email = "  TEST@ABC.COM  ",
                FullName = "  Chandrapal Singh  ",
                MobileNumber = "  9876543210  ",
                Password = "Pass@123",
                Role = UserRole.Chef
            };

            _userRepoMock
                .Setup(r => r.GetByEmailAsync("test@abc.com"))
                .ReturnsAsync((User?)null);

            User? capturedUser = null;

            _userRepoMock
                .Setup(r => r.AddAsync(It.IsAny<User>()))
                .Callback<User>(u => capturedUser = u)
                .Returns(Task.CompletedTask);

            _userRepoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _service.CreateUserAsync(dto);

            // Assert - user object created correctly
            Assert.IsNotNull(capturedUser);

            Assert.AreEqual("test@abc.com", capturedUser.Email);             
            Assert.AreEqual("Chandrapal Singh", capturedUser.Name);       
            Assert.AreEqual("9876543210", capturedUser.MobileNumber);        
            Assert.AreEqual(UserRole.Chef, capturedUser.Role);
            Assert.IsTrue(capturedUser.IsActive);

            Assert.AreNotEqual(dto.Password, capturedUser.Password);
            Assert.IsFalse(string.IsNullOrWhiteSpace(capturedUser.Password));

            // Assert - response dto mapping
            Assert.AreEqual(capturedUser.Id, result.Id);
            Assert.AreEqual("Chandrapal Singh", result.FullName);
            Assert.AreEqual("9876543210", result.MobileNumber);
            Assert.AreEqual("test@abc.com", result.Email);
            Assert.AreEqual(UserRole.Chef, result.Role);
            Assert.IsTrue(result.IsActive);
        }
        [TestMethod]
        public async Task GetAllUsersAsync_ShouldReturnMappedList()
        {
            // Arrange
            var users = new List<User>
            {
                new User
                {
                    Id = Guid.NewGuid(),
                    Name = "User1",
                    Email = "user1@test.com",
                    MobileNumber = "111",
                    Role = UserRole.Waiter,
                    IsActive = true
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Name = "User2",
                    Email = "user2@test.com",
                    MobileNumber = "222",
                    Role = UserRole.Chef,
                    IsActive = false
                }
            };

            _userRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(users);

            // Act
            var result = await _service.GetAllUsersAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(users[0].Id, result[0].Id);
            Assert.AreEqual("User1", result[0].FullName);
            Assert.AreEqual("user1@test.com", result[0].Email);
            Assert.AreEqual("111", result[0].MobileNumber);
            Assert.AreEqual(UserRole.Waiter, result[0].Role);
            Assert.IsTrue(result[0].IsActive);
            Assert.AreEqual(users[1].Id, result[1].Id);
            Assert.AreEqual("User2", result[1].FullName);
            Assert.AreEqual(UserRole.Chef, result[1].Role);
            Assert.IsFalse(result[1].IsActive);
        }
        [TestMethod]
        public async Task GetAllUsersAsync_ShouldReturnEmptyList_WhenNoUsers()
        {
            // Arrange
            _userRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<User>());

            // Act
            var result = await _service.GetAllUsersAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }
        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task GetUserByIdAsync_ShouldThrowNotFound_WhenUserNotExists()
        {
            // Arrange
            _userRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                         .ReturnsAsync((User?)null);

            // Act + Assert
            try
            {
                await _service.GetUserByIdAsync(Guid.NewGuid());
            }
            catch (NotFoundException ex)
            {
                Assert.AreEqual("User not found or Invalid ID.", ex.Message);
                throw;
            }
        }
        [TestMethod]
        public async Task GetUserByIdAsync_ShouldReturnMappedUser_WhenExists()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                Email = "test@abc.com",
                MobileNumber = "123",
                Role = UserRole.Waiter,
                IsActive = true
            };

            _userRepoMock.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);

            // Act
            var result = await _service.GetUserByIdAsync(user.Id);

            // Assert
            Assert.AreEqual(user.Id, result.Id);
            Assert.AreEqual("Test", result.FullName);
            Assert.AreEqual("test@abc.com", result.Email);
            Assert.AreEqual("123", result.MobileNumber);
            Assert.AreEqual(UserRole.Waiter, result.Role);
            Assert.IsTrue(result.IsActive);
        }
        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task UpdateUserAsync_ShouldThrowNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            _userRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                         .ReturnsAsync((User?)null);

            var dto = new UserUpdateRequestDto
            {
                FullName = "New Name"
            };

            // Act + Assert
            try
            {
                await _service.UpdateUserAsync(Guid.NewGuid(), dto);
            } catch(NotFoundException ex)
            {
                Assert.AreEqual("User not found.", ex.Message);
                throw;
            }
        }
        [TestMethod]
        public async Task UpdateUserAsync_ShouldUpdateOnlyProvidedFields()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = "Old Name",
                Email = "old@test.com",
                MobileNumber = "1111",
                Role = UserRole.Waiter,
                IsActive = true
            };

            var dto = new UserUpdateRequestDto
            {
                FullName = "  New Name  ",
                MobileNumber = "  9999 ",
                Email = "  NEW@TEST.COM  ",
                IsActive = false
            };

            _userRepoMock.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);
            _userRepoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            await _service.UpdateUserAsync(user.Id, dto);

            // Assert changes
            Assert.AreEqual("New Name", user.Name);
            Assert.AreEqual("9999", user.MobileNumber);
            Assert.AreEqual("new@test.com", user.Email);
            Assert.AreEqual(false, user.IsActive);
        }
        [TestMethod]
        public async Task UpdateUserAsync_ShouldNotChangeFields_WhenDtoHasOnlyNullOrWhitespace()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = "Same Name",
                Email = "same@test.com",
                MobileNumber = "555",
                Role = UserRole.Waiter,
                IsActive = true
            };

            var dto = new UserUpdateRequestDto
            {
                FullName = "   ",
                Email = null,
                MobileNumber = "",
                IsActive = null,
                Role = UserRole.Waiter
            };

            _userRepoMock.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);
            _userRepoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            await _service.UpdateUserAsync(user.Id, dto);

            // Assert no change
            Assert.AreEqual("Same Name", user.Name);
            Assert.AreEqual("same@test.com", user.Email);
            Assert.AreEqual("555", user.MobileNumber);
            Assert.IsTrue(user.IsActive);
        }
        [TestMethod]
        public async Task UpdateUserAsync_ShouldThrowBadRequest_WhenRoleAdminIsPassed()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = "User",
                Email = "user@test.com",
                MobileNumber = "1234",
                Role = UserRole.Waiter,
                IsActive = true
            };

            var dto = new UserUpdateRequestDto
            {
                Role = UserRole.Admin
            };

            _userRepoMock.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);

            // Act + Assert
            var ex = await Assert.ThrowsExceptionAsync<BadRequestException>(() =>
                _service.UpdateUserAsync(user.Id, dto));

            Assert.AreEqual("Admin cannot be created here.", ex.Message);
        }
    }
}
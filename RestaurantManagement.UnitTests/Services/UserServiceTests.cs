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
        public async Task CreateUserAsync_EmailAlreadyExists_ThrowsConflictException()
        {
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
        public async Task CreateUserAsync_ValidUser_SavesAndReturnsUserDto()
        {
            var dto = new CreateUserRequestDto
            {
                Email = "  TEST@ABC.COM  ",
                FullName = "  Chandrapal Singh  ",
                MobileNumber = "  9876543210  ",
                Password = "Pass@123",
                Role = UserRole.Chef,
                AadharNumber = "123412341234",
                DateOfBirth= new DateTime(2003,8,18)
            };

            _userRepoMock
                .Setup(r => r.GetByEmailAsync("test@abc.com"))
                .ReturnsAsync((User?)null);

            User? capturedUser = null;

            _userRepoMock
                .Setup(r => r.AddAsync(It.IsAny<User>()))
                .Callback<User>(u => capturedUser = u)
                .Returns(Task.CompletedTask);

            _userRepoMock
                .Setup(r => r.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            var result = await _service.CreateUserAsync(dto);

            Assert.IsNotNull(capturedUser);
            Assert.AreEqual("test@abc.com", capturedUser.Email);
            Assert.AreEqual("Chandrapal Singh", capturedUser.Name);
            Assert.AreEqual("9876543210", capturedUser.MobileNumber);
            Assert.AreEqual(UserRole.Chef, capturedUser.Role);
            Assert.IsTrue(capturedUser.IsActive);
            Assert.AreNotEqual(dto.Password, capturedUser.Password);

            Assert.AreEqual(capturedUser.Id, result.Id);
            Assert.AreEqual("Chandrapal Singh", result.FullName);
            Assert.AreEqual("9876543210", result.MobileNumber);
            Assert.AreEqual("test@abc.com", result.Email);
            Assert.AreEqual(UserRole.Chef, result.Role);
            Assert.IsTrue(result.IsActive);
        }

        [TestMethod]
        public async Task GetAllUsersAsync_UsersExist_ReturnsUserDtoList()
        {
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

            var result = await _service.GetAllUsersAsync();

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(users[0].Id, result[0].Id);
            Assert.AreEqual("User1", result[0].FullName);
            Assert.AreEqual(UserRole.Waiter, result[0].Role);
            Assert.IsTrue(result[0].IsActive);
            Assert.AreEqual("User2", result[1].FullName);
            Assert.IsFalse(result[1].IsActive);
        }

        [TestMethod]
        public async Task GetAllUsersAsync_NoUsersFound_ReturnsEmptyList()
        {
            _userRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<User>());

            var result = await _service.GetAllUsersAsync();

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task GetUserByIdAsync_UserDoesNotExist_ThrowsNotFoundException()
        {
            _userRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                         .ReturnsAsync((User?)null);

            try
            {
                await _service.GetUserByIdAsync(Guid.NewGuid(), true, null);
            }
            catch (NotFoundException ex)
            {
                Assert.AreEqual("User not found or Invalid ID.", ex.Message);
                throw;
            }
        }

        [TestMethod]
        public async Task GetUserByIdAsync_AdminAccess_ReturnsUser()
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = "Admin View",
                Email = "admin@test.com",
                MobileNumber = "111",
                Role = UserRole.Waiter,
                IsActive = true
            };

            _userRepoMock.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);

            var result = await _service.GetUserByIdAsync(user.Id, true, Guid.NewGuid());

            Assert.AreEqual(user.Id, result.Id);
            Assert.AreEqual("Admin View", result.FullName);
        }

        [TestMethod]
        public async Task GetUserByIdAsync_SelfAccess_ReturnsUser()
        {
            var userId = Guid.NewGuid();

            var user = new User
            {
                Id = userId,
                Name = "Self User",
                Email = "self@test.com",
                MobileNumber = "222",
                Role = UserRole.Waiter,
                IsActive = true
            };

            _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);

            var result = await _service.GetUserByIdAsync(userId, false, userId);

            Assert.AreEqual(userId, result.Id);
            Assert.AreEqual("Self User", result.FullName);
        }

        [TestMethod]
        public async Task GetUserByIdAsync_NonAdminAccessingOtherUser_ThrowsForbiddenException()
        {
            var requesterId = Guid.NewGuid();
            var targetUserId = Guid.NewGuid();

            var user = new User
            {
                Id = targetUserId,
                Name = "Other User",
                Email = "other@test.com",
                MobileNumber = "333",
                Role = UserRole.Waiter,
                IsActive = true
            };

            _userRepoMock.Setup(r => r.GetByIdAsync(targetUserId)).ReturnsAsync(user);

            var ex = await Assert.ThrowsExceptionAsync<ForbiddenException>(() =>
                _service.GetUserByIdAsync(targetUserId, false, requesterId));

            Assert.AreEqual("You are not allowed to view this user.", ex.Message);
        }
        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task UpdateUserAsync_UserDoesNotExist_ThrowsNotFoundException()
        {
            _userRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                         .ReturnsAsync((User?)null);

            try
            {
                await _service.UpdateUserAsync(Guid.NewGuid(), new UserUpdateRequestDto());
            }
            catch (NotFoundException ex)
            {
                Assert.AreEqual("User not found.", ex.Message);
                throw;
            }
        }

        [TestMethod]
        public async Task UpdateUserAsync_PartialFieldsProvided_UpdatesOnlySpecifiedFields()
        {
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

            await _service.UpdateUserAsync(user.Id, dto);

            Assert.AreEqual("New Name", user.Name);
            Assert.AreEqual("9999", user.MobileNumber);
            Assert.AreEqual("new@test.com", user.Email);
            Assert.IsFalse(user.IsActive);
        }

        [TestMethod]
        public async Task UpdateUserAsync_NullOrWhitespaceFields_MakesNoChanges()
        {
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

            await _service.UpdateUserAsync(user.Id, dto);

            Assert.AreEqual("Same Name", user.Name);
            Assert.AreEqual("same@test.com", user.Email);
            Assert.AreEqual("555", user.MobileNumber);
            Assert.IsTrue(user.IsActive);
        }

        [TestMethod]
        public async Task UpdateUserAsync_RoleIsAdmin_ThrowsBadRequestException()
        {
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

            var ex = await Assert.ThrowsExceptionAsync<BadRequestException>(() =>
                _service.UpdateUserAsync(user.Id, dto));

            Assert.AreEqual("Admin cannot be created here.", ex.Message);
        }
    }
}
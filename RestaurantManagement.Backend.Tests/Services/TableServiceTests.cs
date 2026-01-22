using Azure;
using Moq;
using RestaurantManagement.Backend.Exceptions;
using RestaurantManagement.Backend.Services;
using RestaurantManagement.DataAccess.Models;
using RestaurantManagement.DataAccess.Repositories.Interfaces;
using RestaurantManagement.Dtos.Tables;

namespace RestaurantManagement.Backend.Tests.Services
{
    [TestClass]
    public class TableServiceTests
    {
        private Mock<ITableRepository> _tableRepoMock = null!;
        private TableService _service = null!;

        [TestInitialize]
        public void Setup()
        {
            _tableRepoMock = new Mock<ITableRepository>();
            _service = new TableService(_tableRepoMock.Object);
        }

        [TestMethod]
        public async Task CreateAsync_ShouldCreateTable_SaveAndReturnMappedDto()
        {
            // Arrange
            var dto = new TableCreateRequestDto
            {
                TableName = "  Table-01  ",
                Size = 4
            };

            Table? capturedTable = null;

            _tableRepoMock
                .Setup(r => r.AddAsync(It.IsAny<Table>()))
                .Callback<Table>(t => capturedTable = t)
                // removed callback check
                .Returns(Task.CompletedTask);

            _tableRepoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _service.CreateAsync(dto);

            // Assert - verify created entity
            Assert.IsNotNull(capturedTable);
            Assert.AreEqual("Table-01", capturedTable.TableName); // Trim check
            Assert.AreEqual(4, capturedTable.Capacity);
            Assert.IsFalse(capturedTable.IsOccupied);

            // Assert - verify response dto mapping
            Assert.AreEqual(capturedTable.Id, result.Id);
            Assert.AreEqual("Table-01", result.TableName);
            Assert.AreEqual(4, result.Size);
            Assert.IsFalse(result.IsOccupied);
        }
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public async Task CreateAsync_ShouldThrow_WhenTableNameIsNull()
        {
            // dto.TableName.Trim()
            var dto = new TableCreateRequestDto
            {
                TableName = null,
                Size = 2
            };

            // Act + Assert
            var result = await _service.CreateAsync(dto);
        }
        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task DeleteAsync_ShouldThrowNotFound_WhenTableDoesNotExist()
        {
            // Arrange
            _tableRepoMock
                .Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Table?)null);

            // Act + Assert
            await _service.DeleteAsync(Guid.NewGuid());
        }

        [TestMethod]
        [ExpectedException(typeof(BadRequestException))]
        public async Task DeleteAsync_ShouldThrowBadRequest_WhenTableIsOccupied()
        {
            // Arrange
            var table = new Table
            {
                Id = Guid.NewGuid(),
                TableName = "T1",
                Capacity = 4,
                IsOccupied = true
            };

            _tableRepoMock.Setup(r => r.GetByIdAsync(table.Id))
                          .ReturnsAsync(table);

            // Act + Assert
            try
            {
                await _service.DeleteAsync(table.Id);
            }
            catch(BadRequestException ex)
            {
                Assert.AreEqual("Cannot delete an occupied table.", ex.Message);
                throw;
            } 
        }

        [TestMethod]
        public async Task DeleteAsync_ShouldDeleteTable_WhenNotOccupied()
        {
            // Arrange
            var table = new Table
            {
                Id = Guid.NewGuid(),
                TableName = "T2",
                Capacity = 2,
                IsOccupied = false
            };

            _tableRepoMock.Setup(r => r.GetByIdAsync(table.Id))
                          .ReturnsAsync(table);

            _tableRepoMock.Setup(r => r.SaveChangesAsync())
                          .Returns(Task.CompletedTask);

            // Act
            await _service.DeleteAsync(table.Id);

            // Assert : If the method executed properly
        }
        [TestMethod]
        public async Task GetAllAsync_ShouldReturnMappedList()
        {
            // Arrange
            var tables = new List<Table>
            {
                new Table { Id = Guid.NewGuid(), TableName = "T1", Capacity = 2, IsOccupied = false },
                new Table { Id = Guid.NewGuid(), TableName = "T2", Capacity = 4, IsOccupied = true }
            };

            _tableRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(tables);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(tables[0].Id, result[0].Id);
            Assert.AreEqual("T1", result[0].TableName);
            Assert.AreEqual(2, result[0].Size);
            Assert.IsFalse(result[0].IsOccupied);
            Assert.AreEqual(tables[1].Id, result[1].Id);
            Assert.AreEqual("T2", result[1].TableName);
            Assert.AreEqual(4, result[1].Size);
            Assert.IsTrue(result[1].IsOccupied);
        }
        [TestMethod]
        public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoTablesFound()
        {
            // Arrange
            _tableRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Table>());

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }
        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task GetByIdAsync_ShouldThrowNotFound_WhenTableNotExists()
        {
            // Arrange
            _tableRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                          .ReturnsAsync((Table?)null);

            // Act + Assert
            await _service.GetByIdAsync(Guid.NewGuid());
        }
        [TestMethod]
        public async Task GetByIdAsync_ShouldReturnMappedDto_WhenExists()
        {
            // Arrange
            var table = new Table
            {
                Id = Guid.NewGuid(),
                TableName = "T-11",
                Capacity = 6,
                IsOccupied = true
            };

            _tableRepoMock.Setup(r => r.GetByIdAsync(table.Id)).ReturnsAsync(table);

            // Act
            var result = await _service.GetByIdAsync(table.Id);

            // Assert
            Assert.AreEqual(table.Id, result.Id);
            Assert.AreEqual("T-11", result.TableName);
            Assert.AreEqual(6, result.Size);
            Assert.IsTrue(result.IsOccupied);
        }
    }
}

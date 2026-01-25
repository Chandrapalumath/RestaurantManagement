using Microsoft.AspNetCore.Mvc;
using Moq;
using RestaurantManagement.Api.Controllers;
using RestaurantManagement.Backend.Services.Interfaces;
using RestaurantManagement.Dtos.Tables;

namespace RestaurantManagement.Api.Tests;

[TestClass]
public class TableControllerTests
{
    private Mock<ITableService> _tableServiceMock = null!;
    private TableController _controller = null!;

    [TestInitialize]
    public void Setup()
    {
        _tableServiceMock = new Mock<ITableService>();
        _controller = new TableController(_tableServiceMock.Object);
    }
    [TestMethod]
    public async Task GetAllTablesAsync_TablesExist_ReturnsOkWithTables()
    {
        // Arrange
        var tables = new List<TableResponseDto>
        {
            new TableResponseDto { Id = Guid.NewGuid(), TableName = "Table 1",  Size = 4, IsOccupied = false },
            new TableResponseDto { Id = Guid.NewGuid(), TableName = "Table 2", Size = 6, IsOccupied = true }
        };

        _tableServiceMock
            .Setup(s => s.GetAllAsync())
            .ReturnsAsync(tables);

        // Act
        var result = await _controller.GetAllTablesAsync();

        // Assert
        var ok = result as OkObjectResult;
        Assert.IsNotNull(ok);

        var data = ok.Value as List<TableResponseDto>;
        Assert.IsNotNull(data);
        Assert.AreEqual(2, data.Count);
    }
    [TestMethod]
    public async Task GetTableByIdAsync_TableExists_ReturnsOk()
    {
        // Arrange
        var tableId = Guid.NewGuid();

        var table = new TableResponseDto
        {
            Id = tableId,
            TableName = "Table 5",
            Size = 8,
            IsOccupied = false
        };

        _tableServiceMock
            .Setup(s => s.GetByIdAsync(tableId))
            .ReturnsAsync(table);

        // Act
        var result = await _controller.GetTableByIdAsync(tableId);

        // Assert
        var ok = result as OkObjectResult;
        Assert.IsNotNull(ok);

        var data = ok.Value as TableResponseDto;
        Assert.IsNotNull(data);
        Assert.AreEqual(tableId, data.Id);
    }
    [TestMethod]
    public async Task AddTableAsync_ValidTable_ReturnsCreatedAtRoute()
    {
        // Arrange
        var dto = new TableCreateRequestDto
        {
            TableName = "New Table",
            Size = 4
        };

        var createdTable = new TableResponseDto
        {
            Id = Guid.NewGuid(),
            TableName = dto.TableName,
            Size = dto.Size,
            IsOccupied = false
        };

        _tableServiceMock
            .Setup(s => s.CreateAsync(It.IsAny<TableCreateRequestDto>()))
            .ReturnsAsync(createdTable);

        // Act
        var result = await _controller.AddTableAsync(dto);

        // Assert
        var created = result as CreatedAtRouteResult;
        Assert.IsNotNull(created);

        Assert.AreEqual("GetTableById", created.RouteName);
        Assert.IsNotNull(created.RouteValues);
        Assert.AreEqual(createdTable.Id, created.RouteValues["id"]);
    }
    [TestMethod]
    public async Task DeleteTableAsync_TableExists_ReturnsNoContent()
    {
        // Arrange
        var tableId = Guid.NewGuid();

        _tableServiceMock
            .Setup(s => s.DeleteAsync(tableId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteTableAsync(tableId);

        // Assert
        Assert.IsInstanceOfType(result, typeof(NoContentResult));
    }
}
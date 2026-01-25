using Microsoft.AspNetCore.Mvc;
using Moq;
using RestaurantManagement.Api.Controllers;
using RestaurantManagement.Backend.Services.Interfaces;
using RestaurantManagement.Dtos.Customers;

namespace RestaurantManagement.Api.Tests;

[TestClass]
public class CustomerControllerTests
{
    private Mock<ICustomerService> _customerServiceMock = null!;
    private CustomerController _controller = null!;

    [TestInitialize]
    public void Setup()
    {
        _customerServiceMock = new Mock<ICustomerService>();
        _controller = new CustomerController(_customerServiceMock.Object);
    }

    [TestMethod]
    public async Task CreateCustomerAsync_ValidCustomer_ReturnsCreatedAtRoute()
    {
        // Arrange
        var dto = new CustomerCreateRequestDto
        {
            Name = "David",
            MobileNumber = "9876512340"
        };

        var createdCustomer = new CustomerResponseDto
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            MobileNumber = dto.MobileNumber
        };

        _customerServiceMock
            .Setup(s => s.CreateAsync(dto))
            .ReturnsAsync(createdCustomer);

        // Act
        var result = await _controller.CreateCustomerAsync(dto);

        // Assert
        var created = result as CreatedAtRouteResult;
        Assert.IsNotNull(created);

        Assert.AreEqual("GetCustomerById", created.RouteName);
        Assert.IsNotNull(created.RouteValues);
        Assert.AreEqual(createdCustomer.Id, created.RouteValues["id"]);
    }
    [TestMethod]
    public async Task GetCustomerByIdAsync_CustomerExists_ReturnsOk()
    {
        // Arrange
        var customerId = Guid.NewGuid();

        var customer = new CustomerResponseDto
        {
            Id = customerId,
            Name = "Sam",
            MobileNumber = "9999999999"
        };

        _customerServiceMock
            .Setup(s => s.GetByIdAsync(customerId))
            .ReturnsAsync(customer);

        // Act
        var result = await _controller.GetCustomerByIdAsync(customerId);

        // Assert
        var ok = result as OkObjectResult;
        Assert.IsNotNull(ok);

        var data = ok.Value as CustomerResponseDto;
        Assert.IsNotNull(data);
        Assert.AreEqual(customerId, data.Id);
    }
    [TestMethod]
    public async Task GetAllCustomersAsync_CustomersExist_ReturnsOkWithCustomers()
    {
        // Arrange
        var customers = new List<CustomerResponseDto>
        {
            new CustomerResponseDto { Id = Guid.NewGuid(), Name = "Tom", MobileNumber = "8888888888" },
            new CustomerResponseDto { Id = Guid.NewGuid(), Name = "Pam", MobileNumber = "7777777777" }
        };

        _customerServiceMock
            .Setup(s => s.GetAllAsync())
            .ReturnsAsync(customers);

        // Act
        var result = await _controller.GetAllCustomersAsync();

        // Assert
        var ok = result as OkObjectResult;
        Assert.IsNotNull(ok);

        var data = ok.Value as List<CustomerResponseDto>;
        Assert.IsNotNull(data);
        Assert.AreEqual(2, data.Count);
    }
    [TestMethod]
    public async Task GetCustomerByMobilePartialSearch_MatchingMobileExists_ReturnsOkWithMatchingCustomers()
    {
        // Arrange
        string number = "987";

        var customers = new List<CustomerResponseDto>
            {
                new CustomerResponseDto { Id = Guid.NewGuid(), Name = "Alex", MobileNumber = "9870001111" },
                new CustomerResponseDto { Id = Guid.NewGuid(), Name = "Mark", MobileNumber = "9879992222" }
            };

        _customerServiceMock
            .Setup(s => s.GetByMobileNumberAsync(number))
            .ReturnsAsync(customers);

        // Act
        var result = await _controller.GetCustomerByMobilePartialSearch(number);

        // Assert
        var ok = result as OkObjectResult;
        Assert.IsNotNull(ok);

        var data = ok.Value as List<CustomerResponseDto>;
        Assert.IsNotNull(data);
        Assert.AreEqual(2, data.Count);
    }
}

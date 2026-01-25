using Moq;
using RestaurantManagement.Backend.Exceptions;
using RestaurantManagement.Backend.Services;
using RestaurantManagement.DataAccess.Models;
using RestaurantManagement.DataAccess.Repositories.Interfaces;
using RestaurantManagement.Dtos.Customers;

namespace RestaurantManagement.Backend.Tests;

[TestClass]
public class CustomerServiceTests
{
    private Mock<ICustomerRepository> _customerRepoMock = null!;
    private CustomerService _service = null!;

    [TestInitialize]
    public void Setup()
    {
        _customerRepoMock = new Mock<ICustomerRepository>();
        _service = new CustomerService(_customerRepoMock.Object);
    }

    [TestMethod]
    public async Task CreateAsync_ShouldReturnExistingCustomer_WhenMobileAlreadyExists()
    {
        // Arrange
        var existingCustomer = new Customer
        {
            Id = Guid.NewGuid(),
            Name = "Existing",
            MobileNumber = "9999999999",
            CreatedAt = DateTime.UtcNow.AddDays(-5)
        };

        var dto = new CustomerCreateRequestDto
        {
            Name = "New Name",
            MobileNumber = "9999999999"
        };

        _customerRepoMock
            .Setup(r => r.GetByMobileAsync(dto.MobileNumber))
            .ReturnsAsync(new List<Customer> { existingCustomer });

        // Act
        var result = await _service.CreateAsync(dto);

        // Assert
        Assert.AreEqual(existingCustomer.Id, result.Id);
        Assert.AreEqual(existingCustomer.Name, result.Name);
        Assert.AreEqual(existingCustomer.MobileNumber, result.MobileNumber);
        Assert.AreEqual(existingCustomer.CreatedAt, result.CreatedAt);
    }

    [TestMethod]
    public async Task CreateAsync_ShouldCreateCustomer_WhenMobileDoesNotExist()
    {
        // Arrange
        var dto = new CustomerCreateRequestDto
        {
            Name = "  Chandrapal  ",
            MobileNumber = " 8888888888 "
        };

        _customerRepoMock
            .Setup(r => r.GetByMobileAsync(dto.MobileNumber))
            .ReturnsAsync(new List<Customer>());

        Customer? capturedCustomer = null;

        _customerRepoMock
            .Setup(r => r.AddAsync(It.IsAny<Customer>()))
            .Callback<Customer>(c => capturedCustomer = c)
            .Returns(Task.CompletedTask);

        _customerRepoMock
            .Setup(r => r.SaveChangesAsync())
            .Returns(Task.CompletedTask);

        // Act
        var result = await _service.CreateAsync(dto);

        // Assert
        Assert.IsNotNull(capturedCustomer);
        Assert.AreEqual("Chandrapal", capturedCustomer!.Name);
        Assert.AreEqual("8888888888", capturedCustomer.MobileNumber);

        Assert.AreEqual(capturedCustomer.Id, result.Id);
        Assert.AreEqual("Chandrapal", result.Name);
        Assert.AreEqual("8888888888", result.MobileNumber);
    }

    [TestMethod]
    [ExpectedException(typeof(NotFoundException))]
    public async Task GetByIdAsync_ShouldThrowNotFound_WhenCustomerDoesNotExist()
    {
        // Arrange
        _customerRepoMock
            .Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Customer?)null);

        // Act + Assert
        try
        {
            await _service.GetByIdAsync(Guid.NewGuid());
        }
        catch (NotFoundException ex)
        {
            Assert.AreEqual("Customer not found.", ex.Message);
            throw;
        }
    }

    [TestMethod]
    public async Task GetByIdAsync_ShouldReturnMappedCustomer_WhenExists()
    {
        // Arrange
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            Name = "Test",
            MobileNumber = "777",
            CreatedAt = DateTime.UtcNow
        };

        _customerRepoMock
            .Setup(r => r.GetByIdAsync(customer.Id))
            .ReturnsAsync(customer);

        // Act
        var result = await _service.GetByIdAsync(customer.Id);

        // Assert
        Assert.AreEqual(customer.Id, result.Id);
        Assert.AreEqual("Test", result.Name);
        Assert.AreEqual("777", result.MobileNumber);
        Assert.AreEqual(customer.CreatedAt, result.CreatedAt);
    }

    [TestMethod]
    public async Task GetAllAsync_ShouldReturnMappedList()
    {
        // Arrange
        var customers = new List<Customer>
            {
                new Customer
                {
                    Id = Guid.NewGuid(),
                    Name = "A",
                    MobileNumber = "111",
                    CreatedAt = DateTime.UtcNow
                },
                new Customer
                {
                    Id = Guid.NewGuid(),
                    Name = "B",
                    MobileNumber = "222",
                    CreatedAt = DateTime.UtcNow
                }
            };

        _customerRepoMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(customers);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.AreEqual(2, result.Count);
        Assert.AreEqual("A", result[0].Name);
        Assert.AreEqual("222", result[1].MobileNumber);
    }

    [TestMethod]
    public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoCustomers()
    {
        // Arrange
        _customerRepoMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<Customer>());

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    [ExpectedException(typeof(NotFoundException))]  
    public async Task GetByMobileNumberAsync_ShouldThrowNotFound_WhenNoCustomerFound()
    {
        // Arrange
        _customerRepoMock
            .Setup(r => r.GetByMobileAsync("999"))
            .ReturnsAsync(new List<Customer>());

        // Act + Assert
        try
        {
            await _service.GetByMobileNumberAsync("999");
        }
        catch(NotFoundException ex)
        {
            Assert.AreEqual("No user found with this mobile number", ex.Message);
            throw;
        }
    }

    [TestMethod]
    public async Task GetByMobileNumberAsync_ShouldReturnMappedList_WhenCustomersExist()
    {
        // Arrange
        var customers = new List<Customer>
            {
                new Customer
                {
                    Id = Guid.NewGuid(),
                    Name = "X",
                    MobileNumber = "555",
                    CreatedAt = DateTime.UtcNow
                },
                new Customer
                {
                    Id = Guid.NewGuid(),
                    Name = "Y",
                    MobileNumber = "555",
                    CreatedAt = DateTime.UtcNow
                }
            };

        _customerRepoMock
            .Setup(r => r.GetByMobileAsync("555"))
            .ReturnsAsync(customers);

        // Act
        var result = await _service.GetByMobileNumberAsync("555");

        // Assert
        Assert.AreEqual(2, result.Count);
        Assert.IsTrue(result.All(c => c.MobileNumber == "555"));
    }
}

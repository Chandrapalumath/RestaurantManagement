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
    public async Task CreateAsync_MobileDoesNotExist_CreatesNewCustomer()
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
    public async Task GetByIdAsync_CustomerDoesNotExist_ThrowsNotFoundException()
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
    public async Task GetByIdAsync_CustomerExists_ReturnsMappedCustomer()
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
    }
    [TestMethod]
    public async Task GetAllAsync_ReturnsPagedCustomers()
    {
        _customerRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Customer>
        {
            new Customer { Id = Guid.NewGuid(), Name = "Ram", MobileNumber = "123" },
            new Customer { Id = Guid.NewGuid(), Name = "Shyam", MobileNumber = "456" }
        });

        var result = await _service.GetAllAsync(1, 1, null);

        Assert.AreEqual(1, result.Items.Count);
        Assert.AreEqual(2, result.TotalCount);
    }

    [TestMethod]
    public async Task GetAllAsync_SearchFiltersResults()
    {
        _customerRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Customer>
        {
            new Customer { Id = Guid.NewGuid(), Name = "Aman", MobileNumber = "111" },
            new Customer { Id = Guid.NewGuid(), Name = "Rahul", MobileNumber = "222" }
        });

        var result = await _service.GetAllAsync(1, 5, "Aman");

        Assert.AreEqual(1, result.Items.Count);
    }
    [TestMethod]
    public async Task GetByMobileNumberAsync_CustomerExists_ReturnsMappedCustomer()
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

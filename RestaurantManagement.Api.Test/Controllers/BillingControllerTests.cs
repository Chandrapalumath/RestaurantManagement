using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RestaurantManagement.Api.Controllers;
using RestaurantManagement.Backend.Services.Interfaces;
using RestaurantManagement.Dtos.Billing;
using System.Security.Claims;

namespace RestaurantManagement.Api.Tests;

[TestClass]
public class BillingControllerTests
{
    private Mock<IBillingService> _billingServiceMock = null!;
    private BillingController _controller = null!;

    [TestInitialize]
    public void Setup()
    {
        _billingServiceMock = new Mock<IBillingService>();
        _controller = new BillingController(_billingServiceMock.Object);
    }

    private void SetUser(Guid userId, params string[] roles)
    {
        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            };

        foreach (var role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role));

        var identity = new ClaimsIdentity(claims, "TestAuth");
        var user = new ClaimsPrincipal(identity);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
    }

    [TestMethod]
    public async Task GenerateBill_ValidBill_ReturnsCreatedAtRoute()
    {
        // Arrange
        var waiterId = Guid.NewGuid();
        SetUser(waiterId, "Waiter");

        var dto = new BillGenerateRequestDto
        {
            CustomerId = Guid.NewGuid(),
            OrdersId = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() }
        };

        var response = new BillResponseDto
        {
            BillId = Guid.NewGuid(),
            CustomerId = dto.CustomerId,
            SubTotal = 100,
            DiscountPercent = 10,
            DiscountAmount = 10,
            TaxPercent = 5,
            TaxAmount = 4.5m,
            GrandTotal = 94.5m,
            IsPaymentDone = false,
            GeneratedAt = DateTime.UtcNow
        };

        _billingServiceMock
            .Setup(s => s.GenerateBillAsync(waiterId, dto))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.GenerateBill(dto);

        // Assert
        var created = result as CreatedAtRouteResult;
        Assert.IsNotNull(created);

        Assert.AreEqual("GetBillById", created.RouteName);
        Assert.IsNotNull(created.RouteValues);
        Assert.AreEqual(response.BillId, created.RouteValues["id"]);
    }

    [TestMethod]
    public async Task UpdateBill_ValidBill_ReturnsNoContent()
    {
        // Arrangec
        var waiterId = Guid.NewGuid();
        SetUser(waiterId, "Waiter");

        var billId = Guid.NewGuid();
        var dto = new BillUpdateRequestDto
        {
            IsPaymentDone = true
        };

        _billingServiceMock
            .Setup(s => s.UpdateBill(billId, waiterId, dto))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdateBill(billId, dto);

        // Assert
        Assert.IsInstanceOfType(result, typeof(NoContentResult));
    }

    [TestMethod]
    public async Task GetBillByIdAsync_WaiterRole_CallsServiceWithWaiterId()
    {
        // Arrange
        var waiterId = Guid.NewGuid();
        SetUser(waiterId, "Waiter");

        var billId = Guid.NewGuid();

        var response = new BillResponseDto
        {
            BillId = billId,
            CustomerId = Guid.NewGuid(),
            SubTotal = 100,
            DiscountPercent = 10,
            DiscountAmount = 10,
            TaxPercent = 5,
            TaxAmount = 4.5m,
            GrandTotal = 94.5m,
            IsPaymentDone = false,
            GeneratedAt = DateTime.UtcNow
        };

        _billingServiceMock
            .Setup(s => s.GetBillByIdAsync(billId, waiterId, false))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.GetBillByIdAsync(billId);

        // Assert
        var ok = result as OkObjectResult;
        Assert.IsNotNull(ok);

        var data = ok.Value as BillResponseDto;
        Assert.IsNotNull(data);
        Assert.AreEqual(billId, data.BillId);
    }

    [TestMethod]
    public async Task GetBillByIdAsync_AdminRole_CallsServiceWithNullWaiterId()
    {
        // Arrange
        var adminId = Guid.NewGuid();
        SetUser(adminId, "Admin");

        var billId = Guid.NewGuid();

        var response = new BillResponseDto
        {
            BillId = billId,
            CustomerId = Guid.NewGuid(),
            SubTotal = 100,
            DiscountPercent = 10,
            DiscountAmount = 10,
            TaxPercent = 5,
            TaxAmount = 4.5m,
            GrandTotal = 94.5m,
            IsPaymentDone = false,
            GeneratedAt = DateTime.UtcNow
        };

        _billingServiceMock
            .Setup(s => s.GetBillByIdAsync(billId, null, true))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.GetBillByIdAsync(billId);

        // Assert
        var ok = result as OkObjectResult;
        Assert.IsNotNull(ok);

        var data = ok.Value as BillResponseDto;
        Assert.IsNotNull(data);
        Assert.AreEqual(billId, data.BillId);
    }

    [TestMethod]
    public async Task GetAllBillsAsync_BillsExist_ReturnsOkWithBills()
    {
        // Arrange
        SetUser(Guid.NewGuid(), "Admin");
        var bills = new List<BillResponseDto>
            {
                new BillResponseDto { 
                    BillId = Guid.NewGuid(),
                    CustomerId = Guid.NewGuid(),
                    SubTotal = 100,
                    DiscountPercent = 10,
                    DiscountAmount = 10,
                    TaxPercent = 5,
                    TaxAmount = 4.5m,
                    GrandTotal = 94.5m,
                    IsPaymentDone = false,
                    GeneratedAt = DateTime.UtcNow 
                },
                new BillResponseDto {
                    BillId = Guid.NewGuid(),
                    CustomerId = Guid.NewGuid(),
                    SubTotal = 200,
                    DiscountPercent = 10,
                    DiscountAmount = 10,
                    TaxPercent = 5,
                    TaxAmount = 4.5m,
                    GrandTotal = 94.5m,
                    IsPaymentDone = false,
                    GeneratedAt = DateTime.UtcNow
                }
            };

        _billingServiceMock
            .Setup(s => s.GetAllBillsAsync())
            .ReturnsAsync(bills);

        // Act
        var result = await _controller.GetAllBillsAsync();

        // Assert
        var ok = result as OkObjectResult;
        Assert.IsNotNull(ok);

        var data = ok.Value as List<BillResponseDto>;
        Assert.IsNotNull(data);
        Assert.AreEqual(2, data.Count);
    }
}

using Moq;
using RestaurantManagement.Backend.Exceptions;
using RestaurantManagement.Backend.Services;
using RestaurantManagement.DataAccess.Models;
using RestaurantManagement.DataAccess.Repositories.Interfaces;
using RestaurantManagement.Dtos.Billing;
using RestaurantManagement.Models.Common.Enums;

namespace RestaurantManagement.Backend.Tests;

[TestClass]
public class BillingServiceTests
{
    private Mock<IOrderRepository> _orderRepoMock = null!;
    private Mock<IBillingRepository> _billingRepoMock = null!;
    private Mock<ISettingsRepository> _settingsRepoMock = null!;
    private Mock<ITableRepository> _tableRepoMock = null!;
    private BillingService _service = null!;

    [TestInitialize]
    public void Setup()
    {
        _orderRepoMock = new Mock<IOrderRepository>();
        _billingRepoMock = new Mock<IBillingRepository>();
        _settingsRepoMock = new Mock<ISettingsRepository>();
        _tableRepoMock = new Mock<ITableRepository>();

        _service = new BillingService(
            _orderRepoMock.Object,
            _billingRepoMock.Object,
            _settingsRepoMock.Object,
            _tableRepoMock.Object);
    }

    [TestMethod]
    [ExpectedException(typeof(NotFoundException))]
    public async Task GenerateBillAsync_ShouldThrowNotFound_WhenOrderIdsEmpty()
    {
        var dto = new BillGenerateRequestDto
        {
            OrdersId = new List<Guid>(),
            CustomerId = Guid.NewGuid()
        };
        try
        {
            await _service.GenerateBillAsync(Guid.NewGuid(), dto);
        }
        catch(NotFoundException ex)
        {
            Assert.AreEqual("Please give all the order ids", ex.Message);
            throw;
        }
    }

    [TestMethod]
    [ExpectedException(typeof(BadRequestException))]
    public async Task GenerateBillAsync_ShouldThrow_WhenOrderNotCompleted()
    {
        var orderId = Guid.NewGuid();

        var dto = new BillGenerateRequestDto
        {
            OrdersId = new List<Guid> { orderId },
            CustomerId = Guid.NewGuid()
        };

        _orderRepoMock.Setup(r => r.GetOrderWithItemsAsync(orderId))
                      .ReturnsAsync(new Order { Status = OrderStatus.Pending });

        try
        {
            await _service.GenerateBillAsync(Guid.NewGuid(), dto);
        }
        catch(BadRequestException ex)
        {
            Assert.AreEqual("All Orders are not completed yet", ex.Message);
            throw;
        }
    }

    [TestMethod]
    [ExpectedException(typeof(NotFoundException))]
    public async Task GenerateBillAsync_ShouldThrow_WhenSettingsNotConfigured()
    {
        var orderId = Guid.NewGuid();

        _orderRepoMock.Setup(r => r.GetOrderWithItemsAsync(orderId))
                      .ReturnsAsync(new Order
                      {
                          Id = orderId,
                          Status = OrderStatus.Completed,
                          TableId = Guid.NewGuid(),
                          Items = new List<OrderItem>()
                      });

        _settingsRepoMock.Setup(r => r.GetSettingsAsync())
                         .ReturnsAsync((RestaurantSettings?)null);

        var dto = new BillGenerateRequestDto
        {
            OrdersId = new List<Guid> { orderId },
            CustomerId = Guid.NewGuid()
        };

        try
        {
            await  _service.GenerateBillAsync(Guid.NewGuid(), dto);
        }
        catch (NotFoundException ex)
        {
            Assert.AreEqual("Admin settings not configured.", ex.Message);
            throw;
        }
    }

    [TestMethod]
    [ExpectedException(typeof(BadRequestException))]
    public async Task GenerateBillAsync_ShouldThrow_WhenOrdersFromDifferentTables()
    {
        var o1 = new Order
        {
            Id = Guid.NewGuid(),
            Status = OrderStatus.Completed,
            TableId = Guid.NewGuid(),
            Items = new List<OrderItem>()
        };

        var o2 = new Order
        {
            Id = Guid.NewGuid(),
            Status = OrderStatus.Completed,
            TableId = Guid.NewGuid(),
            Items = new List<OrderItem>()
        };

        _orderRepoMock.Setup(r => r.GetOrderWithItemsAsync(o1.Id)).ReturnsAsync(o1);
        _orderRepoMock.Setup(r => r.GetOrderWithItemsAsync(o2.Id)).ReturnsAsync(o2);
        _settingsRepoMock.Setup(r => r.GetSettingsAsync())
                         .ReturnsAsync(new RestaurantSettings());

        var dto = new BillGenerateRequestDto
        {
            OrdersId = new List<Guid> { o1.Id, o2.Id },
            CustomerId = Guid.NewGuid()
        };

        try
        {
            await _service.GenerateBillAsync(Guid.NewGuid(), dto);
        } catch(BadRequestException ex)
        {
            Assert.AreEqual("All orders must belong to same table.", ex.Message);
            throw;
        }
    }

    [TestMethod]
    public async Task GenerateBillAsync_ShouldGenerateBill_AndUpdateOrdersAndTable()
    {
        var tableId = Guid.NewGuid();
        var waiterId = Guid.NewGuid();

        var order = new Order
        {
            Id = Guid.NewGuid(),
            Status = OrderStatus.Completed,
            TableId = tableId,
            Items = new List<OrderItem>
                {
                    new OrderItem { Quantity = 2, UnitPrice = 100 },
                    new OrderItem { Quantity = 1, UnitPrice = 50 }
                }
        };

        _orderRepoMock.Setup(r => r.GetOrderWithItemsAsync(order.Id))
                      .ReturnsAsync(order);

        _settingsRepoMock.Setup(r => r.GetSettingsAsync())
                         .ReturnsAsync(new RestaurantSettings
                         {
                             DiscountPercent = 10,
                             TaxPercent = 5
                         });

        _billingRepoMock.Setup(r => r.AddAsync(It.IsAny<Bill>()))
                        .Returns(Task.CompletedTask);
        _billingRepoMock.Setup(r => r.SaveChangesAsync())
                        .Returns(Task.CompletedTask);

        _tableRepoMock.Setup(r => r.GetByIdAsync(tableId))
                      .ReturnsAsync(new Table { Id = tableId, IsOccupied = true });

        var dto = new BillGenerateRequestDto
        {
            OrdersId = new List<Guid> { order.Id },
            CustomerId = Guid.NewGuid()
        };

        var result = await _service.GenerateBillAsync(waiterId, dto);

        Assert.AreEqual(250, result.SubTotal); // 2*100 + 1*50
        Assert.AreEqual(10, result.DiscountPercent);
        Assert.AreEqual(25, result.DiscountAmount);
        Assert.AreEqual(11.25m, result.TaxAmount);
        Assert.AreEqual(236.25m, result.GrandTotal);
    }

    [TestMethod]
    [ExpectedException(typeof(ForbiddenException))]
    public async Task GetBillByIdAsync_ShouldThrowForbidden_WhenWaiterAccessOtherBill()
    {
        var bill = new Bill
        {
            Id = Guid.NewGuid(),
            GeneratedByWaiterId = Guid.NewGuid()
        };

        _billingRepoMock.Setup(r => r.GetBillDetailsAsync(bill.Id))
                        .ReturnsAsync(bill);
        try
        {
            await _service.GetBillByIdAsync(bill.Id, Guid.NewGuid(), false);
        }
        catch(ForbiddenException ex)
        {
            Assert.AreEqual("You can only view bills generated by you.", ex.Message);
            throw;
        }
    }

    [TestMethod]
    public async Task GetBillByIdAsync_ShouldAllowAdmin()
    {
        var bill = new Bill { Id = Guid.NewGuid(), DiscountAmount = 120, DiscountPercent =  12 };

        _billingRepoMock.Setup(r => r.GetBillDetailsAsync(bill.Id))
                        .ReturnsAsync(bill);

        var result = await _service.GetBillByIdAsync(bill.Id, null, true);

        Assert.AreEqual(bill.Id, result.BillId);
    }

    [TestMethod]
    [ExpectedException (typeof(BadRequestException))]
    public async Task UpdateBill_ShouldThrow_WhenNotOwnerWaiter()
    {
        var bill = new Bill
        {
            Id = Guid.NewGuid(),
            GeneratedByWaiterId = Guid.NewGuid()
        };

        _billingRepoMock.Setup(r => r.GetByIdAsync(bill.Id))
                        .ReturnsAsync(bill);

        var dto = new BillUpdateRequestDto { IsPaymentDone = true };

        try
        {
            await _service.UpdateBill(bill.Id, Guid.NewGuid(), dto);
        } catch(BadRequestException ex)
        {
            Assert.AreEqual("You can update payment only for your own bill.", ex.Message);
            throw;
        } 
    }

    [TestMethod]
    public async Task UpdateBill_ShouldMarkPaymentDone()
    {
        var waiterId = Guid.NewGuid();

        var bill = new Bill
        {
            Id = Guid.NewGuid(),
            GeneratedByWaiterId = waiterId,
            IsPaymentDone = false
        };

        _billingRepoMock.Setup(r => r.GetByIdAsync(bill.Id))
                        .ReturnsAsync(bill);

        _billingRepoMock.Setup(r => r.SaveChangesAsync())
                        .Returns(Task.CompletedTask);

        var dto = new BillUpdateRequestDto { IsPaymentDone = true };

        await _service.UpdateBill(bill.Id, waiterId, dto);

        Assert.IsTrue(bill.IsPaymentDone);
        _billingRepoMock.Verify(r => r.Update(bill), Times.Once);
    }
}

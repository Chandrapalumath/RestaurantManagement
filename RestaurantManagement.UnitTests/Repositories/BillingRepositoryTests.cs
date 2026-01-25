using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestaurantManagement.DataAccess;
using RestaurantManagement.DataAccess.Models;
using RestaurantManagement.DataAccess.Repositories;
using RestaurantManagement.Models.Common.Enums;

namespace RestaurantManagement.Backend.Tests.Repositories
{
    [TestClass]
    public class BillingRepositoryTests
    {
        private RestaurantDbContext _context = null!;
        private BillingRepository _repo = null!;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<RestaurantDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new RestaurantDbContext(options);
            _repo = new BillingRepository(_context);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [TestMethod]
        public async Task GetBillDetailsAsync_ValidBillId_ReturnsBillWithAllIncludes()
        {
            // Arrange
            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                Name = "Customer One",
                MobileNumber = "9999999999"
            };

            var waiterUser = new User
            {
                Id = Guid.NewGuid(),
                Name = "Waiter One",
                MobileNumber = "8888888888",
                Email = "waiter@test.com",
                Password = "Test@123",
                Role = UserRole.Waiter,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var table = new Table
            {
                Id = Guid.NewGuid(),
                TableName = "T-01",
                Capacity = 4,
                IsOccupied = true,
                CreatedAt = DateTime.UtcNow
            };

            var menuItem = new MenuItem
            {
                Id = Guid.NewGuid(),
                Name = "Paneer Tikka",
                Price = 250,
                IsAvailable = true,
                CreatedAt = DateTime.UtcNow
            };

            var billId = Guid.NewGuid();

            var order = new Order
            {
                Id = Guid.NewGuid(),
                TableId = table.Id,
                Table = table,
                WaiterId = waiterUser.Id,
                Waiter = waiterUser,
                Status = OrderStatus.Pending,
                CreatedAt = DateTime.UtcNow.AddMinutes(-30),
                IsBilled = true,
                BillingId = billId
            };

            var orderItem = new OrderItem
            {
                Id = Guid.NewGuid(),
                OrderId = order.Id,
                Order = order,
                MenuItemId = menuItem.Id,
                MenuItem = menuItem,
                Quantity = 2,
                UnitPrice = menuItem.Price
            };

            order.Items.Add(orderItem);

            var bill = new Bill
            {
                Id = billId,
                CustomerId = customer.Id,
                Customer = customer,
                GeneratedByWaiterId = waiterUser.Id,
                GeneratedByWaiter = waiterUser,
                GeneratedAt = DateTime.UtcNow,
                SubTotal = 500,
                TaxPercent = 5,
                TaxAmount = 25,
                GrandTotal = 525,
                IsPaymentDone = false,
                Orders = new List<Order> { order }
            };
            order.Bill = bill;

            _context.Customers.Add(customer);
            _context.Users.Add(waiterUser);
            _context.Tables.Add(table);
            _context.MenuItems.Add(menuItem);
            _context.Orders.Add(order);
            _context.OrderItems.Add(orderItem);
            _context.Bills.Add(bill);

            await _context.SaveChangesAsync();

            // Act
            var result = await _repo.GetBillDetailsAsync(billId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(billId, result.Id);
            Assert.IsNotNull(result.Customer);
            Assert.AreEqual(customer.Id, result.Customer.Id);
            Assert.AreEqual("Customer One", result.Customer.Name);
            Assert.IsNotNull(result.GeneratedByWaiter);
            Assert.AreEqual(waiterUser.Id, result.GeneratedByWaiter.Id);
            Assert.AreEqual(UserRole.Waiter, result.GeneratedByWaiter.Role);
            Assert.IsNotNull(result.Orders);
            Assert.AreEqual(1, result.Orders.Count);
            var resOrder = result.Orders.First();
            Assert.IsNotNull(resOrder.Items);
            Assert.AreEqual(1, resOrder.Items.Count);
            var resItem = resOrder.Items.First();
            Assert.IsNotNull(resItem.MenuItem);
            Assert.AreEqual("Paneer Tikka", resItem.MenuItem.Name);
            Assert.AreEqual(250, resItem.MenuItem.Price);
        }
        [TestMethod]
        public async Task GetBillDetailsAsync_InvalidBillId_ReturnsNull()
        {
            // Act
            var result = await _repo.GetBillDetailsAsync(Guid.NewGuid());

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetBillsByCustomerIdAsync_ReturnsBills_Filtered_And_OrderedByGeneratedAtDesc()
        {
            // Arrange
            var customerId = Guid.NewGuid();

            var b1 = new Bill
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                GeneratedByWaiterId = Guid.NewGuid(),
                GeneratedAt = DateTime.UtcNow.AddMinutes(-30)
            };

            var b2 = new Bill
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                GeneratedByWaiterId = Guid.NewGuid(),
                GeneratedAt = DateTime.UtcNow.AddMinutes(-5)
            };

            var otherCustomerBill = new Bill
            {
                Id = Guid.NewGuid(),
                CustomerId = Guid.NewGuid(),
                GeneratedByWaiterId = Guid.NewGuid(),
                GeneratedAt = DateTime.UtcNow
            };

            _context.Bills.AddRange(b1, b2, otherCustomerBill);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repo.GetBillsByCustomerIdAsync(customerId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(b2.Id, result[0].Id);
            Assert.AreEqual(b1.Id, result[1].Id);
        }

        [TestMethod]
        public async Task GetAllBillsAsync_ReturnsBills_OrderedByGeneratedAtDesc()
        {
            // Arrange
            var billOld = new Bill
            {
                Id = Guid.NewGuid(),
                CustomerId = Guid.NewGuid(),
                GeneratedByWaiterId = Guid.NewGuid(),
                GeneratedAt = DateTime.UtcNow.AddHours(-5)
            };

            var billNew = new Bill
            {
                Id = Guid.NewGuid(),
                CustomerId = Guid.NewGuid(),
                GeneratedByWaiterId = Guid.NewGuid(),
                GeneratedAt = DateTime.UtcNow.AddMinutes(-1)
            };

            var billMiddle = new Bill
            {
                Id = Guid.NewGuid(),
                CustomerId = Guid.NewGuid(),
                GeneratedByWaiterId = Guid.NewGuid(),
                GeneratedAt = DateTime.UtcNow.AddHours(-1)
            };

            _context.Bills.AddRange(billOld, billNew, billMiddle);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repo.GetAllBillsAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);

            // latest first
            Assert.AreEqual(billNew.Id, result[0].Id);
        }
    }
}

using Moq;
using RestaurantManagement.Backend.Services;
using RestaurantManagement.DataAccess.Models;
using RestaurantManagement.DataAccess.Repositories.Interfaces;

namespace RestaurantManagement.UnitTests;

[TestClass]
public class AdminDashboardServiceTests
{
    private Mock<IBillingRepository> _billRepo = null!;
    private Mock<ITableRepository> _tableRepo = null!;
    private Mock<IReviewRepository> _reviewRepo = null!;
    private Mock<ISettingsRepository> _settingsRepo = null!;
    private Mock<IUserRepository> _userRepo = null!;
    private AdminDashboardService _service = null!;

    [TestInitialize]
    public void Setup()
    {
        _billRepo = new Mock<IBillingRepository>();
        _tableRepo = new Mock<ITableRepository>();
        _reviewRepo = new Mock<IReviewRepository>();
        _settingsRepo = new Mock<ISettingsRepository>();
        _userRepo = new Mock<IUserRepository>();

        _service = new AdminDashboardService(
            _reviewRepo.Object,
            _tableRepo.Object,
            _billRepo.Object,
            _settingsRepo.Object,
            _userRepo.Object);
    }
    [TestMethod]
    public async Task GetDashboardDataAsync_CalculatesTotalSalesCorrectly()
    {
        _billRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Bill>
        {
            new Bill { IsPaymentDone = true, GrandTotal = 200 },
            new Bill { IsPaymentDone = false, GrandTotal = 500 },
            new Bill { IsPaymentDone = true, GrandTotal = 300 }
        });

        _tableRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Table>());
        _reviewRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Review>());
        _settingsRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<RestaurantSettings>());
        _userRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<User>());

        var result = await _service.GetDashboardDataAsync();

        Assert.AreEqual(500, result.TotalSales);
    }
    [TestMethod]
    public async Task GetDashboardDataAsync_ReturnsTopWaiters()
    {
        var waiterId = Guid.NewGuid();

        _billRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Bill>
        {
            new Bill { GeneratedByWaiterId = waiterId, IsPaymentDone = true },
            new Bill { GeneratedByWaiterId = waiterId, IsPaymentDone = true }
        });

        _userRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<User>
        {
            new User { Id = waiterId, Name = "Ankit Kumar" }
        });

        _tableRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Table>());
        _reviewRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Review>());
        _settingsRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<RestaurantSettings>());

        var result = await _service.GetDashboardDataAsync();

        Assert.AreEqual("Ankit Kumar", result.TopWaiters.First().WaiterName);
        Assert.AreEqual(2, result.TopWaiters.First().OrdersServed);
    }

}
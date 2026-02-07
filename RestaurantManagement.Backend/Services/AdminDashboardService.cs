using RestaurantManagement.Backend.Services.Interfaces;
using RestaurantManagement.DataAccess.Repositories.Interfaces;
using RestaurantManagement.Dtos.Authentication;

namespace RestaurantManagement.Backend.Services
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly IReviewRepository _reviewRepo;
        private readonly ITableRepository _tableRepo;
        private readonly IBillingRepository _billRepo;
        private readonly ISettingsRepository _settingsRepo;
        private readonly IUserRepository _userRepo;

        public AdminDashboardService(
            IReviewRepository reviewRepo,
            ITableRepository tableRepo,
            IBillingRepository billRepo,
            ISettingsRepository settingsRepo,
            IUserRepository userRepo) 
        {
            _reviewRepo = reviewRepo;
            _tableRepo = tableRepo;
            _billRepo = billRepo;
            _settingsRepo = settingsRepo;
            _userRepo = userRepo;
        }

        public async Task<AdminDashboardDto> GetDashboardDataAsync()
        {
            var tables = await _tableRepo.GetAllAsync();
            var bills = await _billRepo.GetAllAsync();
            var reviews = await _reviewRepo.GetAllAsync();
            var settingsList = await _settingsRepo.GetAllAsync();
            var settings = settingsList.FirstOrDefault();
            var users = await _userRepo.GetAllAsync();

            var paidBills = bills.Where(b => b.IsPaymentDone);

            var topWaiters = paidBills
                .Where(b => b.GeneratedByWaiterId != null)
                .GroupBy(b => b.GeneratedByWaiterId)
                .Select(g =>
                {
                    var waiter = users.FirstOrDefault(u => u.Id == g.Key);
                    return new TopWaiterDto
                    {
                        WaiterId = g.Key!,
                        WaiterName = waiter?.Name ?? "Unknown",
                        OrdersServed = g.Count()
                    };
                })
                .OrderByDescending(w => w.OrdersServed)
                .Take(5)
                .ToList();

            return new AdminDashboardDto
            {
                TotalTables = tables.Count,
                OccupiedTables = tables.Count(t => t.IsOccupied),
                TotalSales = paidBills.Sum(b => b.GrandTotal),
                TaxPercent = settings?.TaxPercent ?? 0,
                DiscountPercent = settings?.DiscountPercent ?? 0,
                AverageRating = reviews.Any() ? Math.Round(reviews.Average(r => r.Rating), 1) : 0,
                TopWaiters = topWaiters
            };
        }
    }

}

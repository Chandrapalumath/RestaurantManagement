using RestaurantManagement.Dtos.Authentication;

namespace RestaurantManagement.Backend.Services.Interfaces
{
    public interface IAdminDashboardService
    {
        Task<AdminDashboardDto> GetDashboardDataAsync();
    }

}
namespace RestaurantManagement.Dtos.Authentication
{
    public class AuthResponseDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}

namespace RestaurantManagement.Dtos.Tables
{
    public class TableResponseDto
    {
        public Guid Id { get; set; }
        public string TableName { get; set; } = string.Empty;
        public int Size { get; set; }
        public bool IsOccupied { get; set; }
    }
}

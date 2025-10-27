namespace GroceryShop.Api.Models
{
    public class ErrorResponse
    {
        public string? Type { get; set; }
        public string? Message { get; set; }
        public string? Details { get; set; }
        public string? TraceId { get; set; }
    }
}
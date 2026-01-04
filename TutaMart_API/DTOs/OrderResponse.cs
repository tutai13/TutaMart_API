// DTOs/OrderResponse.cs
namespace TutaMart_API.DTOs
{
    public class OrderResponse
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerPhone { get; set; }
        public string PaymentMethod { get; set; } = "Cash";

        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal DiscountAmount { get; set; }
        public string DiscountType { get; set; } = "None";
        public int? DiscountValue { get; set; }

        public decimal TotalAmount { get; set; }

        public List<OrderDetailResponse> OrderDetails { get; set; } = new();
    }

    public class OrderDetailResponse
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
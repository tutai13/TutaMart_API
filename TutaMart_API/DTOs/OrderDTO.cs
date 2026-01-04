namespace TutaMart_API.DTOs
{
    public class OrderDTO
    {
        public string? CustomerName { get; set; }
        public string? CustomerPhone { get; set; }
        public string PaymentMethod { get; set; } = "Cash"; // Cash, QR
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal DiscountAmount { get; set; }
        public string DiscountType { get; set; } = "None";
        public int? DiscountValue { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }

        public List<OrderDetailDTO> OrderDetails { get; set; } = new();
    }
}

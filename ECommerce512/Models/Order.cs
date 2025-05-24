namespace ECommerce512.Models
{
    public enum OrderStatus
    {
        Pending,
        InProcessing,
        Shipped,
        InWay,
        Completed,
        Canceled
    }

    public enum TransactionStatus
    {
        Pending,
        Canceled,
        Completed,
        Refunded
    }

    public class Order
    {
        // Order
        public int Id { get; set; }
        public string ApplicationUserId { get; set; } = null!;
        public ApplicationUser ApplicationUser { get; set; } = null!;

        public DateTime OrderDate { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public decimal TotalPrice { get; set; }

        // Transaction
        public TransactionStatus TransactionStatus { get; set; }
        public string? SessionId { get; set; } 
        public string? PaymentId { get; set; }

        // Carriers
        public DateTime ShippedDate { get; set; }
        public string? Carrier { get; set; }
        public string? CarrierId { get; set; }


    }
}

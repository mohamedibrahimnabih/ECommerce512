using Microsoft.EntityFrameworkCore;

namespace ECommerce512.Models
{
    [PrimaryKey(nameof(ProductId), nameof(OrderId))]
    public class OrderItems
    {
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public decimal ItemPrice { get; set; }
        public int Count { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ECommerce512.Models
{
    [PrimaryKey(nameof(ProductId), nameof(ApplicationUserId))]
    public class Cart
    {
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public string ApplicationUserId { get; set; } = null!;
        public ApplicationUser ApplicationUser { get; set; } = null!;

        [MinLength(0)]
        public int Count { get; set; }
    }
}

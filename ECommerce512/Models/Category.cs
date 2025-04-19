using System.ComponentModel.DataAnnotations;

namespace ECommerce512.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(10)]
        //[Length(3, 50)]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool Status { get; set; }

        public ICollection<Product> Products { get; } = new List<Product>();
    }
}

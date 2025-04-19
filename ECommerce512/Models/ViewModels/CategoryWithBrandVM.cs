namespace ECommerce512.Models.ViewModels
{
    public class CategoryWithBrandVM
    {
        public Product Product { get; set; } = null!;
        public List<Category> Categories { get; set; } = [];
        public List<Brand> Brands { get; set; } = [];
    }
}

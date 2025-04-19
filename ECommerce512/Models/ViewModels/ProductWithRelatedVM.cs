﻿namespace ECommerce512.Models.ViewModels
{
    public class ProductWithRelatedVM
    {
        public Product Product { get; set; } = null!;
        public List<Product> RelatedProducts { get; set; } = null!;
        public List<Product> SameCategory { get; set; } = null!;
        public List<Product> TopProducts { get; set; } = null!;
    }
}

using System;

namespace GroceryShop.Core.DTOs
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public int StockQuantity { get; set; }

        public ProductDto()
        {
        }

        public ProductDto(Guid id, string name, decimal price, string description, int stockQuantity)
        {
            Id = id;
            Name = name;
            Price = price;
            Description = description;
            StockQuantity = stockQuantity;
        }

    }

}
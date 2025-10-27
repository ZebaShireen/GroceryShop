using System;

namespace GroceryShop.Core.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int StockQuantity { get; set; }

        public Product(Guid id, string name, decimal price, string description, int stockQuantity)
        {
            Id = id;
            Name = name;
            Price = price;
            Description = description;
            StockQuantity = stockQuantity;
        }
    }
}
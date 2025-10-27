using GroceryShop.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GroceryShop.Infrastructure.Persistence
{
    public static class SeedData
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Products.Any())
            {
                return;   // DB has been seeded
            }

            var products = new List<Product>
            {
                new Product (new Guid(), "Milk",100,"The best of cows", 100 ),
                new Product (new Guid(), "Bread",20, "Easy toast", 150 ),
                new Product (new Guid(), "Eggs",200,"Wild chicken",  75 )
            };

            context.Products.AddRange(products);
            context.SaveChanges();
        }
    }
}
using System;
using System.Collections.Generic;

namespace GroceryShop.Core.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
        public decimal TotalAmount { get; set; }
        public string ShippingAddress { get; set; }
        public bool IsLoyaltyMember { get; set; }
        public DateTime OrderDate { get; set; }

        public void AddItem(OrderItem item)
        {
            Items.Add(item);
            TotalAmount += item.Price * item.Quantity;
        }
    }

    public class OrderItem
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid ProductId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
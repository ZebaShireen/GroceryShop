using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryShop.Core.DTOs
{
    public class OrderDto
    {
        public Guid CustomerId { get; set; }
        public Guid OrderId { get; set; }

        public List<OrderItemDto> OrderItems { get; set; }
        public decimal TotalAmount { get; set; }
        public string ShippingAddress { get; set; }
    }
    public class OrderItemDto
    {
        public Guid ProductId { get; set; }
    }
}

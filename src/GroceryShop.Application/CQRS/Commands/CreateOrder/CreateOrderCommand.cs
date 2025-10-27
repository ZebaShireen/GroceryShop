using MediatR;
using GroceryShop.Core.DTOs;
using System.Collections.Generic;
using System;

namespace GroceryShop.Application.CQRS.Commands.CreateOrder
{
    public class CreateOrderCommand : IRequest<Guid>
    {
        public Guid CustomerId { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
        public string ShippingAddress { get; set; }
        public bool PurchaseLoyaltyMembership { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class OrderItemDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
    }
}
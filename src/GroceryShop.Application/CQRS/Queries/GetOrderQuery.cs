using GroceryShop.Core.DTOs;
using MediatR;
using System;

namespace GroceryShop.Application.CQRS.Queries
{
    public class GetOrderQuery : IRequest<OrderDto>
    {
        public Guid OrderId { get; }

        public GetOrderQuery(Guid orderId)
        {
            OrderId = orderId;
        }
    }
}
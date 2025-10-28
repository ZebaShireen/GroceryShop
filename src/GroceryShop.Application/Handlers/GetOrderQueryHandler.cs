using System.Threading;
using System.Threading.Tasks;
using GroceryShop.Application.CQRS.Queries;
using GroceryShop.Core.DTOs;
using GroceryShop.Core.Interfaces;
using MediatR;

namespace GroceryShop.Application.CQRS.Handlers
{
    public class GetOrderQueryHandler : IRequestHandler<GetOrderQuery, OrderDto>
    {
        private readonly IOrderRepository _orderRepository;

        public GetOrderQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<OrderDto> Handle(GetOrderQuery request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetOrderByIdAsync(request.OrderId);
            if (order == null)
            {
                return null; // or throw an exception
            }

            return new OrderDto
            {
                OrderId = order.Id,
                //Products = order.Products,
                TotalAmount = order.TotalAmount,
                ShippingAddress = order.ShippingAddress,
                // Map other properties as needed
            };
        }
    }
}
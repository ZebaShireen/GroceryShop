using FluentValidation;
using GroceryShop.Core.Domain.Entities;
using GroceryShop.Core.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GroceryShop.Application.CQRS.Commands.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
    {
        private readonly IValidator<CreateOrderCommand> _validator;
        private readonly IOrderRepository _orderRepository;
        public CreateOrderCommandHandler( IValidator<CreateOrderCommand> validator, IOrderRepository orderRepository)
        {
            _validator = validator;
            _orderRepository = orderRepository;
        }

        public async Task<Guid> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            var order = new Order
            {
                Id = Guid.NewGuid(),
                OrderDate = DateTime.UtcNow,
                CustomerId = command.CustomerId,
                ShippingAddress = command.ShippingAddress,
                IsLoyaltyMember = command.PurchaseLoyaltyMembership
            };

            foreach (var item in command.OrderItems)
            {
                order.AddItem(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price
                });
            }


            // Persist to Database
            return await _orderRepository.AddOrderAsync(order);
        }
    }
}
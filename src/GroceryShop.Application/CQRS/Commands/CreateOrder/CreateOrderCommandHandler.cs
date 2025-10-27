using FluentValidation;
using GroceryShop.Core.Domain.Entities;
using GroceryShop.Core.DTOs;
using GroceryShop.Infrastructure.Repositories;
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
        public CreateOrderCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public CreateOrderCommandHandler(IValidator<CreateOrderCommand> validator)
        {
            _validator = validator;
        }

        public async Task<Guid> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var order = new Order
            {
                Id = Guid.NewGuid(),
                OrderDate = DateTime.UtcNow,
                CustomerId = command.CustomerId,
                ShippingAddress = command.ShippingAddress,
                Items = command.OrderItems?.ConvertAll(item => new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price
                })
            };

            // Persist to Database
            return await _orderRepository.AddOrderAsync(order);

            //return new OrderDto
            //{
            //    OrderId = order.Id,
            //    CustomerId = order.CustomerId,
            //    TotalAmount = order.TotalAmount,
            //    ShippingAddress = order.ShippingAddress,
            //    OrderItems = order.Items?.ConvertAll(item => new GroceryShop.Core.DTOs.OrderItemDto
            //    {
            //        ProductId = item.ProductId,
            //    })
            //};
        }
    }
}
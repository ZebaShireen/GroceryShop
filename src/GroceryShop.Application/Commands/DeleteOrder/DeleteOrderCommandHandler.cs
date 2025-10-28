using FluentValidation;
using GroceryShop.Application.CQRS.Commands.CreateOrder;
using GroceryShop.Application.Exceptions;
using GroceryShop.Core.Domain.Entities;
using GroceryShop.Core.Interfaces;
using GroceryShop.Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GroceryShop.Application.CQRS.Commands.UpdateOrder
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;
        public DeleteOrderCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
       
        public async Task<bool> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetOrderByIdAsync(request.OrderId);

            if (order.CustomerId != request.CustomerId)
                throw new ForbiddenException("You are not allowed to delete this order");

            return await _orderRepository.DeleteOrderAsync(request.OrderId);
        }
    }
}

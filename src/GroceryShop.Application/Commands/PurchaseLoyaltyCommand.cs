using MediatR;
using System;

namespace GroceryShop.Application.CQRS.Commands
{
    public class PurchaseLoyaltyCommand : IRequest<bool>
    {
        public Guid UserId { get; set; }

        public PurchaseLoyaltyCommand(Guid userId)
        {
            UserId = userId;
        }
    }
}
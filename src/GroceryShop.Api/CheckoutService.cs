using GroceryShop.Application.Services;
using GroceryShop.Core.DTOs;

namespace GroceryShop.Api
{
    public class CheckoutService : ICheckoutService
    {
        public void ProcessOrder(OrderDto orderDto)
        {
            throw new System.NotImplementedException();
        }

        public void PurchaseLoyaltyMembership(System.Guid userId)
        {
            throw new System.NotImplementedException();
        }
    }
}
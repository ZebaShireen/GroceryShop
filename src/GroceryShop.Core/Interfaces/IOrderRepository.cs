using GroceryShop.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GroceryShop.Core.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> GetOrderByIdAsync(Guid orderId);
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Guid> AddOrderAsync(Order order);
        Task UpdateOrderAsync(Order order);
        Task<bool> DeleteOrderAsync(Guid orderId);
    }
}
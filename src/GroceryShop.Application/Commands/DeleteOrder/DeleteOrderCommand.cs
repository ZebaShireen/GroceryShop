using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryShop.Application.CQRS.Commands.UpdateOrder
{
    public class DeleteOrderCommand: IRequest<bool>
    {
        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }

        public DeleteOrderCommand(Guid orderId)
        {
            OrderId = orderId;
        }
    }
}

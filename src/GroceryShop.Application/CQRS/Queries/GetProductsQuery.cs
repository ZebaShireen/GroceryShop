using GroceryShop.Core.DTOs;
using MediatR;
using System.Collections.Generic;

namespace GroceryShop.Application.CQRS.Queries
{
    public class GetProductsQuery : IRequest<IEnumerable<ProductDto>>
    {
    }
    public class GetProductByIdQuery : IRequest<ProductDto>
    {
        public int Id { get; set; }
    }
}
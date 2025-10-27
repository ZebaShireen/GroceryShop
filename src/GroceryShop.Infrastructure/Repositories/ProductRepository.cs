using System.Collections.Generic;
using System.Threading.Tasks;
using GroceryShop.Core.Domain.Entities;
using GroceryShop.Core.DTOs;
using Microsoft.EntityFrameworkCore;
using GroceryShop.Infrastructure.Persistence;
using System.Linq;
using System.Threading;

namespace GroceryShop.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync(CancellationToken cancellationToken)
        {
            return await _context.Products
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Description = p.Description,
                    StockQuantity = p.StockQuantity
                })
                .ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task AddProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await GetProductByIdAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
       
    }

    public interface IProductRepository
    {
        Task<IEnumerable<ProductDto>> GetAllProductsAsync( CancellationToken cancellationToken);
    }
}
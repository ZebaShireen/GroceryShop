using GroceryShop.Application.CQRS.Handlers;
using GroceryShop.Application.CQRS.Queries;
using GroceryShop.Core.Domain.Entities;
using GroceryShop.Core.DTOs;
using GroceryShop.Infrastructure.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GroceryShop.Tests.ApplicationTests
{
    public class ProductsQueryTests
    {
        private readonly Mock<GetProductsQueryHandler> _mockHandler;

        public ProductsQueryTests()
        {
            _mockHandler = new Mock<GetProductsQueryHandler>();
        }

        [Fact]
        public async Task GetProducts_ShouldReturnListOfProducts()
        {
            // Arrange
            var mockRepo = new Mock<IProductRepository>();

            var productsDtos = new List<ProductDto>
            {
                new ProductDto (new Guid(), "Apple",200,"test", 100 ),
                new ProductDto (new Guid(), "Banana", 0.3m, "Test", 150 ),
            };

            mockRepo.Setup(r => r.GetAllProductsAsync(It.IsAny<CancellationToken>()))
                    .ReturnsAsync(productsDtos);

            var handler = new GetProductsQueryHandler(mockRepo.Object);

            // Act
            var result = (await handler.Handle(new GetProductsQuery(), default)).ToList();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Apple", result[0].Name);
            Assert.Equal("Banana", result[1].Name);

            mockRepo.Verify(r => r.GetAllProductsAsync(default), Times.Once);
        }


    }
}
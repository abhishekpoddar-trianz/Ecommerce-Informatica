using Xunit;
using Moq;
using EcommerceInformatica.Application.Services;
using EcommerceInformatica.Domain.Entities;
using EcommerceInformatica.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EcommerceInformatica.Application.Services.Tests
{
    public class ProductServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly Mock<ILogger<ProductService>> _mockLogger;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockProductRepository = new Mock<IProductRepository>();
            _mockLogger = new Mock<ILogger<ProductService>>();

            _mockUnitOfWork.Setup(u => u.Products).Returns(_mockProductRepository.Object);
            _productService = new ProductService(_mockUnitOfWork.Object, _mockLogger.Object);
        }

        [Fact]
        public void Constructor_WithNullUnitOfWork_ThrowsArgumentNullException()
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ProductService(null, _mockLogger.Object));
        }

        [Fact]
        public void Constructor_WithNullLogger_ThrowsArgumentNullException()
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ProductService(_mockUnitOfWork.Object, null));
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsProduct_WhenProductExists()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Test Product" };
            _mockProductRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            // Act
            var result = await _productService.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Test Product", result.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenProductDoesNotExist()
        {
            // Arrange
            _mockProductRepository.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product)null);

            // Act
            var result = await _productService.GetByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1" },
                new Product { Id = 2, Name = "Product 2" }
            };
            _mockProductRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(products);

            // Act
            var result = await _productService.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetBySupplierIdAsync_ReturnsProductsForSupplier()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1", SupplierId = 5 },
                new Product { Id = 2, Name = "Product 2", SupplierId = 5 }
            };
            _mockProductRepository.Setup(r => r.GetBySupplierIdAsync(5, It.IsAny<CancellationToken>()))
                .ReturnsAsync(products);

            // Act
            var result = await _productService.GetBySupplierIdAsync(5);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.All(result, p => Assert.Equal(5, p.SupplierId));
        }

        [Fact]
        public async Task GetByBrandIdAsync_ReturnsProductsForBrand()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1", BrandId = 3 }
            };
            _mockProductRepository.Setup(r => r.GetByBrandIdAsync(3, It.IsAny<CancellationToken>()))
                .ReturnsAsync(products);

            // Act
            var result = await _productService.GetByBrandIdAsync(3);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(3, result.First().BrandId);
        }

        [Fact]
        public async Task GetByCategoryIdAsync_ReturnsProductsForCategory()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1", CategoryId = 2 }
            };
            _mockProductRepository.Setup(r => r.GetByCategoryIdAsync(2, It.IsAny<CancellationToken>()))
                .ReturnsAsync(products);

            // Act
            var result = await _productService.GetByCategoryIdAsync(2);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(2, result.First().CategoryId);
        }

        [Fact]
        public async Task GetByArticleIdAsync_ReturnsProduct_WhenArticleIdExists()
        {
            // Arrange
            var product = new Product { Id = 1, ArticleId = "ART001" };
            _mockProductRepository.Setup(r => r.GetByArticleIdAsync("ART001", It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            // Act
            var result = await _productService.GetByArticleIdAsync("ART001");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("ART001", result.ArticleId);
        }

        [Fact]
        public async Task GetActiveProductsAsync_ReturnsOnlyActiveProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Active Product", IsActive = true }
            };
            _mockProductRepository.Setup(r => r.GetActiveProductsAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(products);

            // Act
            var result = await _productService.GetActiveProductsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.All(result, p => Assert.True(p.IsActive));
        }

        [Fact]
        public async Task GetProductsWithLowStockAsync_ReturnsLowStockProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Low Stock Product", Stock = 5 }
            };
            _mockProductRepository.Setup(r => r.GetProductsWithLowStockAsync(10, It.IsAny<CancellationToken>()))
                .ReturnsAsync(products);

            // Act
            var result = await _productService.GetProductsWithLowStockAsync(10);

            // Assert
            Assert.NotNull(result);
            Assert.All(result, p => Assert.True(p.Stock < 10));
        }

        [Fact]
        public async Task CreateAsync_CreatesProduct_WhenArticleIdIsUnique()
        {
            // Arrange
            var product = new Product { ArticleId = "ART001", Name = "New Product" };
            _mockProductRepository.Setup(r => r.GetByArticleIdAsync("ART001", It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product)null);
            _mockProductRepository.Setup(r => r.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            // Act
            var result = await _productService.CreateAsync(product);

            // Assert
            Assert.NotNull(result);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ThrowsException_WhenArticleIdExists()
        {
            // Arrange
            var existingProduct = new Product { ArticleId = "ART001" };
            var newProduct = new Product { ArticleId = "ART001", Name = "New Product" };
            _mockProductRepository.Setup(r => r.GetByArticleIdAsync("ART001", It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingProduct);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _productService.CreateAsync(newProduct));
        }

        [Fact]
        public async Task UpdateAsync_UpdatesProduct_WhenProductExists()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Updated Product" };
            _mockProductRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            // Act
            await _productService.UpdateAsync(product);

            // Assert
            _mockProductRepository.Verify(r => r.UpdateAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ThrowsException_WhenProductDoesNotExist()
        {
            // Arrange
            var product = new Product { Id = 999, Name = "Non-existent Product" };
            _mockProductRepository.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _productService.UpdateAsync(product));
        }

        [Fact]
        public async Task DeleteAsync_DeletesProduct_WhenProductExists()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product to Delete" };
            _mockProductRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            // Act
            await _productService.DeleteAsync(1);

            // Assert
            _mockProductRepository.Verify(r => r.DeleteAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ThrowsException_WhenProductDoesNotExist()
        {
            // Arrange
            _mockProductRepository.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _productService.DeleteAsync(999));
        }

        [Fact]
        public async Task ArticleIdExistsAsync_ReturnsTrue_WhenArticleIdExists()
        {
            // Arrange
            var product = new Product { ArticleId = "ART001" };
            _mockProductRepository.Setup(r => r.GetByArticleIdAsync("ART001", It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            // Act
            var result = await _productService.ArticleIdExistsAsync("ART001");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task ArticleIdExistsAsync_ReturnsFalse_WhenArticleIdDoesNotExist()
        {
            // Arrange
            _mockProductRepository.Setup(r => r.GetByArticleIdAsync("ART999", It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product)null);

            // Act
            var result = await _productService.ArticleIdExistsAsync("ART999");

            // Assert
            Assert.False(result);
        }
    }
}

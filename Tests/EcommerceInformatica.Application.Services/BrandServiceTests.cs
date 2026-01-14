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
    public class BrandServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IBrandRepository> _mockBrandRepository;
        private readonly Mock<ILogger<BrandService>> _mockLogger;
        private readonly BrandService _brandService;

        public BrandServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockBrandRepository = new Mock<IBrandRepository>();
            _mockLogger = new Mock<ILogger<BrandService>>();

            _mockUnitOfWork.Setup(u => u.Brands).Returns(_mockBrandRepository.Object);
            _brandService = new BrandService(_mockUnitOfWork.Object, _mockLogger.Object);
        }

        [Fact]
        public void Constructor_WithNullUnitOfWork_ThrowsArgumentNullException()
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentNullException>(() => new BrandService(null, _mockLogger.Object));
        }

        [Fact]
        public void Constructor_WithNullLogger_ThrowsArgumentNullException()
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentNullException>(() => new BrandService(_mockUnitOfWork.Object, null));
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsBrand_WhenBrandExists()
        {
            // Arrange
            var brand = new Brand { Id = 1, Name = "Test Brand" };
            _mockBrandRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(brand);

            // Act
            var result = await _brandService.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Test Brand", result.Name);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllBrands()
        {
            // Arrange
            var brands = new List<Brand>
            {
                new Brand { Id = 1, Name = "Brand 1" },
                new Brand { Id = 2, Name = "Brand 2" }
            };
            _mockBrandRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(brands);

            // Act
            var result = await _brandService.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetByNameAsync_ReturnsBrand_WhenNameExists()
        {
            // Arrange
            var brand = new Brand { Id = 1, Name = "Test Brand" };
            _mockBrandRepository.Setup(r => r.GetByNameAsync("Test Brand", It.IsAny<CancellationToken>()))
                .ReturnsAsync(brand);

            // Act
            var result = await _brandService.GetByNameAsync("Test Brand");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Brand", result.Name);
        }

        [Fact]
        public async Task GetActiveBrandsAsync_ReturnsOnlyActiveBrands()
        {
            // Arrange
            var brands = new List<Brand>
            {
                new Brand { Id = 1, Name = "Active Brand", IsActive = true }
            };
            _mockBrandRepository.Setup(r => r.GetActiveBrandsAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(brands);

            // Act
            var result = await _brandService.GetActiveBrandsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.All(result, b => Assert.True(b.IsActive));
        }

        [Fact]
        public async Task GetBrandsWithProductsAsync_ReturnsBrandsWithProducts()
        {
            // Arrange
            var brands = new List<Brand>
            {
                new Brand { Id = 1, Name = "Brand With Products" }
            };
            _mockBrandRepository.Setup(r => r.GetBrandsWithProductsAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(brands);

            // Act
            var result = await _brandService.GetBrandsWithProductsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task CreateAsync_CreatesBrand()
        {
            // Arrange
            var brand = new Brand { Name = "New Brand" };
            _mockBrandRepository.Setup(r => r.AddAsync(It.IsAny<Brand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(brand);

            // Act
            var result = await _brandService.CreateAsync(brand);

            // Assert
            Assert.NotNull(result);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesBrand()
        {
            // Arrange
            var brand = new Brand { Id = 1, Name = "Updated Brand" };

            // Act
            await _brandService.UpdateAsync(brand);

            // Assert
            _mockBrandRepository.Verify(r => r.UpdateAsync(It.IsAny<Brand>(), It.IsAny<CancellationToken>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_DeletesBrand_WhenBrandExists()
        {
            // Arrange
            var brand = new Brand { Id = 1, Name = "Brand to Delete" };
            _mockBrandRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(brand);

            // Act
            await _brandService.DeleteAsync(1);

            // Assert
            _mockBrandRepository.Verify(r => r.DeleteAsync(It.IsAny<Brand>(), It.IsAny<CancellationToken>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}

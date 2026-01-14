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
    public class CategoryServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ICategoryRepository> _mockCategoryRepository;
        private readonly Mock<ILogger<CategoryService>> _mockLogger;
        private readonly CategoryService _categoryService;

        public CategoryServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockCategoryRepository = new Mock<ICategoryRepository>();
            _mockLogger = new Mock<ILogger<CategoryService>>();

            _mockUnitOfWork.Setup(u => u.Categories).Returns(_mockCategoryRepository.Object);
            _categoryService = new CategoryService(_mockUnitOfWork.Object, _mockLogger.Object);
        }

        [Fact]
        public void Constructor_WithNullUnitOfWork_ThrowsArgumentNullException()
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentNullException>(() => new CategoryService(null, _mockLogger.Object));
        }

        [Fact]
        public void Constructor_WithNullLogger_ThrowsArgumentNullException()
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentNullException>(() => new CategoryService(_mockUnitOfWork.Object, null));
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCategory_WhenCategoryExists()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Electronics" };
            _mockCategoryRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(category);

            // Act
            var result = await _categoryService.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Electronics", result.Name);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllCategories()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Category 1" },
                new Category { Id = 2, Name = "Category 2" }
            };
            _mockCategoryRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(categories);

            // Act
            var result = await _categoryService.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetByNameAsync_ReturnsCategory_WhenNameExists()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Electronics" };
            _mockCategoryRepository.Setup(r => r.GetByNameAsync("Electronics", It.IsAny<CancellationToken>()))
                .ReturnsAsync(category);

            // Act
            var result = await _categoryService.GetByNameAsync("Electronics");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Electronics", result.Name);
        }

        [Fact]
        public async Task GetActiveCategoriesAsync_ReturnsOnlyActiveCategories()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Active Category", IsActive = true }
            };
            _mockCategoryRepository.Setup(r => r.GetActiveCategoriesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(categories);

            // Act
            var result = await _categoryService.GetActiveCategoriesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.All(result, c => Assert.True(c.IsActive));
        }

        [Fact]
        public async Task GetCategoriesWithProductsAsync_ReturnsCategoriesWithProducts()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Category With Products" }
            };
            _mockCategoryRepository.Setup(r => r.GetCategoriesWithProductsAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(categories);

            // Act
            var result = await _categoryService.GetCategoriesWithProductsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task CreateAsync_CreatesCategory()
        {
            // Arrange
            var category = new Category { Name = "New Category" };
            _mockCategoryRepository.Setup(r => r.AddAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(category);

            // Act
            var result = await _categoryService.CreateAsync(category);

            // Assert
            Assert.NotNull(result);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesCategory()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Updated Category" };

            // Act
            await _categoryService.UpdateAsync(category);

            // Assert
            _mockCategoryRepository.Verify(r => r.UpdateAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_DeletesCategory_WhenCategoryExists()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Category to Delete" };
            _mockCategoryRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(category);

            // Act
            await _categoryService.DeleteAsync(1);

            // Assert
            _mockCategoryRepository.Verify(r => r.DeleteAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}

using Xunit;
using EcommerceInformatica.Domain.Entities;
using System;
using System.Collections.Generic;

namespace EcommerceInformatica.Domain.Entities.Tests
{
    public class ProductTests
    {
        [Fact]
        public void Product_Constructor_InitializesWithDefaultValues()
        {
            // Arrange & Act
            var product = new Product();

            // Assert
            Assert.Equal(0, product.Id);
            Assert.Equal(string.Empty, product.ArticleId);
            Assert.Equal(0, product.SupplierId);
            Assert.Equal(0, product.BrandId);
            Assert.Equal(0, product.CategoryId);
            Assert.Equal(string.Empty, product.Name);
            Assert.Equal(0, product.Stock);
            Assert.Equal(0, product.UnitPrice);
            Assert.False(product.IsActive);
            Assert.Equal(string.Empty, product.CreatedBy);
            Assert.NotNull(product.InvoiceDetails);
        }

        [Fact]
        public void Product_SetProperties_ReturnsCorrectValues()
        {
            // Arrange
            var product = new Product();
            var testDate = DateTime.Now;

            // Act
            product.Id = 1;
            product.ArticleId = "ART001";
            product.SupplierId = 10;
            product.BrandId = 5;
            product.CategoryId = 3;
            product.Name = "Test Product";
            product.Stock = 100;
            product.UnitPrice = 29.99m;
            product.IsActive = true;
            product.CreatedDate = testDate;
            product.ModifiedDate = testDate;
            product.CreatedBy = "admin";
            product.ModifiedBy = "user1";

            // Assert
            Assert.Equal(1, product.Id);
            Assert.Equal("ART001", product.ArticleId);
            Assert.Equal(10, product.SupplierId);
            Assert.Equal(5, product.BrandId);
            Assert.Equal(3, product.CategoryId);
            Assert.Equal("Test Product", product.Name);
            Assert.Equal(100, product.Stock);
            Assert.Equal(29.99m, product.UnitPrice);
            Assert.True(product.IsActive);
            Assert.Equal(testDate, product.CreatedDate);
            Assert.Equal(testDate, product.ModifiedDate);
            Assert.Equal("admin", product.CreatedBy);
            Assert.Equal("user1", product.ModifiedBy);
        }

        [Fact]
        public void Product_NavigationProperties_CanBeSet()
        {
            // Arrange
            var product = new Product();
            var supplier = new Supplier { Id = 1, Name = "Test Supplier" };
            var brand = new Brand { Id = 1, Name = "Test Brand" };
            var category = new Category { Id = 1, Name = "Test Category" };

            // Act
            product.Supplier = supplier;
            product.Brand = brand;
            product.Category = category;

            // Assert
            Assert.NotNull(product.Supplier);
            Assert.Equal(1, product.Supplier.Id);
            Assert.NotNull(product.Brand);
            Assert.Equal(1, product.Brand.Id);
            Assert.NotNull(product.Category);
            Assert.Equal(1, product.Category.Id);
        }

        [Fact]
        public void Product_InvoiceDetails_InitializesAsEmptyCollection()
        {
            // Arrange & Act
            var product = new Product();

            // Assert
            Assert.NotNull(product.InvoiceDetails);
            Assert.Empty(product.InvoiceDetails);
        }

        [Fact]
        public void Product_UnitPrice_AcceptsDecimalValues()
        {
            // Arrange
            var product = new Product();

            // Act
            product.UnitPrice = 99.99m;

            // Assert
            Assert.Equal(99.99m, product.UnitPrice);
        }

        [Fact]
        public void Product_Stock_AcceptsNegativeValues()
        {
            // Arrange
            var product = new Product();

            // Act
            product.Stock = -10;

            // Assert
            Assert.Equal(-10, product.Stock);
        }

        [Fact]
        public void Product_ModifiedDate_CanBeNull()
        {
            // Arrange
            var product = new Product();

            // Act
            product.ModifiedDate = null;

            // Assert
            Assert.Null(product.ModifiedDate);
        }

        [Fact]
        public void Product_ModifiedBy_CanBeNull()
        {
            // Arrange
            var product = new Product();

            // Act
            product.ModifiedBy = null;

            // Assert
            Assert.Null(product.ModifiedBy);
        }
    }
}

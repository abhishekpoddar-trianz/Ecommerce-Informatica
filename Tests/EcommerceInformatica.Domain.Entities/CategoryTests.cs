using Xunit;
using EcommerceInformatica.Domain.Entities;
using System;

namespace EcommerceInformatica.Domain.Entities.Tests
{
    public class CategoryTests
    {
        [Fact]
        public void Category_Constructor_InitializesWithDefaultValues()
        {
            // Arrange & Act
            var category = new Category();

            // Assert
            Assert.Equal(0, category.Id);
            Assert.Equal(string.Empty, category.Name);
            Assert.False(category.IsActive);
            Assert.Equal(string.Empty, category.CreatedBy);
            Assert.NotNull(category.Products);
        }

        [Fact]
        public void Category_SetProperties_ReturnsCorrectValues()
        {
            // Arrange
            var category = new Category();
            var testDate = DateTime.Now;

            // Act
            category.Id = 1;
            category.Name = "Electronics";
            category.Description = "Electronic products";
            category.IsActive = true;
            category.CreatedDate = testDate;
            category.ModifiedDate = testDate;
            category.CreatedBy = "admin";
            category.ModifiedBy = "user1";

            // Assert
            Assert.Equal(1, category.Id);
            Assert.Equal("Electronics", category.Name);
            Assert.Equal("Electronic products", category.Description);
            Assert.True(category.IsActive);
            Assert.Equal(testDate, category.CreatedDate);
            Assert.Equal(testDate, category.ModifiedDate);
            Assert.Equal("admin", category.CreatedBy);
            Assert.Equal("user1", category.ModifiedBy);
        }

        [Fact]
        public void Category_Products_InitializesAsEmptyCollection()
        {
            // Arrange & Act
            var category = new Category();

            // Assert
            Assert.NotNull(category.Products);
            Assert.Empty(category.Products);
        }

        [Fact]
        public void Category_Description_CanBeNull()
        {
            // Arrange
            var category = new Category();

            // Act
            category.Description = null;

            // Assert
            Assert.Null(category.Description);
        }

        [Fact]
        public void Category_ModifiedDate_CanBeNull()
        {
            // Arrange
            var category = new Category();

            // Act
            category.ModifiedDate = null;

            // Assert
            Assert.Null(category.ModifiedDate);
        }

        [Fact]
        public void Category_ModifiedBy_CanBeNull()
        {
            // Arrange
            var category = new Category();

            // Act
            category.ModifiedBy = null;

            // Assert
            Assert.Null(category.ModifiedBy);
        }

        [Fact]
        public void Category_IsActive_DefaultsToFalse()
        {
            // Arrange & Act
            var category = new Category();

            // Assert
            Assert.False(category.IsActive);
        }
    }
}

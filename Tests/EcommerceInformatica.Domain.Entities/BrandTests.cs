using Xunit;
using EcommerceInformatica.Domain.Entities;
using System;

namespace EcommerceInformatica.Domain.Entities.Tests
{
    public class BrandTests
    {
        [Fact]
        public void Brand_Constructor_InitializesWithDefaultValues()
        {
            // Arrange & Act
            var brand = new Brand();

            // Assert
            Assert.Equal(0, brand.Id);
            Assert.Equal(string.Empty, brand.Name);
            Assert.False(brand.IsActive);
            Assert.Equal(string.Empty, brand.CreatedBy);
            Assert.NotNull(brand.Products);
        }

        [Fact]
        public void Brand_SetProperties_ReturnsCorrectValues()
        {
            // Arrange
            var brand = new Brand();
            var testDate = DateTime.Now;

            // Act
            brand.Id = 1;
            brand.Name = "Test Brand";
            brand.Description = "A test brand description";
            brand.IsActive = true;
            brand.CreatedDate = testDate;
            brand.ModifiedDate = testDate;
            brand.CreatedBy = "admin";
            brand.ModifiedBy = "user1";

            // Assert
            Assert.Equal(1, brand.Id);
            Assert.Equal("Test Brand", brand.Name);
            Assert.Equal("A test brand description", brand.Description);
            Assert.True(brand.IsActive);
            Assert.Equal(testDate, brand.CreatedDate);
            Assert.Equal(testDate, brand.ModifiedDate);
            Assert.Equal("admin", brand.CreatedBy);
            Assert.Equal("user1", brand.ModifiedBy);
        }

        [Fact]
        public void Brand_Products_InitializesAsEmptyCollection()
        {
            // Arrange & Act
            var brand = new Brand();

            // Assert
            Assert.NotNull(brand.Products);
            Assert.Empty(brand.Products);
        }

        [Fact]
        public void Brand_Description_CanBeNull()
        {
            // Arrange
            var brand = new Brand();

            // Act
            brand.Description = null;

            // Assert
            Assert.Null(brand.Description);
        }

        [Fact]
        public void Brand_ModifiedDate_CanBeNull()
        {
            // Arrange
            var brand = new Brand();

            // Act
            brand.ModifiedDate = null;

            // Assert
            Assert.Null(brand.ModifiedDate);
        }

        [Fact]
        public void Brand_ModifiedBy_CanBeNull()
        {
            // Arrange
            var brand = new Brand();

            // Act
            brand.ModifiedBy = null;

            // Assert
            Assert.Null(brand.ModifiedBy);
        }

        [Fact]
        public void Brand_IsActive_DefaultsToFalse()
        {
            // Arrange & Act
            var brand = new Brand();

            // Assert
            Assert.False(brand.IsActive);
        }
    }
}

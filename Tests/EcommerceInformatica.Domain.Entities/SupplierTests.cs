using Xunit;
using EcommerceInformatica.Domain.Entities;
using System;

namespace EcommerceInformatica.Domain.Entities.Tests
{
    public class SupplierTests
    {
        [Fact]
        public void Supplier_Constructor_InitializesWithDefaultValues()
        {
            // Arrange & Act
            var supplier = new Supplier();

            // Assert
            Assert.Equal(0, supplier.Id);
            Assert.Equal(string.Empty, supplier.Name);
            Assert.False(supplier.IsActive);
            Assert.Equal(string.Empty, supplier.CreatedBy);
            Assert.NotNull(supplier.Products);
        }

        [Fact]
        public void Supplier_SetProperties_ReturnsCorrectValues()
        {
            // Arrange
            var supplier = new Supplier();
            var testDate = DateTime.Now;

            // Act
            supplier.Id = 1;
            supplier.Name = "Test Supplier";
            supplier.ContactPerson = "Jane Smith";
            supplier.Email = "supplier@example.com";
            supplier.Phone = "555-1234";
            supplier.Address = "456 Supplier St";
            supplier.IsActive = true;
            supplier.CreatedDate = testDate;
            supplier.ModifiedDate = testDate;
            supplier.CreatedBy = "admin";
            supplier.ModifiedBy = "user1";

            // Assert
            Assert.Equal(1, supplier.Id);
            Assert.Equal("Test Supplier", supplier.Name);
            Assert.Equal("Jane Smith", supplier.ContactPerson);
            Assert.Equal("supplier@example.com", supplier.Email);
            Assert.Equal("555-1234", supplier.Phone);
            Assert.Equal("456 Supplier St", supplier.Address);
            Assert.True(supplier.IsActive);
            Assert.Equal(testDate, supplier.CreatedDate);
            Assert.Equal(testDate, supplier.ModifiedDate);
            Assert.Equal("admin", supplier.CreatedBy);
            Assert.Equal("user1", supplier.ModifiedBy);
        }

        [Fact]
        public void Supplier_Products_InitializesAsEmptyCollection()
        {
            // Arrange & Act
            var supplier = new Supplier();

            // Assert
            Assert.NotNull(supplier.Products);
            Assert.Empty(supplier.Products);
        }

        [Fact]
        public void Supplier_ContactPerson_CanBeNull()
        {
            // Arrange
            var supplier = new Supplier();

            // Act
            supplier.ContactPerson = null;

            // Assert
            Assert.Null(supplier.ContactPerson);
        }

        [Fact]
        public void Supplier_Email_CanBeNull()
        {
            // Arrange
            var supplier = new Supplier();

            // Act
            supplier.Email = null;

            // Assert
            Assert.Null(supplier.Email);
        }

        [Fact]
        public void Supplier_Phone_CanBeNull()
        {
            // Arrange
            var supplier = new Supplier();

            // Act
            supplier.Phone = null;

            // Assert
            Assert.Null(supplier.Phone);
        }

        [Fact]
        public void Supplier_Address_CanBeNull()
        {
            // Arrange
            var supplier = new Supplier();

            // Act
            supplier.Address = null;

            // Assert
            Assert.Null(supplier.Address);
        }

        [Fact]
        public void Supplier_ModifiedDate_CanBeNull()
        {
            // Arrange
            var supplier = new Supplier();

            // Act
            supplier.ModifiedDate = null;

            // Assert
            Assert.Null(supplier.ModifiedDate);
        }

        [Fact]
        public void Supplier_ModifiedBy_CanBeNull()
        {
            // Arrange
            var supplier = new Supplier();

            // Act
            supplier.ModifiedBy = null;

            // Assert
            Assert.Null(supplier.ModifiedBy);
        }
    }
}

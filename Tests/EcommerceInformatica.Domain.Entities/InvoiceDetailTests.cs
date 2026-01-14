using Xunit;
using EcommerceInformatica.Domain.Entities;
using System;

namespace EcommerceInformatica.Domain.Entities.Tests
{
    public class InvoiceDetailTests
    {
        [Fact]
        public void InvoiceDetail_Constructor_InitializesWithDefaultValues()
        {
            // Arrange & Act
            var invoiceDetail = new InvoiceDetail();

            // Assert
            Assert.Equal(0, invoiceDetail.Id);
            Assert.Equal(0, invoiceDetail.InvoiceId);
            Assert.Equal(0, invoiceDetail.ProductId);
            Assert.Equal(0, invoiceDetail.Quantity);
            Assert.Equal(0, invoiceDetail.UnitPrice);
            Assert.Equal(0, invoiceDetail.Subtotal);
            Assert.False(invoiceDetail.IsActive);
            Assert.Equal(string.Empty, invoiceDetail.CreatedBy);
        }

        [Fact]
        public void InvoiceDetail_SetProperties_ReturnsCorrectValues()
        {
            // Arrange
            var invoiceDetail = new InvoiceDetail();
            var testDate = DateTime.Now;

            // Act
            invoiceDetail.Id = 1;
            invoiceDetail.InvoiceId = 10;
            invoiceDetail.ProductId = 5;
            invoiceDetail.Quantity = 3;
            invoiceDetail.UnitPrice = 25.99m;
            invoiceDetail.Subtotal = 77.97m;
            invoiceDetail.IsActive = true;
            invoiceDetail.CreatedDate = testDate;
            invoiceDetail.ModifiedDate = testDate;
            invoiceDetail.CreatedBy = "admin";
            invoiceDetail.ModifiedBy = "user1";

            // Assert
            Assert.Equal(1, invoiceDetail.Id);
            Assert.Equal(10, invoiceDetail.InvoiceId);
            Assert.Equal(5, invoiceDetail.ProductId);
            Assert.Equal(3, invoiceDetail.Quantity);
            Assert.Equal(25.99m, invoiceDetail.UnitPrice);
            Assert.Equal(77.97m, invoiceDetail.Subtotal);
            Assert.True(invoiceDetail.IsActive);
            Assert.Equal(testDate, invoiceDetail.CreatedDate);
            Assert.Equal(testDate, invoiceDetail.ModifiedDate);
            Assert.Equal("admin", invoiceDetail.CreatedBy);
            Assert.Equal("user1", invoiceDetail.ModifiedBy);
        }

        [Fact]
        public void InvoiceDetail_NavigationProperties_CanBeSet()
        {
            // Arrange
            var invoiceDetail = new InvoiceDetail();
            var invoice = new Invoice { Id = 1, InvoiceNumber = "INV-001" };
            var product = new Product { Id = 1, Name = "Test Product" };

            // Act
            invoiceDetail.Invoice = invoice;
            invoiceDetail.Product = product;

            // Assert
            Assert.NotNull(invoiceDetail.Invoice);
            Assert.Equal(1, invoiceDetail.Invoice.Id);
            Assert.NotNull(invoiceDetail.Product);
            Assert.Equal(1, invoiceDetail.Product.Id);
        }

        [Fact]
        public void InvoiceDetail_UnitPrice_AcceptsDecimalValues()
        {
            // Arrange
            var invoiceDetail = new InvoiceDetail();

            // Act
            invoiceDetail.UnitPrice = 49.99m;

            // Assert
            Assert.Equal(49.99m, invoiceDetail.UnitPrice);
        }

        [Fact]
        public void InvoiceDetail_Subtotal_AcceptsDecimalValues()
        {
            // Arrange
            var invoiceDetail = new InvoiceDetail();

            // Act
            invoiceDetail.Subtotal = 149.97m;

            // Assert
            Assert.Equal(149.97m, invoiceDetail.Subtotal);
        }

        [Fact]
        public void InvoiceDetail_Quantity_AcceptsPositiveValues()
        {
            // Arrange
            var invoiceDetail = new InvoiceDetail();

            // Act
            invoiceDetail.Quantity = 5;

            // Assert
            Assert.Equal(5, invoiceDetail.Quantity);
        }

        [Fact]
        public void InvoiceDetail_ModifiedDate_CanBeNull()
        {
            // Arrange
            var invoiceDetail = new InvoiceDetail();

            // Act
            invoiceDetail.ModifiedDate = null;

            // Assert
            Assert.Null(invoiceDetail.ModifiedDate);
        }

        [Fact]
        public void InvoiceDetail_ModifiedBy_CanBeNull()
        {
            // Arrange
            var invoiceDetail = new InvoiceDetail();

            // Act
            invoiceDetail.ModifiedBy = null;

            // Assert
            Assert.Null(invoiceDetail.ModifiedBy);
        }
    }
}

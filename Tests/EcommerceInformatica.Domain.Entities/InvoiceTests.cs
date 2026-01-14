using Xunit;
using EcommerceInformatica.Domain.Entities;
using System;

namespace EcommerceInformatica.Domain.Entities.Tests
{
    public class InvoiceTests
    {
        [Fact]
        public void Invoice_Constructor_InitializesWithDefaultValues()
        {
            // Arrange & Act
            var invoice = new Invoice();

            // Assert
            Assert.Equal(0, invoice.Id);
            Assert.Equal(string.Empty, invoice.InvoiceNumber);
            Assert.Equal(0, invoice.PersonId);
            Assert.Equal(0, invoice.PaymentMethodId);
            Assert.Equal(0, invoice.TotalAmount);
            Assert.False(invoice.IsActive);
            Assert.Equal(string.Empty, invoice.CreatedBy);
            Assert.NotNull(invoice.InvoiceDetails);
        }

        [Fact]
        public void Invoice_SetProperties_ReturnsCorrectValues()
        {
            // Arrange
            var invoice = new Invoice();
            var testDate = DateTime.Now;

            // Act
            invoice.Id = 1;
            invoice.InvoiceNumber = "INV-001";
            invoice.PersonId = 10;
            invoice.PaymentMethodId = 2;
            invoice.InvoiceDate = testDate;
            invoice.TotalAmount = 150.50m;
            invoice.IsActive = true;
            invoice.CreatedDate = testDate;
            invoice.ModifiedDate = testDate;
            invoice.CreatedBy = "admin";
            invoice.ModifiedBy = "user1";

            // Assert
            Assert.Equal(1, invoice.Id);
            Assert.Equal("INV-001", invoice.InvoiceNumber);
            Assert.Equal(10, invoice.PersonId);
            Assert.Equal(2, invoice.PaymentMethodId);
            Assert.Equal(testDate, invoice.InvoiceDate);
            Assert.Equal(150.50m, invoice.TotalAmount);
            Assert.True(invoice.IsActive);
            Assert.Equal(testDate, invoice.CreatedDate);
            Assert.Equal(testDate, invoice.ModifiedDate);
            Assert.Equal("admin", invoice.CreatedBy);
            Assert.Equal("user1", invoice.ModifiedBy);
        }

        [Fact]
        public void Invoice_NavigationProperties_CanBeSet()
        {
            // Arrange
            var invoice = new Invoice();
            var person = new Person { Id = 1, FirstName = "John" };
            var paymentMethod = new PaymentMethod { Id = 1, Name = "Credit Card" };

            // Act
            invoice.Person = person;
            invoice.PaymentMethod = paymentMethod;

            // Assert
            Assert.NotNull(invoice.Person);
            Assert.Equal(1, invoice.Person.Id);
            Assert.NotNull(invoice.PaymentMethod);
            Assert.Equal(1, invoice.PaymentMethod.Id);
        }

        [Fact]
        public void Invoice_InvoiceDetails_InitializesAsEmptyCollection()
        {
            // Arrange & Act
            var invoice = new Invoice();

            // Assert
            Assert.NotNull(invoice.InvoiceDetails);
            Assert.Empty(invoice.InvoiceDetails);
        }

        [Fact]
        public void Invoice_TotalAmount_AcceptsDecimalValues()
        {
            // Arrange
            var invoice = new Invoice();

            // Act
            invoice.TotalAmount = 999.99m;

            // Assert
            Assert.Equal(999.99m, invoice.TotalAmount);
        }

        [Fact]
        public void Invoice_ModifiedDate_CanBeNull()
        {
            // Arrange
            var invoice = new Invoice();

            // Act
            invoice.ModifiedDate = null;

            // Assert
            Assert.Null(invoice.ModifiedDate);
        }

        [Fact]
        public void Invoice_ModifiedBy_CanBeNull()
        {
            // Arrange
            var invoice = new Invoice();

            // Act
            invoice.ModifiedBy = null;

            // Assert
            Assert.Null(invoice.ModifiedBy);
        }
    }
}

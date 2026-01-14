using Xunit;
using EcommerceInformatica.Domain.Entities;
using System;

namespace EcommerceInformatica.Domain.Entities.Tests
{
    public class PaymentMethodTests
    {
        [Fact]
        public void PaymentMethod_Constructor_InitializesWithDefaultValues()
        {
            // Arrange & Act
            var paymentMethod = new PaymentMethod();

            // Assert
            Assert.Equal(0, paymentMethod.Id);
            Assert.Equal(string.Empty, paymentMethod.Name);
            Assert.False(paymentMethod.IsActive);
            Assert.Equal(string.Empty, paymentMethod.CreatedBy);
            Assert.NotNull(paymentMethod.Invoices);
        }

        [Fact]
        public void PaymentMethod_SetProperties_ReturnsCorrectValues()
        {
            // Arrange
            var paymentMethod = new PaymentMethod();
            var testDate = DateTime.Now;

            // Act
            paymentMethod.Id = 1;
            paymentMethod.Name = "Credit Card";
            paymentMethod.Description = "Visa/Mastercard payments";
            paymentMethod.IsActive = true;
            paymentMethod.CreatedDate = testDate;
            paymentMethod.ModifiedDate = testDate;
            paymentMethod.CreatedBy = "admin";
            paymentMethod.ModifiedBy = "user1";

            // Assert
            Assert.Equal(1, paymentMethod.Id);
            Assert.Equal("Credit Card", paymentMethod.Name);
            Assert.Equal("Visa/Mastercard payments", paymentMethod.Description);
            Assert.True(paymentMethod.IsActive);
            Assert.Equal(testDate, paymentMethod.CreatedDate);
            Assert.Equal(testDate, paymentMethod.ModifiedDate);
            Assert.Equal("admin", paymentMethod.CreatedBy);
            Assert.Equal("user1", paymentMethod.ModifiedBy);
        }

        [Fact]
        public void PaymentMethod_Invoices_InitializesAsEmptyCollection()
        {
            // Arrange & Act
            var paymentMethod = new PaymentMethod();

            // Assert
            Assert.NotNull(paymentMethod.Invoices);
            Assert.Empty(paymentMethod.Invoices);
        }

        [Fact]
        public void PaymentMethod_Description_CanBeNull()
        {
            // Arrange
            var paymentMethod = new PaymentMethod();

            // Act
            paymentMethod.Description = null;

            // Assert
            Assert.Null(paymentMethod.Description);
        }

        [Fact]
        public void PaymentMethod_ModifiedDate_CanBeNull()
        {
            // Arrange
            var paymentMethod = new PaymentMethod();

            // Act
            paymentMethod.ModifiedDate = null;

            // Assert
            Assert.Null(paymentMethod.ModifiedDate);
        }

        [Fact]
        public void PaymentMethod_ModifiedBy_CanBeNull()
        {
            // Arrange
            var paymentMethod = new PaymentMethod();

            // Act
            paymentMethod.ModifiedBy = null;

            // Assert
            Assert.Null(paymentMethod.ModifiedBy);
        }

        [Fact]
        public void PaymentMethod_IsActive_DefaultsToFalse()
        {
            // Arrange & Act
            var paymentMethod = new PaymentMethod();

            // Assert
            Assert.False(paymentMethod.IsActive);
        }
    }
}

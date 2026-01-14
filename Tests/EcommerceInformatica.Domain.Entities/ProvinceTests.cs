using Xunit;
using EcommerceInformatica.Domain.Entities;
using System;

namespace EcommerceInformatica.Domain.Entities.Tests
{
    public class ProvinceTests
    {
        [Fact]
        public void Province_Constructor_InitializesWithDefaultValues()
        {
            // Arrange & Act
            var province = new Province();

            // Assert
            Assert.Equal(0, province.Id);
            Assert.Equal(string.Empty, province.Name);
            Assert.False(province.IsActive);
            Assert.Equal(string.Empty, province.CreatedBy);
            Assert.NotNull(province.Cities);
        }

        [Fact]
        public void Province_SetProperties_ReturnsCorrectValues()
        {
            // Arrange
            var province = new Province();
            var testDate = DateTime.Now;

            // Act
            province.Id = 1;
            province.Name = "Test Province";
            province.IsActive = true;
            province.CreatedDate = testDate;
            province.ModifiedDate = testDate;
            province.CreatedBy = "admin";
            province.ModifiedBy = "user1";

            // Assert
            Assert.Equal(1, province.Id);
            Assert.Equal("Test Province", province.Name);
            Assert.True(province.IsActive);
            Assert.Equal(testDate, province.CreatedDate);
            Assert.Equal(testDate, province.ModifiedDate);
            Assert.Equal("admin", province.CreatedBy);
            Assert.Equal("user1", province.ModifiedBy);
        }

        [Fact]
        public void Province_Cities_InitializesAsEmptyCollection()
        {
            // Arrange & Act
            var province = new Province();

            // Assert
            Assert.NotNull(province.Cities);
            Assert.Empty(province.Cities);
        }

        [Fact]
        public void Province_ModifiedDate_CanBeNull()
        {
            // Arrange
            var province = new Province();

            // Act
            province.ModifiedDate = null;

            // Assert
            Assert.Null(province.ModifiedDate);
        }

        [Fact]
        public void Province_ModifiedBy_CanBeNull()
        {
            // Arrange
            var province = new Province();

            // Act
            province.ModifiedBy = null;

            // Assert
            Assert.Null(province.ModifiedBy);
        }

        [Fact]
        public void Province_IsActive_DefaultsToFalse()
        {
            // Arrange & Act
            var province = new Province();

            // Assert
            Assert.False(province.IsActive);
        }
    }
}

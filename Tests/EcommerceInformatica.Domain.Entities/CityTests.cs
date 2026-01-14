using Xunit;
using EcommerceInformatica.Domain.Entities;
using System;

namespace EcommerceInformatica.Domain.Entities.Tests
{
    public class CityTests
    {
        [Fact]
        public void City_Constructor_InitializesWithDefaultValues()
        {
            // Arrange & Act
            var city = new City();

            // Assert
            Assert.Equal(0, city.Id);
            Assert.Equal(string.Empty, city.Name);
            Assert.Equal(0, city.ProvinceId);
            Assert.False(city.IsActive);
            Assert.Equal(string.Empty, city.CreatedBy);
            Assert.NotNull(city.People);
        }

        [Fact]
        public void City_SetProperties_ReturnsCorrectValues()
        {
            // Arrange
            var city = new City();
            var testDate = DateTime.Now;

            // Act
            city.Id = 1;
            city.Name = "Test City";
            city.ProvinceId = 5;
            city.IsActive = true;
            city.CreatedDate = testDate;
            city.ModifiedDate = testDate;
            city.CreatedBy = "admin";
            city.ModifiedBy = "user1";

            // Assert
            Assert.Equal(1, city.Id);
            Assert.Equal("Test City", city.Name);
            Assert.Equal(5, city.ProvinceId);
            Assert.True(city.IsActive);
            Assert.Equal(testDate, city.CreatedDate);
            Assert.Equal(testDate, city.ModifiedDate);
            Assert.Equal("admin", city.CreatedBy);
            Assert.Equal("user1", city.ModifiedBy);
        }

        [Fact]
        public void City_NavigationProperty_Province_CanBeSet()
        {
            // Arrange
            var city = new City();
            var province = new Province { Id = 1, Name = "Test Province" };

            // Act
            city.Province = province;

            // Assert
            Assert.NotNull(city.Province);
            Assert.Equal(1, city.Province.Id);
            Assert.Equal("Test Province", city.Province.Name);
        }

        [Fact]
        public void City_People_InitializesAsEmptyCollection()
        {
            // Arrange & Act
            var city = new City();

            // Assert
            Assert.NotNull(city.People);
            Assert.Empty(city.People);
        }

        [Fact]
        public void City_ModifiedDate_CanBeNull()
        {
            // Arrange
            var city = new City();

            // Act
            city.ModifiedDate = null;

            // Assert
            Assert.Null(city.ModifiedDate);
        }

        [Fact]
        public void City_ModifiedBy_CanBeNull()
        {
            // Arrange
            var city = new City();

            // Act
            city.ModifiedBy = null;

            // Assert
            Assert.Null(city.ModifiedBy);
        }

        [Fact]
        public void City_IsActive_DefaultsToFalse()
        {
            // Arrange & Act
            var city = new City();

            // Assert
            Assert.False(city.IsActive);
        }
    }
}

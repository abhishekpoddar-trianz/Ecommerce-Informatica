using Xunit;
using EcommerceInformatica.Domain.Entities;
using System;
using System.Collections.Generic;

namespace EcommerceInformatica.Domain.Entities.Tests
{
    public class PersonTests
    {
        [Fact]
        public void Person_Constructor_InitializesWithDefaultValues()
        {
            // Arrange & Act
            var person = new Person();

            // Assert
            Assert.Equal(0, person.Id);
            Assert.Equal(string.Empty, person.FirstName);
            Assert.Equal(string.Empty, person.LastName);
            Assert.Equal(string.Empty, person.Email);
            Assert.Equal(string.Empty, person.Username);
            Assert.Equal(string.Empty, person.PasswordHash);
            Assert.False(person.IsActive);
            Assert.Equal(string.Empty, person.CreatedBy);
            Assert.NotNull(person.Invoices);
        }

        [Fact]
        public void Person_SetProperties_ReturnsCorrectValues()
        {
            // Arrange
            var person = new Person();
            var testDate = DateTime.Now;

            // Act
            person.Id = 1;
            person.FirstName = "John";
            person.LastName = "Doe";
            person.Email = "john.doe@example.com";
            person.Phone = "123-456-7890";
            person.Address = "123 Main St";
            person.CityId = 10;
            person.Username = "johndoe";
            person.PasswordHash = "hashed_password";
            person.IsActive = true;
            person.CreatedDate = testDate;
            person.ModifiedDate = testDate;
            person.CreatedBy = "admin";
            person.ModifiedBy = "user1";

            // Assert
            Assert.Equal(1, person.Id);
            Assert.Equal("John", person.FirstName);
            Assert.Equal("Doe", person.LastName);
            Assert.Equal("john.doe@example.com", person.Email);
            Assert.Equal("123-456-7890", person.Phone);
            Assert.Equal("123 Main St", person.Address);
            Assert.Equal(10, person.CityId);
            Assert.Equal("johndoe", person.Username);
            Assert.Equal("hashed_password", person.PasswordHash);
            Assert.True(person.IsActive);
            Assert.Equal(testDate, person.CreatedDate);
            Assert.Equal(testDate, person.ModifiedDate);
            Assert.Equal("admin", person.CreatedBy);
            Assert.Equal("user1", person.ModifiedBy);
        }

        [Fact]
        public void Person_NavigationProperty_City_CanBeSet()
        {
            // Arrange
            var person = new Person();
            var city = new City { Id = 1, Name = "Test City" };

            // Act
            person.City = city;

            // Assert
            Assert.NotNull(person.City);
            Assert.Equal(1, person.City.Id);
            Assert.Equal("Test City", person.City.Name);
        }

        [Fact]
        public void Person_Invoices_InitializesAsEmptyCollection()
        {
            // Arrange & Act
            var person = new Person();

            // Assert
            Assert.NotNull(person.Invoices);
            Assert.Empty(person.Invoices);
        }

        [Fact]
        public void Person_Phone_CanBeNull()
        {
            // Arrange
            var person = new Person();

            // Act
            person.Phone = null;

            // Assert
            Assert.Null(person.Phone);
        }

        [Fact]
        public void Person_Address_CanBeNull()
        {
            // Arrange
            var person = new Person();

            // Act
            person.Address = null;

            // Assert
            Assert.Null(person.Address);
        }

        [Fact]
        public void Person_CityId_CanBeNull()
        {
            // Arrange
            var person = new Person();

            // Act
            person.CityId = null;

            // Assert
            Assert.Null(person.CityId);
        }

        [Fact]
        public void Person_ModifiedDate_CanBeNull()
        {
            // Arrange
            var person = new Person();

            // Act
            person.ModifiedDate = null;

            // Assert
            Assert.Null(person.ModifiedDate);
        }

        [Fact]
        public void Person_ModifiedBy_CanBeNull()
        {
            // Arrange
            var person = new Person();

            // Act
            person.ModifiedBy = null;

            // Assert
            Assert.Null(person.ModifiedBy);
        }
    }
}

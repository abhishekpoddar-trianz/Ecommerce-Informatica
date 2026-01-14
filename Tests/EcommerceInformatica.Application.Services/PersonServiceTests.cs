using Xunit;
using Moq;
using EcommerceInformatica.Application.Services;
using EcommerceInformatica.Domain.Entities;
using EcommerceInformatica.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EcommerceInformatica.Application.Services.Tests
{
    public class PersonServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IPersonRepository> _mockPersonRepository;
        private readonly Mock<ILogger<PersonService>> _mockLogger;
        private readonly PersonService _personService;

        public PersonServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockPersonRepository = new Mock<IPersonRepository>();
            _mockLogger = new Mock<ILogger<PersonService>>();

            _mockUnitOfWork.Setup(u => u.People).Returns(_mockPersonRepository.Object);
            _personService = new PersonService(_mockUnitOfWork.Object, _mockLogger.Object);
        }

        [Fact]
        public void Constructor_WithNullUnitOfWork_ThrowsArgumentNullException()
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentNullException>(() => new PersonService(null, _mockLogger.Object));
        }

        [Fact]
        public void Constructor_WithNullLogger_ThrowsArgumentNullException()
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentNullException>(() => new PersonService(_mockUnitOfWork.Object, null));
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsPerson_WhenPersonExists()
        {
            // Arrange
            var person = new Person { Id = 1, FirstName = "John", LastName = "Doe" };
            _mockPersonRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(person);

            // Act
            var result = await _personService.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("John", result.FirstName);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenPersonDoesNotExist()
        {
            // Arrange
            _mockPersonRepository.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Person)null);

            // Act
            var result = await _personService.GetByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllPeople()
        {
            // Arrange
            var people = new List<Person>
            {
                new Person { Id = 1, FirstName = "John" },
                new Person { Id = 2, FirstName = "Jane" }
            };
            _mockPersonRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(people);

            // Act
            var result = await _personService.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetByUsernameAsync_ReturnsPerson_WhenUsernameExists()
        {
            // Arrange
            var person = new Person { Id = 1, Username = "johndoe" };
            _mockPersonRepository.Setup(r => r.GetByUsernameAsync("johndoe", It.IsAny<CancellationToken>()))
                .ReturnsAsync(person);

            // Act
            var result = await _personService.GetByUsernameAsync("johndoe");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("johndoe", result.Username);
        }

        [Fact]
        public async Task GetByEmailAsync_ReturnsPerson_WhenEmailExists()
        {
            // Arrange
            var person = new Person { Id = 1, Email = "john@example.com" };
            _mockPersonRepository.Setup(r => r.GetByEmailAsync("john@example.com", It.IsAny<CancellationToken>()))
                .ReturnsAsync(person);

            // Act
            var result = await _personService.GetByEmailAsync("john@example.com");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("john@example.com", result.Email);
        }

        [Fact]
        public async Task GetByCityIdAsync_ReturnsPeopleInCity()
        {
            // Arrange
            var people = new List<Person>
            {
                new Person { Id = 1, FirstName = "John", CityId = 5 },
                new Person { Id = 2, FirstName = "Jane", CityId = 5 }
            };
            _mockPersonRepository.Setup(r => r.GetByCityIdAsync(5, It.IsAny<CancellationToken>()))
                .ReturnsAsync(people);

            // Act
            var result = await _personService.GetByCityIdAsync(5);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.All(result, p => Assert.Equal(5, p.CityId));
        }

        [Fact]
        public async Task GetActiveCustomersAsync_ReturnsOnlyActiveCustomers()
        {
            // Arrange
            var people = new List<Person>
            {
                new Person { Id = 1, FirstName = "John", IsActive = true }
            };
            _mockPersonRepository.Setup(r => r.GetActiveCustomersAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(people);

            // Act
            var result = await _personService.GetActiveCustomersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.All(result, p => Assert.True(p.IsActive));
        }

        [Fact]
        public async Task CreateAsync_CreatesPerson_WhenUsernameAndEmailAreUnique()
        {
            // Arrange
            var person = new Person { Username = "newuser", Email = "new@example.com" };
            _mockPersonRepository.Setup(r => r.UsernameExistsAsync("newuser", It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);
            _mockPersonRepository.Setup(r => r.EmailExistsAsync("new@example.com", It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);
            _mockPersonRepository.Setup(r => r.AddAsync(It.IsAny<Person>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(person);

            // Act
            var result = await _personService.CreateAsync(person);

            // Assert
            Assert.NotNull(result);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ThrowsException_WhenUsernameExists()
        {
            // Arrange
            var person = new Person { Username = "existinguser", Email = "new@example.com" };
            _mockPersonRepository.Setup(r => r.UsernameExistsAsync("existinguser", It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _personService.CreateAsync(person));
        }

        [Fact]
        public async Task CreateAsync_ThrowsException_WhenEmailExists()
        {
            // Arrange
            var person = new Person { Username = "newuser", Email = "existing@example.com" };
            _mockPersonRepository.Setup(r => r.UsernameExistsAsync("newuser", It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);
            _mockPersonRepository.Setup(r => r.EmailExistsAsync("existing@example.com", It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _personService.CreateAsync(person));
        }

        [Fact]
        public async Task UpdateAsync_UpdatesPerson_WhenPersonExists()
        {
            // Arrange
            var person = new Person { Id = 1, FirstName = "Updated" };
            _mockPersonRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(person);

            // Act
            await _personService.UpdateAsync(person);

            // Assert
            _mockPersonRepository.Verify(r => r.UpdateAsync(It.IsAny<Person>(), It.IsAny<CancellationToken>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ThrowsException_WhenPersonDoesNotExist()
        {
            // Arrange
            var person = new Person { Id = 999, FirstName = "Non-existent" };
            _mockPersonRepository.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Person)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _personService.UpdateAsync(person));
        }

        [Fact]
        public async Task DeleteAsync_DeletesPerson_WhenPersonExists()
        {
            // Arrange
            var person = new Person { Id = 1, FirstName = "John" };
            _mockPersonRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(person);

            // Act
            await _personService.DeleteAsync(1);

            // Assert
            _mockPersonRepository.Verify(r => r.DeleteAsync(It.IsAny<Person>(), It.IsAny<CancellationToken>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ThrowsException_WhenPersonDoesNotExist()
        {
            // Arrange
            _mockPersonRepository.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Person)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _personService.DeleteAsync(999));
        }

        [Fact]
        public async Task UsernameExistsAsync_ReturnsTrue_WhenUsernameExists()
        {
            // Arrange
            _mockPersonRepository.Setup(r => r.UsernameExistsAsync("johndoe", It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _personService.UsernameExistsAsync("johndoe");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task EmailExistsAsync_ReturnsTrue_WhenEmailExists()
        {
            // Arrange
            _mockPersonRepository.Setup(r => r.EmailExistsAsync("john@example.com", It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _personService.EmailExistsAsync("john@example.com");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task ValidateCredentialsAsync_ReturnsTrue_WhenCredentialsAreValid()
        {
            // Arrange
            var person = new Person { Username = "johndoe", PasswordHash = "password123", IsActive = true };
            _mockPersonRepository.Setup(r => r.GetByUsernameAsync("johndoe", It.IsAny<CancellationToken>()))
                .ReturnsAsync(person);

            // Act
            var result = await _personService.ValidateCredentialsAsync("johndoe", "password123");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task ValidateCredentialsAsync_ReturnsFalse_WhenCredentialsAreInvalid()
        {
            // Arrange
            var person = new Person { Username = "johndoe", PasswordHash = "password123", IsActive = true };
            _mockPersonRepository.Setup(r => r.GetByUsernameAsync("johndoe", It.IsAny<CancellationToken>()))
                .ReturnsAsync(person);

            // Act
            var result = await _personService.ValidateCredentialsAsync("johndoe", "wrongpassword");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ValidateCredentialsAsync_ReturnsFalse_WhenUserIsInactive()
        {
            // Arrange
            var person = new Person { Username = "johndoe", PasswordHash = "password123", IsActive = false };
            _mockPersonRepository.Setup(r => r.GetByUsernameAsync("johndoe", It.IsAny<CancellationToken>()))
                .ReturnsAsync(person);

            // Act
            var result = await _personService.ValidateCredentialsAsync("johndoe", "password123");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ValidateCredentialsAsync_ReturnsFalse_WhenUserDoesNotExist()
        {
            // Arrange
            _mockPersonRepository.Setup(r => r.GetByUsernameAsync("nonexistent", It.IsAny<CancellationToken>()))
                .ReturnsAsync((Person)null);

            // Act
            var result = await _personService.ValidateCredentialsAsync("nonexistent", "password");

            // Assert
            Assert.False(result);
        }
    }
}

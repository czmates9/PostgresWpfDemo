using PostgresWpfDemo.Validation;
using Xunit;

namespace PostgresWpfDemo.Tests
{
    public class UserValidatorTests
    {
        [Fact]
        public void IsValidName_ValidName_ReturnsTrue()
        {
            // Arrange
            var validator = new UserValidator();

            // Act
            bool result = validator.IsValidName("Matous");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValidName_EmptyName_ReturnsFalse()
        {
            // Arrange
            var validator = new UserValidator();

            // Act
            bool result = validator.IsValidName("");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValidName_WhitespaceOnly_ReturnsFalse()
        {
            // Arrange
            var validator = new UserValidator();

            // Act
            bool result = validator.IsValidName("   ");

            // Assert
            Assert.False(result);
        }
    }
}
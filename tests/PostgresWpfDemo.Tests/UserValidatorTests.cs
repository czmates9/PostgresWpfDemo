using PostgresWpfDemo.Validation;

namespace PostgresWpfDemo.Tests;

public sealed class UserValidatorTests
{
    [Theory]
    [InlineData("Matous", UserNameValidationResult.Valid)]
    [InlineData("A", UserNameValidationResult.NameTooShort)]
    [InlineData("", UserNameValidationResult.NameRequired)]
    [InlineData("   ", UserNameValidationResult.NameRequired)]
    [InlineData(null, UserNameValidationResult.NameRequired)]
    public void ValidateName_ReturnsExpectedResult(
        string? name,
        UserNameValidationResult expectedResult)
    {
        var validator = new UserValidator();

        var actualResult = validator.ValidateName(name);

        Assert.Equal(expectedResult, actualResult);
    }
}
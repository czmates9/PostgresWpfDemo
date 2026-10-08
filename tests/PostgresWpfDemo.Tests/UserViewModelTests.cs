using PostgresWpfDemo.Tests.Fakes;
using PostgresWpfDemo.Validation;
using PostgresWpfDemo.ViewModels;

namespace PostgresWpfDemo.Tests;

public sealed class UserViewModelTests
{
    [Fact]
    public async Task AddCommand_ValidName_AddsUser()
    {
        // Arrange
        var userService = new FakeUserService();
        var validator = new UserValidator();

        var viewModel = new UserViewModel(
            userService,
            validator)
        {
            NewUserName = "Matous"
        };

        // Act
        await viewModel.AddCommand.ExecuteAsync();

        // Assert
        Assert.Single(userService.Users);
        Assert.Equal("Matous", userService.Users[0].Name);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("A")]
    public async Task AddCommand_InvalidName_DoesNotAddUser(string name)
    {
        // Arrange
        var userService = new FakeUserService();
        var validator = new UserValidator();

        var viewModel = new UserViewModel(
            userService,
            validator)
        {
            NewUserName = name
        };

        // Act
        await viewModel.AddCommand.ExecuteAsync();

        // Assert
        Assert.Empty(userService.Users);
    }
}
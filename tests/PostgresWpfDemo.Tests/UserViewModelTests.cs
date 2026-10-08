using PostgresWpfDemo.Models;
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




    [Fact]
    public void UpdateAndDeleteCommands_NoSelectedUser_CannotExecute()
    {
        // Arrange
        var userService = new FakeUserService();
        var validator = new UserValidator();

        var viewModel = new UserViewModel(
            userService,
            validator);

        // Act
        var canUpdate = viewModel.UpdateCommand.CanExecute(null);
        var canDelete = viewModel.DeleteCommand.CanExecute(null);

        // Assert
        Assert.False(canUpdate);
        Assert.False(canDelete);
    }

    [Fact]
    public void UpdateAndDeleteCommands_SelectedUser_CanExecute()
    {
        // Arrange
        var userService = new FakeUserService();
        var validator = new UserValidator();

        var viewModel = new UserViewModel(
            userService,
            validator);

        // Act
        viewModel.SelectedUser = new User
        {
            Id = 1,
            Name = "Matous",
            CreatedAt = DateTime.UtcNow
        };

        // Assert
        Assert.True(viewModel.UpdateCommand.CanExecute(null));
        Assert.True(viewModel.DeleteCommand.CanExecute(null));
    }
}


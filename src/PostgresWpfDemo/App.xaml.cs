using PostgresWpfDemo.Services;
using PostgresWpfDemo.Validation;
using PostgresWpfDemo.ViewModels;
using System.Windows;

namespace PostgresWpfDemo;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        IUserService userService = new UserService();
        var userValidator = new UserValidator();

        var userViewModel = new UserViewModel(
            userService,
            userValidator);

        var mainWindow = new MainWindow(userViewModel);

        mainWindow.Show();
    }
}
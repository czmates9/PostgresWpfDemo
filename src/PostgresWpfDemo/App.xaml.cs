using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PostgresWpfDemo.Data;
using PostgresWpfDemo.Services;
using PostgresWpfDemo.Validation;
using PostgresWpfDemo.ViewModels;

namespace PostgresWpfDemo;

public partial class App : Application
{
    private readonly IHost _host;

    public App()
    {
        _host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                var connectionString =
                    context.Configuration.GetConnectionString("Postgres")
                    ?? throw new InvalidOperationException(
                        "Connection string 'Postgres' was not found.");

                services.AddDbContextFactory<AppDbContext>(
                    options =>
                        options.UseNpgsql(connectionString));

                services.AddSingleton<IUserService, UserService>();
                services.AddSingleton<UserValidator>();

                services.AddTransient<UserViewModel>();
                services.AddTransient<MainWindow>();
            })
            .Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        await _host.StartAsync();

        var mainWindow = _host.Services.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await _host.StopAsync();
        _host.Dispose();

        base.OnExit(e);
    }
}
using PostgresWpfDemo.ViewModels;
using System.Windows;

namespace PostgresWpfDemo;

public partial class MainWindow : Window
{
    private readonly UserViewModel _viewModel;

    public MainWindow(UserViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;
        DataContext = viewModel;

        Loaded += MainWindow_Loaded;
    }

    private async void MainWindow_Loaded(
        object sender,
        RoutedEventArgs e)
    {
        await _viewModel.LoadUsersAsync();
    }
}
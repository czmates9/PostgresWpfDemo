using PostgresWpfDemo.Data;
using PostgresWpfDemo.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Windows;

namespace PostgresWpfDemo
{
    public partial class MainWindow : Window
    {
        private readonly UserViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();

            _viewModel = new UserViewModel();

            DataContext = _viewModel;

            Loaded += MainWindow_Loaded;

            //TestEf();
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                await _viewModel.LoadUsersAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void TestEf()
        {
            using var db = new AppDbContext();

            var users = await db.Users.ToListAsync();

            MessageBox.Show($"EF načetl {users.Count} uživatelů.");
        }
    }
}
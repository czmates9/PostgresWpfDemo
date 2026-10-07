using PostgresWpfDemo.Commands;
using PostgresWpfDemo.Models;
using PostgresWpfDemo.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace PostgresWpfDemo.ViewModels
{
    public class UserViewModel : INotifyPropertyChanged
    {
        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }

        private readonly UserService _userService;

        private User? _selectedUser;
        private string _newUserName = string.Empty;

        public ObservableCollection<User> Users { get; }

        public User? SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged();

                if (_selectedUser != null)
                {
                    NewUserName = _selectedUser.Name;
                }
            }
        }

        public string NewUserName
        {
            get => _newUserName;
            set
            {
                _newUserName = value;
                OnPropertyChanged();
            }
        }

        public UserViewModel()
        {
            _userService = new UserService();

            Users = new ObservableCollection<User>();

            AddCommand = new AsyncRelayCommand(AddUserAsync);
            UpdateCommand = new AsyncRelayCommand(UpdateUserAsync);
            DeleteCommand = new AsyncRelayCommand(DeleteUserAsync);
        }

        private async Task AddUserAsync()
        {
            string name = NewUserName.Trim();

            if (string.IsNullOrWhiteSpace(name))
                return;

            var user = new User
            {
                Name = name,
                CreatedAt = DateTime.UtcNow
            };

            await _userService.AddAsync(user);

            NewUserName = string.Empty;

            await LoadUsersAsync();
        }

        private async Task UpdateUserAsync()
        {
            if (SelectedUser == null)
                return;

            string name = NewUserName.Trim();

            if (string.IsNullOrWhiteSpace(name))
                return;

            SelectedUser.Name = name;

            await _userService.UpdateAsync(SelectedUser);

            NewUserName = string.Empty;
            SelectedUser = null;

            await LoadUsersAsync();
        }

        private async Task DeleteUserAsync()
        {
            if (SelectedUser == null)
                return;

            await _userService.DeleteAsync(SelectedUser);

            NewUserName = string.Empty;
            SelectedUser = null;

            await LoadUsersAsync();
        }

        public async Task LoadUsersAsync()
        {
            var users = await _userService.GetAllAsync();

            Users.Clear();

            foreach (var user in users)
            {
                Users.Add(user);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(
            [CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(propertyName));
        }
    }
}
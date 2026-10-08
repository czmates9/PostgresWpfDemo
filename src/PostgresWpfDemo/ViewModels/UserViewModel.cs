using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using PostgresWpfDemo.Commands;
using PostgresWpfDemo.Models;
using PostgresWpfDemo.Services;
using PostgresWpfDemo.Validation;


namespace PostgresWpfDemo.ViewModels
{
    public class UserViewModel : INotifyPropertyChanged
    {
        public AsyncRelayCommand AddCommand { get; }
        public AsyncRelayCommand UpdateCommand { get; }
        public AsyncRelayCommand DeleteCommand { get; }

        private readonly IUserService _userService;
        private readonly UserValidator _userValidator;

        private User? _selectedUser;
        private string _newUserName = string.Empty;

        public ObservableCollection<User> Users { get; }

        public User? SelectedUser
        {
            get => _selectedUser;

            set
            {
                if (_selectedUser == value)
                    return;

                _selectedUser = value;
                OnPropertyChanged();

                if (_selectedUser is not null)
                    NewUserName = _selectedUser.Name;

                UpdateCommand.RaiseCanExecuteChanged();
                DeleteCommand.RaiseCanExecuteChanged();
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

        public UserViewModel(
            IUserService userService,
            UserValidator userValidator)
        {
            _userService = userService;
            _userValidator = userValidator;

            Users = [];

            AddCommand = new AsyncRelayCommand(AddUserAsync);

            UpdateCommand = new AsyncRelayCommand(
                UpdateUserAsync,
                () => SelectedUser is not null);

            DeleteCommand = new AsyncRelayCommand(
                DeleteUserAsync,
                () => SelectedUser is not null);
        }

        private async Task AddUserAsync()
        {
            var validationResult = _userValidator.ValidateName(NewUserName);

            if (validationResult != UserNameValidationResult.Valid)
                return;

            var user = new User
            {
                Name = NewUserName.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            await _userService.AddAsync(user);

            NewUserName = string.Empty;

            await LoadUsersAsync();
        }

        private async Task UpdateUserAsync()
        {
            if (SelectedUser is null)
                return;

            var validationResult = _userValidator.ValidateName(NewUserName);

            if (validationResult != UserNameValidationResult.Valid)
                return;

            SelectedUser.Name = NewUserName.Trim();

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
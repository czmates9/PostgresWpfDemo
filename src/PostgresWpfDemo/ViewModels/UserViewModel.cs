using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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
        private bool _isBusy;
        private string? _errorMessage;

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
                {
                    NewUserName = _selectedUser.Name;
                }

                UpdateCommand.RaiseCanExecuteChanged();
                DeleteCommand.RaiseCanExecuteChanged();
            }
        }

        public string NewUserName
        {
            get => _newUserName;

            set
            {
                if (_newUserName == value)
                    return;

                _newUserName = value;
                OnPropertyChanged();
            }
        }

        public bool IsBusy
        {
            get => _isBusy;

            set
            {
                if (_isBusy == value)
                    return;

                _isBusy = value;
                OnPropertyChanged();

                AddCommand.RaiseCanExecuteChanged();
                UpdateCommand.RaiseCanExecuteChanged();
                DeleteCommand.RaiseCanExecuteChanged();
            }
        }

        public string? ErrorMessage
        {
            get => _errorMessage;

            set
            {
                if (_errorMessage == value)
                    return;

                _errorMessage = value;
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

            AddCommand = new AsyncRelayCommand(
     AddUserAsync,
     () => !IsBusy);

            UpdateCommand = new AsyncRelayCommand(
                UpdateUserAsync,
                () => !IsBusy && SelectedUser is not null);

            DeleteCommand = new AsyncRelayCommand(
                DeleteUserAsync,
                () => !IsBusy && SelectedUser is not null);
        }

        private async Task AddUserAsync()
        {
            var validationResult =
                _userValidator.ValidateName(NewUserName);

            if (validationResult != UserNameValidationResult.Valid)
                return;

            try
            {
                IsBusy = true;
                ErrorMessage = null;

                var user = new User
                {
                    Name = NewUserName.Trim(),
                    CreatedAt = DateTime.UtcNow
                };

                await _userService.AddAsync(user);

                NewUserName = string.Empty;

                await LoadUsersAsync();
            }
            catch
            {
                ErrorMessage =
                    "Nepodařilo se přidat uživatele.";
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task UpdateUserAsync()
        {
            if (SelectedUser is null)
                return;

            var validationResult =
                _userValidator.ValidateName(NewUserName);

            if (validationResult != UserNameValidationResult.Valid)
                return;

            try
            {
                IsBusy = true;
                ErrorMessage = null;

                SelectedUser.Name =
                    NewUserName.Trim();

                await _userService.UpdateAsync(
                    SelectedUser);

                NewUserName = string.Empty;
                SelectedUser = null;

                await LoadUsersAsync();
            }
            catch
            {
                ErrorMessage =
                    "Nepodařilo se upravit uživatele.";
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task DeleteUserAsync()
        {
            if (SelectedUser is null)
                return;

            try
            {
                IsBusy = true;
                ErrorMessage = null;

                await _userService.DeleteAsync(
                    SelectedUser);

                NewUserName = string.Empty;
                SelectedUser = null;

                await LoadUsersAsync();
            }
            catch
            {
                ErrorMessage =
                    "Nepodařilo se smazat uživatele.";
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task LoadUsersAsync()
        {
            try
            {
                IsBusy = true;
                ErrorMessage = null;

                var users =
                    await _userService.GetAllAsync();

                Users.Clear();

                foreach (var user in users)
                {
                    Users.Add(user);
                }
            }
            catch
            {
                ErrorMessage =
                    "Nepodařilo se načíst uživatele.";
            }
            finally
            {
                IsBusy = false;
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
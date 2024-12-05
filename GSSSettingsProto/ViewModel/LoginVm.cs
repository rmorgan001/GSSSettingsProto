using System.Windows;
using System.Windows.Input;
using GSSSettingsProto.Commands;
using GSSSettingsProto.Models;

namespace GSSSettingsProto.ViewModel
{
    public class LoginVm : ViewModelBase
    {
        private readonly User _user = new();
        
        public ICommand LoginCommand {get; } = new RelayCommand(LoggedIn);

        public string? UserName
        {
            get => _user.UserName;
            set
            {
                _user.UserName = value;
                OnPropertyChanged(nameof(UserName));
            }
        }

        public string? Password
        {
            get => _user.Password;
            set
            {
                _user.Password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        private static void LoggedIn(object parameter)
        {
            var s = Settings.Profile.SettingsList;

            MessageBox.Show($"Logged in successful as {parameter}");
        }
    }
}

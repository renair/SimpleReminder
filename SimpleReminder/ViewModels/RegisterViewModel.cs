using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using DataStorage.Contexts;
using DataStorage.Models;
using SimpleReminder.Managers;
using Tools;

namespace SimpleReminder.ViewModels
{
    class RegisterViewModel : INotifyPropertyChanged
    {
        private ICommand _registerCommand;
        private ICommand _backToLogInCommand;

        private string _surname;
        private string _name;
        private string _login;
        private string _password;
        private string _email;

        public string Surname
        {
            get { return _surname; }
            set
            {
                _surname = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value; 
                OnPropertyChanged();
            }
        }

        public string Login
        {
            get { return _login; }
            set
            {
                _login = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        public ICommand RegisterCommand
        {
            get
            {
                return _registerCommand ??  (_registerCommand = new RelayCommand<object>(Register, CanRegister));
            }
        }

        public ICommand BackToLogInCommand
        {
            get
            {
                return _backToLogInCommand ?? (_backToLogInCommand = new RelayCommand<object>(BackToLogIn));
            }
        }

        private void BackToLogIn(object obj)
        {
            NavigationManager.Navigate(Managers.Screens.SignIn);
        }

        private async void Register(object obj)
        {
            UserData registrationData = new UserData
            {
                Login = Login,
                Name = Name,
                Surname = Surname,
                Email = Email
            };
            try
            {
                LoaderManager.ShowLoader();
                UserData registeredUser = await UserAccess.SignUp(registrationData, Password);
                AccountManager.CurrentUser = registeredUser;
                NavigationManager.Navigate(Managers.Screens.Main);
            }
            catch (Exception e)
            {
                Logger.Log("Can't register new user", e);
            }
            finally
            {
                LoaderManager.HideLoader();
            }
        }

        private bool CanRegister(object obj)
        {
            return Login != "" && Name != "" && Surname != "" && Email != "" && Password != "";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

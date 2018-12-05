using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SimpleReminder.Managers;
using Tools;

namespace SimpleReminder.ViewModels
{
    class LoginViewModel : INotifyPropertyChanged
    {
        #region Fields

        private string _login;

        public string Login
        {
            get { return _login; }
            set
            {
                _login = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        private ICommand _loginCommand;
        private ICommand _signUpCommand;

        public ICommand LoginCommand
        {
            get
            {
                return _loginCommand ??
                       (_loginCommand = new RelayCommand<object>(LoginCommandImpl, CanLoginCommandImpl));
            }
        }

        public ICommand SignUpCommand
        {
            get
            {
                return _signUpCommand ??
                       (_signUpCommand = new RelayCommand<object>(SignUpCommandImpl));
            }
        }

        #endregion

        #region Methods

        public LoginViewModel()
        {}

        #endregion

        #region Command implementation

        private async void LoginCommandImpl(object arg)
        {
            var passBox = arg as PasswordBox;
            if(passBox == null)
            {
                return;
            }
            LoaderManager.ShowLoader();
            var isLoggedIn = await AccountManager.SignIn(Login, passBox.Password);
            LoaderManager.HideLoader();

            // If user exists and password is correct
            if (isLoggedIn)
            {
                NavigationManager.Navigate(Managers.Screens.Main);
            }
            else
            {
                MessageBox.Show("Can't login to account.");
            }
        }

        private bool CanLoginCommandImpl(object arg)
        {
            var passBox = arg as PasswordBox;
            if (passBox == null)
            {
                return false;
            }
            return Login != "" && passBox.Password != "";
        }

        private void SignUpCommandImpl(object arg)
        {
            NavigationManager.Navigate(Managers.Screens.SignUp);
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

using System.Windows;
using SimpleReminder.DataAccess;
using SimpleReminder.Managers;
using SimpleReminder.Tools;

namespace SimpleReminder.Screens
{
    // Codebehind for LoginScreen it should just login user.
    public partial class LoginScreen : IScreen
    {
        public LoginScreen()
        {
            InitializeComponent();
        }

        private async void loginButton_Click(object sender, RoutedEventArgs e)
        {
            LoaderManager.ShowLoader();
            var result = await UserAccess.SignIn(loginBox.Text, passwordBox.Password);
            LoaderManager.HideLoader();

            // If user exists and password is correct
            if (result)
            {
                NavigationManager.Navigate(Managers.Screens.Main);
            }
            else
            {
                MessageBox.Show("Application works in test mode.\nPassword: andrew\nLogin: andrew");
            }
        }

        public void NavigatedTo()
        {
            loginBox.Text = "";
            passwordBox.Password = "";
        }

        public void NavigatedFrom(){}
    }
}

using System.Windows;
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
            var isLoggedIn = await AccountManager.SignIn(loginBox.Text, passwordBox.Password);
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

        private void registerButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationManager.Navigate(Managers.Screens.SignUp);
        }

        public void NavigatedTo()
        {
            loginBox.Text = "";
            passwordBox.Password = "";
        }

        public void NavigatedFrom(){}
    }
}

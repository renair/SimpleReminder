using System.Windows;
using SimpleReminder.DataAccess;

namespace SimpleReminder.Screens
{
    // Codebehind for LoginScreen it should just login user.
    public partial class LoginScreen
    {
        public delegate void Auth();

        // Event which will be invoked when user pass authorization
        public event Auth LoginDone;

        public LoginScreen()
        {
            InitializeComponent();
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            // Chak if user registered
            if(!UserAccess.IsRegistered(loginBox.Text, passwordBox.Password))
            {
                MessageBox.Show("Application works in test mode.\nPassword: andrew\nLogin: andrew");
                return;
            }
            // If user exists and password is correct
            LoginDone?.Invoke();
        }
    }
}

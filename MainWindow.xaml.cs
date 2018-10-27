using System.Windows;
using SimpleReminder.Screens;

namespace SimpleReminder
{
    // This class only change from LoginScreen to MainScreen
    // and backward in future on log out
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoginScreen loginScreen = new LoginScreen();
            loginScreen.LoginDone += UserLoggedIn;
            contentControl.Content = loginScreen;
        }

        private void UserLoggedIn()
        {
            contentControl.Content = new MainScreen();
        }
    }
}

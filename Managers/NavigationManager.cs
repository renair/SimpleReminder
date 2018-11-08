using System;
using System.Windows.Controls;
using SimpleReminder.Screens;

namespace SimpleReminder.Managers
{
    internal static class NavigationManager
    {
        private static UserControl _loginScreen;
        private static UserControl _registerScreen;
        private static UserControl _mainScreen;

        private static MainWindow _contentControl;

        public static void Initialize(MainWindow mainContentControl)
        {
            _contentControl = mainContentControl;
            _loginScreen = new LoginScreen();
            _registerScreen = new LoginScreen();
            _mainScreen = new MainScreen();
        }

        public static void Navigate(Screens navigateToScreen)
        {
            switch(navigateToScreen)
            {
                case Screens.SignIn:
                    _contentControl.ContentControl.Content = _loginScreen;
                    break;
                case Screens.SignUp:
                    _contentControl.ContentControl.Content = _registerScreen;
                    break;
                case Screens.Main:
                    _contentControl.ContentControl.Content = _mainScreen;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(navigateToScreen), navigateToScreen, null);
            }
        }
    }

    internal enum Screens
    {
        SignIn,
        SignUp,
        Main
    }
}

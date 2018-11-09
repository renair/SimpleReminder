using System;
using System.Windows.Controls;
using SimpleReminder.Screens;
using SimpleReminder.Tools;

namespace SimpleReminder.Managers
{
    internal static class NavigationManager
    {
        private static IScreen _loginScreen;
        private static IScreen _registerScreen;
        private static IScreen _mainScreen;

        private static MainWindow _contentControl;
        private static IScreen _currentScreen;

        public static void Initialize(MainWindow mainContentControl)
        {
            _contentControl = mainContentControl;
            _currentScreen = null;
            _loginScreen = new LoginScreen();
            _registerScreen = new RegisterScreen();
            _mainScreen = new MainScreen();
        }

        public static void Navigate(Screens navigateToScreen)
        {
            _currentScreen?.NavigatedFrom();
            switch (navigateToScreen)
            {
                case Screens.SignIn:
                    _loginScreen.NavigatedTo();
                    _currentScreen = _loginScreen;
                    break;
                case Screens.SignUp:
                    _registerScreen.NavigatedTo();
                     _currentScreen = _registerScreen;
                    break;
                case Screens.Main:
                    _mainScreen.NavigatedTo();
                    _currentScreen = _mainScreen;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(navigateToScreen), navigateToScreen, null);
            }

            _contentControl.ContentControl.Content = _currentScreen;
        }
    }

    internal enum Screens
    {
        SignIn,
        SignUp,
        Main
    }
}

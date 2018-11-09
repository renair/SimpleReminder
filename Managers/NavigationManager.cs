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
            _registerScreen = new LoginScreen();
            _mainScreen = new MainScreen();
        }

        public static void Navigate(Screens navigateToScreen)
        {
            _currentScreen?.NavigatedFrom();
            switch (navigateToScreen)
            {
                case Screens.SignIn:
                    _loginScreen.NavigatedTo();
                    _contentControl.ContentControl.Content = _loginScreen;
                    break;
                case Screens.SignUp:
                    _registerScreen.NavigatedTo();
                    _contentControl.ContentControl.Content = _registerScreen;
                    break;
                case Screens.Main:
                    _mainScreen.NavigatedTo();
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

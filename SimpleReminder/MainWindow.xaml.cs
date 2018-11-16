using SimpleReminder.Managers;
using SimpleReminder.ViewModels;
using Tools;

namespace SimpleReminder
{
    // This class only change from LoginScreen to MainScreen
    // and backward in future on log out
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            NavigationManager.Initialize(this);
            Managers.Screens direction = AccountManager.CurrentUser != null ? Managers.Screens.Main : Managers.Screens.SignIn;
            NavigationManager.Navigate(direction);

            LoaderViewModel vmodel = new LoaderViewModel();
            LoaderManager.Initialize(vmodel);
            DataContext = vmodel;

            Logger.Log("Application started.");
        }
    }
}

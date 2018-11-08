using SimpleReminder.Managers;
using SimpleReminder.ViewModels;

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
            NavigationManager.Navigate(Managers.Screens.SignIn);

            LoaderViewModel vmodel = new LoaderViewModel();
            LoaderManager.Initialize(vmodel);
            DataContext = vmodel;
        }
    }
}

using SimpleReminder.Tools;
using SimpleReminder.ViewModels;

namespace SimpleReminder.Screens
{
    /// <summary>
    /// Логика взаимодействия для MainContent.xaml
    /// </summary>
    public partial class MainScreen : IScreen
    {
        private readonly MainViewModel _mvm;

        public MainScreen()
        {
            InitializeComponent();
            _mvm = new MainViewModel(NotificationsContainer.Children);
            DataContext = _mvm;
        }

        public void NavigatedTo()
        {
            _mvm.FillNotifications();
        }

        public void NavigatedFrom()
        {}
    }
}

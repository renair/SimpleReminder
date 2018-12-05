using SimpleReminder.Tools;
using SimpleReminder.ViewModels;

namespace SimpleReminder.Screens
{
    // Codebehind for LoginScreen it should just login user.
    public partial class LoginScreen : IScreen
    {
        public LoginScreen()
        {
            InitializeComponent();
            DataContext = new LoginViewModel();
        }

        public void NavigatedTo(){}

        public void NavigatedFrom(){}
    }
}

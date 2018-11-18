using System.Windows;
using SimpleReminder.Tools;
using SimpleReminder.ViewModels;

namespace SimpleReminder.Screens
{
    /// <summary>
    /// Логика взаимодействия для RegisterScreen.xaml
    /// </summary>
    public partial class RegisterScreen : IScreen
    {
        public RegisterScreen()
        {
            InitializeComponent();
            DataContext = new RegisterViewModel();
        }

        // PasswordBox can not be used with Binding, so use this method
        // to change ViewModel directly.
        private void PasswordBoxChanged(object sender, RoutedEventArgs e)
        {
            if(DataContext is RegisterViewModel model)
            {
                model.Password = PasswordBox.Password;
            }
        }

        public void NavigatedTo()
        {
            LoginBox.Text = "";
            PasswordBox.Password = "";
            FirstNameBox.Text = "";
            LastNameBox.Text = "";
            EmailBox.Text = "";
        }

        public void NavigatedFrom()
        {}
    }
}

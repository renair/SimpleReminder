using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SimpleReminder.Data;
using SimpleReminder.Controlls;

namespace SimpleReminder.Screens
{
    /// <summary>
    /// Логика взаимодействия для MainContent.xaml
    /// </summary>
    public partial class MainScreen : UserControl
    {
        private SortedList<ReminderData, NotificationControll> remindings = new SortedList<ReminderData, NotificationControll>();

        public MainScreen()
        {
            InitializeComponent();
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            ReminderData data = new ReminderData();
            data.SelectedDate = DateTime.Now.AddHours(1);
            data.ReminderText = "This is test!";
            NotificationControll ctrl = new NotificationControll(data);
            ctrl.RequiringSettings += ChangeNotification;
            //remindings.Add(data, ctrl);
            //int i = remindings.IndexOfKey(data);
            //NotificationsContainer.Children.Insert(i, ctrl);
            NotificationsContainer.Children.Add(ctrl);
        }

        private void ChangeNotification(ReminderData d)
        {
            throw new NotImplementedException();
        }

        private void RedrawRemindings()
        {
            NotificationsContainer.Children.Clear();
            foreach(NotificationControll ctrl in remindings.Values)
            {
                NotificationsContainer.Children.Add(ctrl);
            }
        }

        private void ReorderButtonClick(object sender, RoutedEventArgs e)
        {
            RedrawRemindings();
        }
    }
}

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
        private List<NotificationControll> remindings = new List<NotificationControll>();

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
            ctrl.NotificationOutdated += (notif) => {
                NotificationsContainer.Children.Remove(ctrl);
                remindings.Remove(ctrl);
            };
            remindings.Add(ctrl);
            RedrawRemindings();
        }

        private void ChangeNotification(ReminderData d)
        {
            Canvas.SetZIndex(NotificationEditor, 1);
            NotificationScreen editor = new NotificationScreen(d);
            editor.ScreenClosing += OnNotificationEdited;
            NotificationEditorContentControll.Content = editor;
        }

        private void OnNotificationEdited()
        {
            Canvas.SetZIndex(NotificationEditor, -1);
            NotificationEditorContentControll.Content = null;
            RedrawRemindings();
        }

        private void RedrawRemindings()
        {
            remindings.Sort((obj, obj1) => { return obj.ReminderData.Compare(obj.ReminderData, obj1.ReminderData); });
            NotificationsContainer.Children.Clear();
            foreach (NotificationControll ctrl in remindings)
            {
                ctrl.DisplayDate();
                NotificationsContainer.Children.Add(ctrl);
            }
        }
    }
}

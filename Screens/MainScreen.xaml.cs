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
using System.Windows.Threading;

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
            //TODO do it in another function
            DispatcherTimer t = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 15) //every 15 seconds
            };
            t.Tick += TimerTicked;
            t.Start();
        }

        private void TimerTicked(object sender, EventArgs e)
        {
            RedrawRemindings();
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            ReminderData data = new ReminderData();
            data.SelectedDate = DateTime.Now.AddHours(1);
            data.ReminderText = "";
            NotificationControll ctrl = new NotificationControll(data);
            ctrl.RequiringSettings += ChangeNotification;
            ctrl.NotificationOutdated += (obj) => {
                RedrawRemindings();
            };
            // TODO do something with it!
            ctrl.ReadyToRemove += (notif) => {
                NotificationsContainer.Children.Remove(ctrl);
                lock(remindings)
                {
                    remindings.Remove(ctrl);
                }
            };
            lock(remindings)
            {
                remindings.Add(ctrl);
            }
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
            lock(remindings)
            {
                //TODO implement comparing
                remindings.Sort((obj, obj1) => { return obj.ReminderData.Compare(obj.ReminderData, obj1.ReminderData); });
                NotificationsContainer.Children.Clear();
                foreach (NotificationControll ctrl in remindings.ToArray())
                {
                    ctrl.DisplayDate();
                    NotificationsContainer.Children.Add(ctrl);
                }
            }
        }
    }
}

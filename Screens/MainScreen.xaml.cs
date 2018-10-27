using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Linq;
using SimpleReminder.Data;
using SimpleReminder.Controlls;

namespace SimpleReminder.Screens
{
    /// <summary>
    /// Логика взаимодействия для MainContent.xaml
    /// </summary>
    public partial class MainScreen : UserControl
    {
        private Dictionary<ReminderData, NotificationControll> remindings = new Dictionary<ReminderData, NotificationControll>();

        public MainScreen()
        {
            InitializeComponent();
            // Setup Timer to update controlls
            DispatcherTimer t = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 1) //every second
            };
            t.Tick += TimerTicked;
            t.Start();
        }

        private void TimerTicked(object sender, EventArgs e)
        {
            // lock remindings to grant tht we use it personaly
            lock(remindings)
            {
                // Iterate over controlls and update if TimeIsCome()
                // Consider all remindings already sorted.
                for(int i = 0; i < remindings.Count && remindings.Keys.ElementAt(i).isTimeCome(); ++i)
                {
                    var  k = remindings.Keys.ElementAt(i);
                    remindings[k].DisplayDate();
                }
            }
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            // Configure empty ReminderData
            ReminderData data = new ReminderData();
            data.SelectedDate = DateTime.Now.AddHours(1);
            data.ReminderText = "";
            // Create controll which contain this data
            NotificationControll ctrl = new NotificationControll(data);
            // Set some handlers for this controll
            ctrl.RequiringSettings += ChangeNotification;
            ctrl.NotificationOutdated += OnNotificationOutdated;
            // Remove notification controll when time come
            ctrl.ReadyToRemove += OnReadyToRemove;
            // Add data and controll to Dictionary for redrawing, etc.
            lock(remindings)
            {
                remindings.Add(data, ctrl);
            }
            // Redraw all existing Notifications included created one.
            RedrawRemindings();
        }

        private void OnNotificationOutdated(NotificationControll ctrl)
        {
            ReminderData d = ctrl.ReminderData;
            try
            {
                // This will update exactly one controll
                // instead of redrawing all.
                //remindings[d].DisplayDate();

                // Redraw remindings because we are able to outdate random notification
                // In release version notification will be already sorted and we can use code above.
                RedrawRemindings();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Some data were lost. Can't update reminding controll.\nMessage: " + ex.Message);
            }
        }

        private void OnReadyToRemove(NotificationControll ctrl)
        {
            try
            {
                lock (remindings)
                {
                    NotificationsContainer.Children.Remove(ctrl);
                    remindings.Remove(ctrl.ReminderData);
                }
            }
            catch(ArgumentNullException ex)
            {
                MessageBox.Show("Controll does not contain ReminderData. Message: " + ex.Message);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Unexpected exception on deleting notification controll. Message: " + ex.Message);
            }
        }

        private void ChangeNotification(ReminderData d)
        {
            // Move NotificationEditor in front of Screen
            Canvas.SetZIndex(NotificationEditor, 1);
            // Create edit screen with reminder data
            NotificationScreen editor = new NotificationScreen(d);
            // Setup reaction for events
            editor.SaveRequired += OnNotificationEdited;
            editor.RemoveRequired += OnNotificationRemoved;
            // Show edition screen
            NotificationEditorContentControll.Content = editor;
        }

        private void OnNotificationEdited(ReminderData data)
        {
            // Move NotificationEditor behind all notifications
            Canvas.SetZIndex(NotificationEditor, -1);
            // Remove edit screen
            NotificationEditorContentControll.Content = null;
            // Redraw all list because date can change and we need to
            // order all notifications.
            RedrawRemindings();
        }

        private void OnNotificationRemoved(ReminderData data)
        {
            // Move NotificationEditor behind all notifications
            Canvas.SetZIndex(NotificationEditor, -1);
            // Remove edit screen
            NotificationEditorContentControll.Content = null;
            try
            {
                lock(remindings)
                {
                    // Remove controll from all notifications controlls
                    NotificationsContainer.Children.Remove(remindings[data]);
                    // Remove data with controll because we dont't need it anymore 
                    remindings.Remove(data);
                }
                
            }
            catch(ArgumentNullException)
            {
                MessageBox.Show("Some data were lost. Can't remove reminding.");
            }
        }

        private void RedrawRemindings()
        {
            lock(remindings)
            {
                try
                {
                    // Order remindings based in its data
                    remindings = remindings.OrderBy(e => e.Key).ToDictionary(e => e.Key, e => e.Value);
                    // Clear all controlls from screen
                    NotificationsContainer.Children.Clear();
                    // Add already ordered controlls
                    foreach (NotificationControll ctrl in remindings.Values.ToArray())
                    {
                        // Update controll because it can be outdated or have changed data
                        ctrl.DisplayDate();
                        // This if will be romoved in release
                        if (!NotificationsContainer.Children.Contains(ctrl))
                        {
                            NotificationsContainer.Children.Add(ctrl);
                        }
                    }
                }
                    catch (ArgumentNullException)
                {
                    MessageBox.Show("Some data were lost. Can't order remindings.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unexpected exception on redrawing! \nMessage: " + ex.Message);
                }
            }
        }
    }
}

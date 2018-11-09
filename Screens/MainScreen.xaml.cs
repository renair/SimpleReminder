using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Linq;
using SimpleReminder.Data;
using SimpleReminder.Controlls;
using SimpleReminder.Managers;
using SimpleReminder.Tools;

namespace SimpleReminder.Screens
{
    /// <summary>
    /// Логика взаимодействия для MainContent.xaml
    /// </summary>
    public partial class MainScreen : IScreen
    {
        private Dictionary<ReminderData, NotificationControl> _remindings = new Dictionary<ReminderData, NotificationControl>();
        private DispatcherTimer _timer;

        public MainScreen()
        {
            InitializeComponent();
        }

        private void TimerTicked(object sender, EventArgs e)
        {
            // lock remindings to grant tht we use it personally
            lock(_remindings)
            {
                // Iterate over controls and update if TimeIsCome()
                // Consider all remindings already sorted.
                for(int i = 0; i < _remindings.Count && _remindings.Keys.ElementAt(i).IsTimeCome(); ++i)
                {
                    var  k = _remindings.Keys.ElementAt(i);
                    _remindings[k].DisplayDate();
                }
            }
        }

        public void NavigatedTo()
        {
            // Setup Timer to update controls
            _timer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 1) //every second
            };
            _timer.Tick += TimerTicked;
            _timer.Start();
            try
            {
                foreach (ReminderData data in AccountManager.CurrentUser.Notifications)
                {
                    NotificationControl ctrl = new NotificationControl(data);
                    // Set some handlers for this control
                    ctrl.RequiringSettings += ChangeNotification;
                    ctrl.NotificationOutdated += OnNotificationOutdated;
                    // Remove notification control when time come
                    ctrl.ReadyToRemove += OnReadyToRemove;
                    // Add data and control to Dictionary for redrawing, etc.
                    lock (_remindings)
                    {
                        _remindings.Add(data, ctrl);
                    }
                }
                RedrawRemindings();
                Logger.Log("User's notifications loaded.");
            }
            catch (NullReferenceException e)
            {
                Logger.Log("User open man window without authorization", e);
                MessageBox.Show("Can't load current user!");
                NavigationManager.Navigate(Managers.Screens.SignIn);
            }
            catch (Exception e)
            {
                Logger.Log("Unknown exception.", e);
                MessageBox.Show("Unknown error!");
            }
        }

        public void NavigatedFrom()
        {
            _timer.Stop();
            lock (_remindings)
            {
                _remindings.Clear();
            }
            RedrawRemindings();
        }

        private async void AddButtonClick(object sender, RoutedEventArgs e)
        {
            // Configure empty ReminderData
            ReminderData data = new ReminderData();
            data.SelectedDate = DateTime.Now.AddHours(1);
            data.ReminderText = "";
            // Add ReminderData to other user's notifications
            LoaderManager.ShowLoader();
            await AccountManager.AddReminding(data);
            LoaderManager.HideLoader();;
            // Create controll which contain this data
            NotificationControl ctrl = new NotificationControl(data);
            // Set some handlers for this controll
            ctrl.RequiringSettings += ChangeNotification;
            ctrl.NotificationOutdated += OnNotificationOutdated;
            // Remove notification controll when time come
            ctrl.ReadyToRemove += OnReadyToRemove;
            // Add data and controll to Dictionary for redrawing, etc.
            lock(_remindings)
            {
                _remindings.Add(data, ctrl);
            }
            Logger.Log("New reminding added.");
            // Redraw all existing Notifications included created one.
            RedrawRemindings();
        }

        private void OnNotificationOutdated(NotificationControl ctrl)
        {
            try
            {
                // This will update exactly one controll
                // instead of redrawing all.
                //remindings[d].DisplayDate();

                // Redraw remindings because we are able to outdate random notification
                // In release version notification will be already sorted and we can use code above.
                RedrawRemindings();
                Logger.Log("Some notifications outdated and marked with red color.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Some data were lost. Can't update reminding control.\nMessage: " + ex.Message);
                Logger.Log("Some data were lost. Can't update reminding control" ,ex);
            }
        }

        private async void OnReadyToRemove(NotificationControl ctrl)
        {
            try
            {
                lock (_remindings)
                {
                    NotificationsContainer.Children.Remove(ctrl);
                    _remindings.Remove(ctrl.ReminderData);
                }

                LoaderManager.ShowLoader();
                await AccountManager.DeleteReminding(ctrl.ReminderData);
                LoaderManager.HideLoader(); ;

                Logger.Log("Notification removed.");
            }
            catch(ArgumentNullException ex)
            {
                MessageBox.Show("Control does not contain ReminderData. Message: " + ex.Message);
                Logger.Log("Control does not contain ReminderData", ex);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Unexpected exception on deleting notification control. Message: " + ex.Message);
                Logger.Log("Unexpected exception on deleting notification control", ex);
            }
        }

        private void ChangeNotification(ReminderData d)
        {
            // Move NotificationEditor in front of Screen
            Panel.SetZIndex(NotificationEditor, 1);
            // Create edit screen with reminder data
            NotificationScreen editor = new NotificationScreen(d);
            // Setup reaction for events
            editor.SaveRequired += OnNotificationEdited;
            editor.RemoveRequired += OnNotificationRemoved;
            // Show edition screen
            NotificationEditorContentControll.Content = editor;
        }

        private async void OnNotificationEdited(ReminderData data)
        {
            // Move NotificationEditor behind all notifications
            Panel.SetZIndex(NotificationEditor, -1);
            // Remove edit screen
            NotificationEditorContentControll.Content = null;

            LoaderManager.ShowLoader();
            await AccountManager.UpdateReminding(data);
            LoaderManager.HideLoader();

            // Redraw all list because date can change and we need to
            // order all notifications.
            RedrawRemindings();
        }

        private async void OnNotificationRemoved(ReminderData data)
        {
            // Move NotificationEditor behind all notifications
            Panel.SetZIndex(NotificationEditor, -1);
            // Remove edit screen
            NotificationEditorContentControll.Content = null;
            try
            {
                lock(_remindings)
                {
                    // Remove controll from all notifications controlls
                    NotificationsContainer.Children.Remove(_remindings[data]);
                    // Remove data with controll because we dont't need it anymore 
                    _remindings.Remove(data);
                }

                LoaderManager.ShowLoader();
                await AccountManager.DeleteReminding(data);
                LoaderManager.HideLoader(); ;
            }
            catch(ArgumentNullException)
            {
                MessageBox.Show("Some data were lost. Can't remove reminding.");
                Logger.Log("Some data were lost. Can't remove reminding.");
            }
        }

        private void RedrawRemindings()
        {
            lock(_remindings)
            {
                try
                {
                    // Order remindings based in its data
                    _remindings = _remindings.OrderBy(e => e.Key).ToDictionary(e => e.Key, e => e.Value);
                    // Clear all controlls from screen
                    NotificationsContainer.Children.Clear();
                    // Add already ordered controlls
                    foreach (NotificationControl ctrl in _remindings.Values.ToArray())
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
                catch (ArgumentNullException ex)
                {
                    MessageBox.Show("Some data were lost. Can't order remindings.");
                    Logger.Log("Some data were lost. Can't order remindings.", ex);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unexpected exception on redrawing! \nMessage: " + ex.Message);
                    Logger.Log("Unexpected exception on redrawing!", ex);
                }
            }
        }
    }
}

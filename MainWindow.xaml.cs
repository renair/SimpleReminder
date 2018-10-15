﻿using System;
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

namespace SimpleReminder
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SortedList<ReminderData, NotificationControll> remindings = new SortedList<ReminderData, NotificationControll>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            ReminderData data = new ReminderData();
            data.SelectedDate = DateTime.Now.AddHours(1);
            data.ReminderText = "This is test!";
            NotificationControll ctrl = new NotificationControll(data);
            //remindings.Add(data, ctrl);
            //int i = remindings.IndexOfKey(data);
            //NotificationsContainer.Children.Insert(i, ctrl);
            NotificationsContainer.Children.Add(ctrl);
        }
    }
}

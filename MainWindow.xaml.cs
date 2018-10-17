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
using SimpleReminder.Screens;

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
            contentControl.Content = new MainScreen();
        }
    }
}

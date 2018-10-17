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

namespace SimpleReminder.Screens
{
    /// <summary>
    /// Логика взаимодействия для LoginScreen.xaml
    /// </summary>
    public partial class LoginScreen : UserControl
    {
        public delegate void Auth();

        public event Auth LoginDone;

        public LoginScreen()
        {
            // TODO Make all fields private!
            InitializeComponent();
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO Chek if user exists
            if(loginBox.Text != "andrew" && passwordBox.Password != "andrew")
            {
                MessageBox.Show("Password: andrew\nLogin: andrew");
                return;
            }
            //If user exists and password is correct
            if(LoginDone != null)
            {
                LoginDone();
            }
        }
    }
}
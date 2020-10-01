using System;
using System.Windows;
using PLWPF.AdminWindows;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for AdminMainWindow.xaml
    /// </summary>
    public partial class AdminMainWindow : Window
    {
        
        public AdminMainWindow()
        {
            InitializeComponent();

            if (ContainsEnglish(Users.Content.ToString()))
                Welcome.Text = "Welcome back, Admin!";
            else
                Welcome.Text = "!ברוך שובך, מנהל";
        }

        private bool ContainsEnglish(string text)
        {
            foreach (var c in text)
                if ((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'))
                    return true;
            return false;
        }

        private void Hosts_Click(object sender, RoutedEventArgs e)
        {
            Window hostsStatsWindow = new HostsStatsWindow();
            hostsStatsWindow.Show();
            Close();
        }

        private void Users_Click(object sender, RoutedEventArgs e)
        {
            Window usersStatsWindow = new UsersStatsWindow();
            usersStatsWindow.Show();
            Close();
        }

        private void Guests_Click(object sender, RoutedEventArgs e)
        {
            Window guestsStatsWindow = new GuestRequestsStatsWindow();
            guestsStatsWindow.Show();
            Close();
        }

        private void Orders_Click(object sender, RoutedEventArgs e)
        {
            Window ordersStatsWindow = new OrdersStatsWindow();
            ordersStatsWindow.Show();
            Close();
        }

        private void Money_Click(object sender, RoutedEventArgs e)
        {
            Window moneyStatsWindow = new MoneyStatsWindow();
            moneyStatsWindow.Show();
            Close();
        }

        private void HostingUnits_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("This page is under constructions. Please try to use another page.", "Sorry!", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Log_off_Click(object sender, RoutedEventArgs e)
        {
            Window LoginWindow = new Login();
            LoginWindow.Show();
            Close();
        }
    }
}

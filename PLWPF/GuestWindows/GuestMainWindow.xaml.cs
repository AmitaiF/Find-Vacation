using System;
using System.Windows;
using BE;
using BL;
using PLWPF.GuestWindows;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class GuestMainWindow : Window
    {
        Guest guest;
        
        public GuestMainWindow(Guest guest)
        {
            InitializeComponent();

            this.guest = guest;

            if (ContainsEnglish(watchHostingUnitsWindow.Content.ToString()))
                WelcomeBlock.Text = "Welcome " + this.guest.FirstName + "!";
            else
                WelcomeBlock.Text = "!" + this.guest.FirstName + " ברוך הבא";
        }

        private bool ContainsEnglish(string text)
        {
            foreach (var c in text)
                if ((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'))
                    return true;
            return false;
        }

        private void addGuestRequestWindow_Click(object sender, RoutedEventArgs e)
        {
            Window guestRequestWindow = new GuestRequestWindow(guest);
            guestRequestWindow.Show();
            this.Close();
        }

        private void watchHostingUnitWindow_Click(object sender, RoutedEventArgs e)
        {
            Window watchOfferedHostingUnitWindow = new WatchOrdersWindow(guest);
            watchOfferedHostingUnitWindow.Show();
            Close();
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
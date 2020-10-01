using System;
using System.Windows;
using BE;
using PLWPF.HostWindows;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for HostMainWindow.xaml
    /// </summary>
    public partial class HostMainWindow : Window
    {
        Host host;

        static Uri ENStrings = new Uri(@"/Resources/Languages/Strings_EN.xaml", UriKind.Relative);
        ResourceDictionary ENStringsR = Application.LoadComponent(ENStrings) as ResourceDictionary;


        public HostMainWindow(Host host)
        {
            InitializeComponent();

            this.host = host;

            if (ContainsEnglish(OrderBtn.Content.ToString()))
            {
                Welcome.Text = "Welcome back, " + this.host.FirstName + "!";
                MoneyEarned.Text = "You Earned: " + this.host.MoneyEarned + "$";
            }
            else
            {
                Welcome.Text = "!" + this.host.FirstName + " ברוך הבא";
                MoneyEarned.Text = this.host.MoneyEarned + "$ :הרווחת";
            }

        }

        private bool ContainsEnglish(string text)
        {
            foreach (var c in text)
                if ((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'))
                    return true;
            return false;
        }

        private void AddHostingUnitWindow_Click(object sender, RoutedEventArgs e)
        {
            Window addHostingWindow = new AddHostingUnitWindow(host);
            addHostingWindow.Show();
            Close();
        }

        private void AddOrder_Click(object sender, RoutedEventArgs e)
        {
            if (BL.SingletonFactoryBL.GetBL().GetHostingUnitsOfHost(host).Count == 0)
                MessageBox.Show("You don't have any hosting unit.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                Window addOrderWindow = new AddOrderWindow(host);
                addOrderWindow.Show();
                this.Close();
            }

        }

        private void WatchHostingUnit_Click(object sender, RoutedEventArgs e)
        {
            if (BL.SingletonFactoryBL.GetBL().GetHostingUnitsOfHost(host).Count == 0)
                MessageBox.Show("You don't have any hosting unit.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                Window updateHostingUnit = new UpdateHostingUnit(host);
                updateHostingUnit.Show();
                this.Close();
            }
        }

        private void WatchOrders_Click(object sender, RoutedEventArgs e)
        {
            Window watchOrdersWindow = new WatchOrdersWindow(host);
            watchOrdersWindow.Show();
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

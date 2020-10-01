using System;
using System.Windows;
using BE;
using BL;

namespace PLWPF.HostWindows
{
    /// <summary>
    /// Interaction logic for WatchOrdersWindow.xaml
    /// </summary>
    public partial class WatchOrdersWindow : Window
    {
        Host host;
        IBL bl;

        public WatchOrdersWindow(Host host)
        {
            InitializeComponent();

            this.host = host;
            bl = SingletonFactoryBL.GetBL();

            orderDataGrid.ItemsSource = bl.GetOrdersOfHost(this.host);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Data.CollectionViewSource orderViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("orderViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // orderViewSource.Source = [generic data source]
        }

        private void orderDataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Order order = orderDataGrid.SelectedItem as Order;
            if (order == null) ;
            else
            {
                Order newOrder = order.GetCopy();
                Window updateOrderWindow = new UpdateOrderWindow(newOrder, host);
                updateOrderWindow.Show();
                Close();
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Window Window = new HostMainWindow(bl.GetSpecificHosts(x => x.HostKey == host.HostKey)[0]); // to get updated host
            Window.Show();
            Close();
        }

        private void Log_off_Click(object sender, RoutedEventArgs e)
        {
            Window LoginWindow = new Login();
            LoginWindow.Show();
            Close();
        }
    }
}

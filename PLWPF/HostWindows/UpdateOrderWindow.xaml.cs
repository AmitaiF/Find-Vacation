using System;
using System.Windows;
using BE;
using BL;

namespace PLWPF.HostWindows
{
    /// <summary>
    /// Interaction logic for UpdateOrderWindow.xaml
    /// </summary>
    public partial class UpdateOrderWindow : Window
    {
        Order order;
        Host host;

        public UpdateOrderWindow(Order order, Host host)
        {
            InitializeComponent();

            this.order = order;
            this.host = host;
            OrderGrid.DataContext = this.order;

            statusComboBox.ItemsSource = Enum.GetValues(typeof(OrderStatus));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Data.CollectionViewSource orderViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("orderViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // orderViewSource.Source = [generic data source]
        }

        private void UpdateOrder_Click(object sender, RoutedEventArgs e)
        {
            OrderStatus orderStatus = (OrderStatus)statusComboBox.SelectedItem;

            try
            {
                SingletonFactoryBL.GetBL().UpdateOrder(order.OrderKey, orderStatus);
                Back_Click(null, null);
                Close();
            }
            catch (BlDealClosedException)
            {
                MessageBox.Show("You can't update status of closed order.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (BlInvalidKeyException)
            {
                MessageBox.Show("There was a problem. Please try again later.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (BlKeyNotFoundException)
            {
                MessageBox.Show("There was a problem. Please try again later.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Window watchOrdersWindow = new WatchOrdersWindow(host);
            watchOrdersWindow.Show();
            Close();
        }
    }
}

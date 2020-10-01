using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using BE;
using BL;

namespace PLWPF.GuestWindows
{
    /// <summary>
    /// Interaction logic for WatchOfferedHostingUnitWindow.xaml
    /// </summary>
    public partial class WatchOrdersWindow : Window
    {
        Guest guest;
        IBL bl;
        ObservableCollection<Order> offers;

        public WatchOrdersWindow(Guest guest)
        {
            InitializeComponent();

            this.guest = guest;
            bl = SingletonFactoryBL.GetBL();

            offers = new ObservableCollection<Order>(bl.GetOrders().FindAll(x => bl.GetSpecificGuestRequests(y => (y.GuestRequestKey == x.GuestRequestKey) && (y.Guest.Username == guest.Username)).FirstOrDefault() != null));
            OffersGrid.ItemsSource = offers;
        }

        private void OffersGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Order order = OffersGrid.SelectedItem as Order;
            if (order == null) ;
            else
            {
                HostingUnit hostingUnit = bl.GetHostingUnits().Find(x => x.HostingUnitKey == order.HostingUnitKey);
                Window watchHostingUnitWindow = new WatchHostingUnitWindow(hostingUnit, guest);
                watchHostingUnitWindow.Show();
                Close();
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Window Window = new GuestMainWindow(guest);
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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;
using BE;
using BL;

namespace PLWPF.GuestWindows
{
    /// <summary>
    /// Interaction logic for WatchOfferedHostingUnitWindow.xaml
    /// </summary>
    public partial class WatchOfferedHostingUnitWindow : Window
    {
        Guest guest;
        IBL bl;
        ObservableCollection<Order> offers = new ObservableCollection<Order>();

        public WatchOfferedHostingUnitWindow(Guest guest)
        {
            InitializeComponent();
            this.guest = guest;
            bl = SingletonFactoryBL.GetBL();
            
            offers = new ObservableCollection<Order>(bl.GetOrders().FindAll(x =>
            bl.GetGuestRequests().FindAll(y => (y.GuestRequestKey == x.GuestRequestKey) && (y.Guest == guest)).FirstOrDefault() != null));
            OffersGrid.ItemsSource = offers;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Window mainGuestWindow = new GuestMainWindow(guest);
            mainGuestWindow.Show();
            this.Close();
        }
    }
}

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
using System.Windows.Shapes;
using BE;
using BL;

namespace PLWPF.AdminWindows
{
    /// <summary>
    /// Interaction logic for OrdersStatsWindow.xaml
    /// </summary>
    public partial class OrdersStatsWindow : Window
    {
        IBL bl;

        public OrdersStatsWindow()
        {
            InitializeComponent();

            bl = SingletonFactoryBL.GetBL();
            GroupByComboBox.ItemsSource = new List<string> { "None", "Hosting Unit Key", "Guest Request Key", "Status" };

            //ordersDataGrid.ItemsSource = bl.GetOrders();
        }

        private void SearchByComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selection = GroupByComboBox.SelectedItem as string;
            var orders = new List<Order>();

            if (selection == "None")
                ordersDataGrid.ItemsSource = bl.GetOrders();

            else if (selection == "Hosting Unit Key")
            {
                var ordersGrouped = bl.GetOrdersByHostingUnitKey();
                foreach (var item in ordersGrouped)
                {
                    var i = item.GetEnumerator();
                    while (i.MoveNext())
                        orders.Add(i.Current);
                }
                ordersDataGrid.ItemsSource = orders;
            }

            else if (selection == "Guest Request Key")
            {
                var ordersGrouped = bl.GetOrdersByGuestRequestKey();
                foreach (var item in ordersGrouped)
                {
                    var i = item.GetEnumerator();
                    while (i.MoveNext())
                        orders.Add(i.Current);
                }
                ordersDataGrid.ItemsSource = orders;
            }

            else if (selection == "Status")
            {
                var ordersGrouped = bl.GetOrdersByStatus();
                foreach (var item in ordersGrouped)
                {
                    var i = item.GetEnumerator();
                    while (i.MoveNext())
                        orders.Add(i.Current);
                }
                ordersDataGrid.ItemsSource = orders;
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Window Window = new AdminMainWindow();
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

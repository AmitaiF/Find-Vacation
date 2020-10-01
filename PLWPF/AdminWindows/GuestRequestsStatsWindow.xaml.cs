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
using BL;
using BE;

namespace PLWPF.AdminWindows
{
    /// <summary>
    /// Interaction logic for GuestsStatsWindow.xaml
    /// </summary>
    public partial class GuestRequestsStatsWindow : Window
    {
        IBL bl;

        public GuestRequestsStatsWindow()
        {
            InitializeComponent();

            bl = SingletonFactoryBL.GetBL();
            GroupByComboBox.ItemsSource = new List<string> { "None", "Area", "Number Of Vacationers", "Type", "Entry Date", "Release Date" };
            guestRequestsDataGrid.ItemsSource = bl.GetGuestRequests();
        }

        private void SearchByComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selection = GroupByComboBox.SelectedItem as string;
            var guestRequests = new List<GuestRequest>();

            if (selection == "None")
                guestRequestsDataGrid.ItemsSource = bl.GetGuestRequests();

            else if (selection == "Area")
            {
                var guestRequestsGrouped = bl.GetGuestRequestsByArea();
                foreach (var item in guestRequestsGrouped)
                {
                    var i = item.GetEnumerator();
                    while (i.MoveNext())
                        guestRequests.Add(i.Current);
                }
                guestRequestsDataGrid.ItemsSource = guestRequests;
            }

            else if (selection == "Number Of Vacationers")
            {
                var guestRequestsGrouped = bl.GetGuestRequestsByNumOfVacationers();
                foreach (var item in guestRequestsGrouped)
                {
                    var i = item.GetEnumerator();
                    while (i.MoveNext())
                        guestRequests.Add(i.Current);
                }
                guestRequestsDataGrid.ItemsSource = guestRequests;
            }

            else if (selection == "Type")
            {
                var guestRequestsGrouped = bl.GetGuestRequestByType();
                foreach (var item in guestRequestsGrouped)
                {
                    var i = item.GetEnumerator();
                    while (i.MoveNext())
                        guestRequests.Add(i.Current);
                }
                guestRequestsDataGrid.ItemsSource = guestRequests;
            }

            else if (selection == "Entry Date")
            {
                var guestRequestsGrouped = bl.GetGuestRequestsByEntryDate();
                foreach (var item in guestRequestsGrouped)
                {
                    var i = item.GetEnumerator();
                    while (i.MoveNext())
                        guestRequests.Add(i.Current);
                }
                guestRequestsDataGrid.ItemsSource = guestRequests;
            }

            else if (selection == "Release Date")
            {
                var guestRequestsGrouped = bl.GetGuestRequestsByReleaseDate();
                foreach (var item in guestRequestsGrouped)
                {
                    var i = item.GetEnumerator();
                    while (i.MoveNext())
                        guestRequests.Add(i.Current);
                }
                guestRequestsDataGrid.ItemsSource = guestRequests;
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

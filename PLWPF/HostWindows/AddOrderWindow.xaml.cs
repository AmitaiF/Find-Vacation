using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using BE;
using BL;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for OrderWindow.xaml
    /// </summary>
    public partial class AddOrderWindow : Window
    {
        IBL bl;
        Host host;

        public AddOrderWindow(Host host)
        {
            InitializeComponent();

            this.host = host;
            bl = SingletonFactoryBL.GetBL();

            HostingUnits.ItemsSource = bl.GetHostingUnitsOfHost(host);
            guestRequestDataGrid.ItemsSource = GetRequestsSuitableHostingUnit();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Data.CollectionViewSource guestRequestViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("guestRequestViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // guestRequestViewSource.Source = [generic data source]
        }

        private List<GuestRequest> GetRequestsSuitableHostingUnit()
        {
            var hU = HostingUnits.SelectedItem as HostingUnit;
            return bl.GetSuitableRequests(hU);
        }

        private void HostingUnits_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            guestRequestDataGrid.ItemsSource = GetRequestsSuitableHostingUnit();
        }
        
        private void guestRequestDataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var guestRequest = guestRequestDataGrid.SelectedItem as GuestRequest;
            var hostingUnit = HostingUnits.SelectedItem as HostingUnit;
            if (guestRequest == null) ;
            else
            {
                string TextToShow = "Are you sure that you want to order " + guestRequest.Guest.FirstName + " " + guestRequest.Guest.LastName + " to " + hostingUnit.HostingUnitName + " from " + guestRequest.EntryDate.ToString("dd/MM/yy") + " to " + guestRequest.ReleaseDate.ToString("dd/MM/yy") + "?";
                MessageBoxResult messageBoxResult = MessageBox.Show(TextToShow, "Please Choose:", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    try
                    {
                        Order order = new Order();
                        order.CreateDate = DateTime.Today;
                        order.HostingUnitKey = hostingUnit.HostingUnitKey;
                        order.GuestRequestKey = guestRequest.GuestRequestKey;
                        bl.AddOrder(order);
                    }
                    catch (BlArgumentNullException)
                    {
                        MessageBox.Show("There was a problem. Please try again later.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (BlVacationDatesAlreadyOccupiedException)
                    {
                        MessageBox.Show("These dates already taken.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (BlNotSignedClearanceException)
                    {
                        MessageBox.Show("You didn't signed a clearance. Sign and then try again.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (BlHostingUnitDoesntExistException)
                    {
                        MessageBox.Show("There was a problem. Please try again later.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (BlGuestRequestDoesntExistException)
                    {
                        MessageBox.Show("There was a problem. Please try again later.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Window Window = new HostMainWindow(host);
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

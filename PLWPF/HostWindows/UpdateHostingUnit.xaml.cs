using System;
using System.Windows;
using System.Windows.Controls;
using BE;
using BL;

namespace PLWPF.HostWindows
{
    /// <summary>
    /// Interaction logic for UpdateHostingUnit.xaml
    /// </summary>
    public partial class UpdateHostingUnit : Window
    {
        Host host;
        IBL bl;
        HostingUnit hostingUnit;

        public UpdateHostingUnit(Host host)
        {
            InitializeComponent();

            bl = BL.SingletonFactoryBL.GetBL();
            this.host = host;

            areaComboBox.ItemsSource = Enum.GetValues(typeof(Area));
            typeComboBox.ItemsSource = Enum.GetValues(typeof(VacationType));
            HostingUnits.ItemsSource = bl.GetHostingUnitsOfHost(host);

            hostingUnit = HostingUnits.SelectedItem as HostingUnit;
            UpdateGrid.DataContext = hostingUnit;
        }

        private void Update_click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.UpdateHostingUnit(hostingUnit);
                MessageBox.Show("The hosting unit was updated.", "Success", MessageBoxButton.OK);
            }
            catch (BlArgumentNullException)
            {
                MessageBox.Show("There was a problem. Please try again later.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void DeleteHostingUnit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.DeleteHostingUnit(hostingUnit.HostingUnitKey);
                MessageBox.Show("The hosting unit was deleted.", "Success", MessageBoxButton.OK);
                HostingUnits.ItemsSource = bl.GetHostingUnitsOfHost(host);
            }
            catch (BlHostingUnitHasOpenOrderException)
            {
                MessageBox.Show("You can't delete hosting unit with open orders.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void HostingUnits_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            hostingUnit = HostingUnits.SelectedItem as HostingUnit;
            UpdateGrid.DataContext = hostingUnit;
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

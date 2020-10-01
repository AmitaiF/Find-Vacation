using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BE;
using BL;
using PLWPF.HostWindows;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for HostingUnitWindow.xaml
    /// </summary>
    public partial class WatchHostingUnitWindow : Window
    {
        Host host;
        IBL bl;

        public WatchHostingUnitWindow(Host host)
        {
            InitializeComponent();

            bl = SingletonFactoryBL.GetBL();
            this.host = host;

            hostingUnitDataGrid.ItemsSource = bl.GetHostingUnitsOfHost(host);
        }

        private void UnitCollection_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            HostingUnit hostingUnit = (HostingUnit)hostingUnitDataGrid.SelectedItem;
            if (hostingUnit == null) ;
            else
            {
                Window updateUnit = new UpdateHostingUnit(host);
                updateUnit.ShowDialog();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource hostingUnitViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("hostingUnitViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // hostingUnitViewSource.Source = [generic data source]
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Window HostMainWindow = new HostMainWindow(host);
            HostMainWindow.Show();
            this.Close();
        }

        private void Log_off_Click(object sender, RoutedEventArgs e)
        {
            Window LoginWindow = new Login();
            LoginWindow.Show();
            this.Close();
        }
    }
}

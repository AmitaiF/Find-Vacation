using System;
using System.Windows;
using BE;
using BL;

namespace PLWPF.GuestWindows
{
    /// <summary>
    /// Interaction logic for WatchHostingUnitWindow.xaml
    /// </summary>
    public partial class WatchHostingUnitWindow : Window
    {
        HostingUnit hostingUnit;
        Guest guest;

        public WatchHostingUnitWindow(HostingUnit hostingUnit, Guest guest)
        {
            InitializeComponent();

            this.hostingUnit = hostingUnit;
            this.guest = guest;
            hostingUnitName.DataContext = hostingUnit;
            VacationPropertiesGrid.DataContext = hostingUnit.VacationProperties;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource hostingUnitViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("hostingUnitViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // hostingUnitViewSource.Source = [generic data source]
            System.Windows.Data.CollectionViewSource vacationPropertiesViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("vacationPropertiesViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // vacationPropertiesViewSource.Source = [generic data source]
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

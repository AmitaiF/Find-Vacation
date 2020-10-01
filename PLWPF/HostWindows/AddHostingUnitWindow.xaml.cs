using BE;
using System.Windows;
using BL;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Linq;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for AddHostingUnitWindow.xaml
    /// </summary>
    public partial class AddHostingUnitWindow : Window
    {
        Host host;
        IBL bl;
        HostingUnit hostingUnit;
        VacationProperties vacProp;
        private List<string> errorMessages;

        public AddHostingUnitWindow(Host host)
        {
            InitializeComponent();

            this.host = host;
            bl = SingletonFactoryBL.GetBL();
            errorMessages = new List<string>();

            hostingUnit = new HostingUnit();
            vacProp = new VacationProperties();

            NewUnitGrid.DataContext = hostingUnit;
            VcationPropertiesGrid.DataContext = vacProp;

            areaComboBox.ItemsSource = Enum.GetValues(typeof(Area));
            typeComboBox.ItemsSource = Enum.GetValues(typeof(BE.VacationType));
        }

        private void Addbutton_Click(object sender, RoutedEventArgs e)
        {
            hostingUnit.Owner = host;
            hostingUnit.VacationProperties = vacProp;
            try
            {
                bl.AddHostingUnit(hostingUnit);
                MessageBox.Show(hostingUnit.HostingUnitName + " was added successfuly.");

                hostingUnit = new HostingUnit();
                NewUnitGrid.DataContext = hostingUnit;
                vacProp = new VacationProperties();
                VcationPropertiesGrid.DataContext = vacProp;
            }
            catch (BlArgumentNullException)
            {
                MessageBox.Show("There was a problem. Please try again later.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (BlNoVacationeersException)
            {
                MessageBox.Show("Please add some vacationeers.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (BlPriceLowException)
            {
                MessageBox.Show("The price you chose is too low.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (BlNameTooShortException)
            {
                MessageBox.Show("The Name has to be at least 5 characters.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (BlNameAlreadyExistException)
            {
                MessageBox.Show("The Name already exist. Please try another name.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void Window_Error(object sender, System.Windows.Controls.ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
                errorMessages.Add(e.Error.Exception.Message);
            else
                errorMessages.Remove(e.Error.Exception.Message);
            Addbutton.IsEnabled = !errorMessages.Any();
        }
    }
}

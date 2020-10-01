using BE;
using BL;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for HostRegistrationWindow.xaml
    /// </summary>
    public partial class HostRegistrationWindow : Window
    {
        IBL bl;
        Host host;
        public BankBranch bankBranch;
        private List<string> errorMessages;

        public HostRegistrationWindow(Host host)
        {
            InitializeComponent();

            this.host = host;
            bl = SingletonFactoryBL.GetBL();
            errorMessages = new List<string>();

            HostGrid.DataContext = this.host;
            phoneNumberTextBox.Text = ""; // erase default value
        }

        private void FinishRegistration_Click(object sender, RoutedEventArgs e)
        {
            HostGrid.DataContext = host;
            host.BankBranchDetails = bankBranch;
            if (host.BankBranchDetails.BranchNumber == 0)
                MessageBox.Show("You mus't choose a bank branch.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                try
                {
                    host.FinishedRegistration = true;
                    bl.UpdateHost(host);

                    Window loginWindiw = new Login();
                    loginWindiw.Show();
                    Close();
                }
                catch (BlArgumentNullException)
                {
                    MessageBox.Show("There was a problem. Please try again later.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (BlKeyNotFoundException)
                {
                    MessageBox.Show("There was a problem. Please try again later.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (BlInvalidPhoneNumberException)
                {
                    MessageBox.Show("The phone number invaild.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (BlOpenOrderException)
                {
                    MessageBox.Show("You can't cancel your collection clearance while you have open order.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (BlInvalidKeyException)
                {
                    MessageBox.Show("There was a problem. Please try again later.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (BlFileErrorException)
                {
                    MessageBox.Show("There was file error. Please try again later.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Window RegistrationWindow = new RegisterWindow();
            RegistrationWindow.Show();
            Close();
        }

        private void Window_Error(object sender, System.Windows.Controls.ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
                errorMessages.Add(e.Error.Exception.Message);
            else
                errorMessages.Remove(e.Error.Exception.Message);
            FinishButton.IsEnabled = !errorMessages.Any();
        }

        private void ChooseBank_Click(object sender, RoutedEventArgs e)
        {
            Window chooseBankBranchWindow = new BankBranchWindow(this);
            chooseBankBranchWindow.ShowDialog();
        }
    }
}

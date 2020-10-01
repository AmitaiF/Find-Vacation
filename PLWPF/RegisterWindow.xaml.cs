using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using BE;
using BL;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        User newUser;
        IBL bl;

        /// <summary>
        /// 
        /// </summary>
        public RegisterWindow()
        {
            InitializeComponent();

            newUser = new User();
            bl = SingletonFactoryBL.GetBL();

            userDetailsGrid.DataContext = newUser;

            List<UserType> comboBoxValues = new List<UserType>();
            comboBoxValues.Add(UserType.Guest);
            comboBoxValues.Add(UserType.Host);
            typeOfUser.ItemsSource = comboBoxValues;
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            userDetailsGrid.DataContext = newUser;

            newUser.Password = passwordTextBox.Password;
            string rePass = rePassword.Password;

            if (nicknameTextBox.Text == "" || passwordTextBox.Password == "" || rePassword.Password == "" || mail.Text == "")
                MessageBox.Show("You must fill all fields!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            else if (rePass != newUser.Password)
                MessageBox.Show("Passwords aren't equal!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                newUser.RegistrationDate = DateTime.Today;

                if (newUser.Type == UserType.Guest)
                    CompleteGuestRegistration();
                else
                    CompleteHostRegistration();

            }
        }

        private void CompleteGuestRegistration()
        {
            try
            {
                newUser.FinishedRegistration = true;
                Guest guest = GetGuest(newUser);
                bl.AddGuest(guest);
                GoToLogin();
            }
            catch (BlArgumentNullException)
            {
                MessageBox.Show("There was a problem. Please try again later.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (BlMailAlreadyExistException)
            {
                MessageBox.Show("Mail address already exist in the system. Please try another address.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (BlNickAlreadyExistException)
            {
                MessageBox.Show("Nickname already exist in the system. Please try another nickname.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (BlUsernameInvalidException)
            {
                MessageBox.Show("Username invalid.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (BlPasswordInvalidException)
            {
                MessageBox.Show("Password invalid.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (BlInvalidEmailException)
            {
                MessageBox.Show("Mail address invalid.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (BlFileErrorException)
            {
                MessageBox.Show("There was file error. Please try again later.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private Guest GetGuest(User user)
        {
            Guest guest = new Guest();

            guest.Username = user.Username;
            guest.Password = user.Password;
            guest.MailAddress = user.MailAddress;
            guest.FirstName = user.FirstName;
            guest.LastName = user.LastName;
            guest.RegistrationDate = user.RegistrationDate;
            guest.Type = user.Type;
            guest.FinishedRegistration = user.FinishedRegistration;

            return guest;
        }

        private Host GetHost(User user)
        {
            Host host = new Host();

            host.Username = user.Username;
            host.Password = user.Password;
            host.MailAddress = user.MailAddress;
            host.FirstName = user.FirstName;
            host.LastName = user.LastName;
            host.RegistrationDate = user.RegistrationDate;
            host.Type = user.Type;
            host.FinishedRegistration = false;
            host.PhoneNumber = "0500000000";

            return host;
        }

        private void CompleteHostRegistration()
        {
            Host host = GetHost(newUser);
            host.BankBranchDetails = new BankBranch() { BankName = "", BankNumber = 0, BranchAddress = "", BranchCity = "", BranchNumber = 0 };
            try
            {
                bl.AddHost(host);
                Window hostRegistrationWindow = new HostRegistrationWindow(host);
                hostRegistrationWindow.Show();
                Close();
            }
            catch (BlArgumentNullException)
            {
                MessageBox.Show("There was a problem. Please try again later.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (BlMailAlreadyExistException)
            {
                MessageBox.Show("Mail address already exist in the system. Please try another address.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (BlNickAlreadyExistException)
            {
                MessageBox.Show("Nickname already exist in the system. Please try another nickname.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (BlUsernameInvalidException)
            {
                MessageBox.Show("Username invalid.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (BlPasswordInvalidException)
            {
                MessageBox.Show("Password invalid.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (BlInvalidEmailException)
            {
                MessageBox.Show("Mail address invalid.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (BlFileErrorException)
            {
                MessageBox.Show("There was file error. Please try again later.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GoToLogin()
        {
            Window LoginWindow = new Login();
            LoginWindow.Show();
            Close();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            GoToLogin();
        }
    }
}


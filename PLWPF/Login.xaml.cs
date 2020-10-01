using BE;
using BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        IBL bl;
        User user;

        static Uri ENStrings = new Uri(@"/Resources/Languages/Strings_EN.xaml", UriKind.Relative);
        static Uri HEStrings = new Uri(@"/Resources/Languages/Strings_HE.xaml", UriKind.Relative);
        ResourceDictionary ENStringsR = Application.LoadComponent(ENStrings) as ResourceDictionary;
        ResourceDictionary HEStringsR = Application.LoadComponent(HEStrings) as ResourceDictionary;

        public Login()
        {
            InitializeComponent();

            Lang.ItemsSource = new List<string> { "English", "עברית" };

            if (ContainsEnglish(username.Text))
                Lang.SelectedIndex = 0;
            else
                Lang.SelectedIndex = 1;

            try
            {
                bl = SingletonFactoryBL.GetBL();
            }
            catch (BlFileErrorException)
            {
                MessageBox.Show("There was a problem with the files!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                Exit_Click(null, null);
            }

            bl.ActivateExpiredOrdersThread();
        }

        private bool ContainsEnglish(string text)
        {
            foreach (var c in text)
                if ((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'))
                    return true;
            return false;
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = this.username.Text;
            string password = this.password.Password;

            if (username == "" || password == "")
                MessageBox.Show("You must fill all fields!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            else
                TryToLogin(username, password);
        }

        private void TryToLogin(string username, string password)
        {
            if (username == "Admin" && password == bl.GetAdminPassword())
            {
                Window adminWindow = new AdminMainWindow();
                adminWindow.Show();
                Close();
            }
            else
                TryLoginToUser(username, password);
        }

        private void TryLoginToUser(string username, string password)
        {
            try
            {
                user = bl.GetUser(username);

                if (password == user.Password)
                {
                    if (user.Type == UserType.Guest)
                    {
                        Guest guest = GetGuest(user);
                        Window guestWindow = new GuestMainWindow(guest);
                        guestWindow.Show();
                    }
                    else if (user.Type == UserType.Host)
                    {
                        if (!user.FinishedRegistration)
                            CompleteHostRegistration(user);
                        else
                        {
                            Host host = GetHost(user);
                            Window hostMainWindow = new HostMainWindow(host);
                            hostMainWindow.Show();
                        }
                    }
                    Close();
                }
                else throw new BlUserDoesNotExistException();
            }
            catch (BlUserDoesNotExistException)
            {
                this.username.Text = "";
                this.password.Password = "";
                MessageBox.Show("Username or Password is incorrect.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private Guest GetGuest(User user)
        {
            return (from item in bl.GetGuests()
                    where item.Username == user.Username
                    select item).ToList().First();
        }

        private Host GetHost(User user)
        {
            return (from item in bl.GetHosts()
                    where item.Username == user.Username
                    select item).ToList().First();
        }

        private void CompleteHostRegistration(User user)
        {
            Host host = GetHost(user);
            Window hostRegistrationWindow = new HostRegistrationWindow(host);
            hostRegistrationWindow.Show();
            this.Close();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            Window registerWindow = new RegisterWindow();
            registerWindow.Show();
            Close();
        }

        private void ForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            Window forgotPasswordWindow = new ForgotPasswordWindow();
            forgotPasswordWindow.Show();
            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void username_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.Text = string.Empty;
            tb.GotFocus -= username_GotFocus;
        }

        private void password_GotFocus(object sender, RoutedEventArgs e)
        {
            PasswordBox tb = (PasswordBox)sender;
            tb.Password = string.Empty;
            tb.GotFocus -= password_GotFocus;
        }

        private void Instagram_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.instagram.com/nathanamitaivacationsystem/");
        }

        private void Facebook_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.facebook.com/nathanamitai.vacation.5");
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Lang_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Lang.SelectedItem as string == "English")
            {
                Application.Current.Resources.MergedDictionaries.Remove(HEStringsR);
                Application.Current.Resources.MergedDictionaries.Add(ENStringsR);
            }
            else
            {
                Application.Current.Resources.MergedDictionaries.Remove(ENStringsR);
                Application.Current.Resources.MergedDictionaries.Add(HEStringsR);
            }

        }
    }
}

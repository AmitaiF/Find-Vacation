using System;
using System.Windows;
using BL;
using BE;
using System.Linq;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for ForgotPasswordWindow.xaml
    /// </summary>
    public partial class ForgotPasswordWindow : Window
    {
        IBL bl;
        User user;

        public ForgotPasswordWindow()
        {
            InitializeComponent();

            bl = SingletonFactoryBL.GetBL();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Window LoginWindow = new Login();
            LoginWindow.Show();
            Close();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void SendBtn_Click(object sender, RoutedEventArgs e)
        {
            string mail = MailTextBox.Text;
            var user = bl.GetUsers().Find(x => x.MailAddress == mail);
            if (user == null)
                MessageBox.Show("The mail doesn't exist in the system!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                try
                {
                    bl.SendCodeMail(user);

                    this.user = user;

                    SendText.Visibility = Visibility.Hidden;
                    SendBtn.Visibility = Visibility.Hidden;
                    MailTextBox.Visibility = Visibility.Hidden;

                    SentText.Visibility = Visibility.Visible;
                    ConfirmBtn.Visibility = Visibility.Visible;
                    CodeTextBox.Visibility = Visibility.Visible;
                }
                catch(BlFileErrorException)
                {
                    MessageBox.Show("There was file error. Please try again later.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (BlKeyNotFoundException)
                {
                    MessageBox.Show("There was a problem. Please try again later.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (BlInvalidKeyException)
                {
                    MessageBox.Show("There was a problem. Please try again later.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (BlInvalidPhoneNumberException)
                {
                    MessageBox.Show("There was a problem. Please try again later.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (BlOpenOrderException)
                {
                    MessageBox.Show("There was a problem. Please try again later.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (BlArgumentNullException)
                {
                    MessageBox.Show("There was a problem. Please try again later.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            int code = int.Parse(CodeTextBox.Text);

            if (user.Type == UserType.Host)
            {
                Host host = bl.GetSpecificHosts(x => x.Username == user.Username).First();
                if (host.Code == code)
                    ShowPasswod(host);
                else
                    MessageBox.Show("The code you entered is wrong!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                Guest guest = bl.GetGuests().Find(x => x.Username == user.Username);
                if (guest.Code == code)
                    ShowPasswod(guest);
                else
                    MessageBox.Show("The code you entered is wrong!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShowPasswod(User user)
        {
            SentText.Visibility = Visibility.Hidden;
            ConfirmBtn.Visibility = Visibility.Hidden;
            CodeTextBox.Visibility = Visibility.Hidden;

            PasswordTextBlock.Visibility = Visibility.Visible;
            PasswordTextBlock.Text = "Your password is:\n" + user.Password;
        }
    }
}

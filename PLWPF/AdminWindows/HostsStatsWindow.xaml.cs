using System;
using System.Collections.Generic;
using System.Windows;
using BE;
using BL;

namespace PLWPF.AdminWindows
{
    /// <summary>
    /// Interaction logic for HostsStatsWindow.xaml
    /// </summary>
    public partial class HostsStatsWindow : Window
    {
        IBL bl;

        public HostsStatsWindow()
        {
            InitializeComponent();

            bl = SingletonFactoryBL.GetBL();
            hostDataGrid.ItemsSource = bl.GetHosts();
            List<string> SearchBy = new List<string> {"Registration Date", "Username", "First Name", "Last Name", "Mail Address",
                                                        "Phone Number", "Collection Clearance", "Finished Registration",
                                                        "Bank Name", "Bank Account Number" };
            SearchByComboBox.ItemsSource = SearchBy;
            TFSearch.ItemsSource = new List<string> { "Yes", "No" };
        }
        
        private bool NeedsComboBox(string selectedItem)
        {
            if (selectedItem == "Collection Clearance")
                return true;
            if (selectedItem == "Finished Registration")
                return true;
            return false;
        }

        private bool NeedsTextBox(string selectedItem)
        {
            if (selectedItem == "Username")
                return true;
            if (selectedItem == "First Name")
                return true;
            if (selectedItem == "Last Name")
                return true;
            if (selectedItem == "Mail Address")
                return true;
            if (selectedItem == "Phone Number")
                return true;
            if (selectedItem == "Bank Name")
                return true;
            if (selectedItem == "Bank Account Number")
                return true;
            return false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Data.CollectionViewSource hostViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("hostViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // hostViewSource.Source = [generic data source]
        }

        private void SearchByComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if(NeedsTextBox(SearchByComboBox.SelectedItem as string))
            {
                TextSearch.Visibility = Visibility.Visible;
                TFSearch.Visibility = Visibility.Hidden;
                DateSearch.Visibility = Visibility.Hidden;
            }
            else if(NeedsComboBox(SearchByComboBox.SelectedItem as string))
            {
                TextSearch.Visibility = Visibility.Hidden;
                TFSearch.Visibility = Visibility.Visible;
                DateSearch.Visibility = Visibility.Hidden;
            }
            else
            {
                TextSearch.Visibility = Visibility.Hidden;
                TFSearch.Visibility = Visibility.Hidden;
                DateSearch.Visibility = Visibility.Visible;
            }
        }

        private void TFSearch_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            string selectedItem = SearchByComboBox.SelectedItem as string;
            bool TFValue = (string)TFSearch.SelectedItem == "Yes" ? true : false;

            if (selectedItem == "Collection Clearance")
                hostDataGrid.ItemsSource = bl.GetSpecificHosts(x => x.CollectionClearance == TFValue);
            if (selectedItem == "Finished Registration")
                hostDataGrid.ItemsSource = bl.GetSpecificHosts(x => x.FinishedRegistration == TFValue);
        }

        private void TextSearch_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            string selectedItem = SearchByComboBox.SelectedItem as string;
            string TextSearchVal = TextSearch.Text;

            if (selectedItem == "Username")
                hostDataGrid.ItemsSource = bl.GetSpecificHosts(x => x.Username.ToLower().Contains(TextSearchVal.ToLower()));
            if (selectedItem == "First Name")
                hostDataGrid.ItemsSource = bl.GetSpecificHosts(x => x.FirstName.ToLower().Contains(TextSearchVal.ToLower()));
            if (selectedItem == "Last Name")
                hostDataGrid.ItemsSource = bl.GetSpecificHosts(x => x.LastName.ToLower().Contains(TextSearchVal.ToLower()));
            if (selectedItem == "Mail Address")
                hostDataGrid.ItemsSource = bl.GetSpecificHosts(x => x.MailAddress.ToLower().Contains(TextSearchVal.ToLower()));
            if (selectedItem == "Phone Number")
                hostDataGrid.ItemsSource = bl.GetSpecificHosts(x => x.PhoneNumber.ToLower().Contains(TextSearchVal.ToLower()));
            if (selectedItem == "Bank Name")
                hostDataGrid.ItemsSource = bl.GetSpecificHosts(x => x.BankBranchDetails.BankName.ToLower().Contains(TextSearchVal.ToLower()));
            if (selectedItem == "Bank Account Number")
                hostDataGrid.ItemsSource = bl.GetSpecificHosts(x => x.BankAccountNumber.ToString().Contains(TextSearchVal));
        }

        private void DateSearch_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            hostDataGrid.ItemsSource = bl.GetSpecificHosts(x => x.RegistrationDate == DateSearch.SelectedDate);
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

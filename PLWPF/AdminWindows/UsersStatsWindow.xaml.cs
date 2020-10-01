using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using BE;
using BL;

namespace PLWPF.AdminWindows
{
    /// <summary>
    /// Interaction logic for UsersStatsWindow.xaml
    /// </summary>
    public partial class UsersStatsWindow : Window
    {
        IBL bl;

        public UsersStatsWindow()
        {
            InitializeComponent();

            bl = SingletonFactoryBL.GetBL();
            GroupByComboBox.ItemsSource = new List<string> { "None", "First Name", "Last Name", "Registration Date",
                                                             "Type", "Finished Registration" };
            userDataGrid.ItemsSource = bl.GetUsers();
        }

        private void SearchByComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selection = GroupByComboBox.SelectedItem as string;
            var users = new List<User>();

            if (selection == "First Name")
            {
                var usersGrouped = bl.GetUsersByFirstName();
                foreach (var item in usersGrouped)
                {
                    var i = item.GetEnumerator();
                    while (i.MoveNext())
                        users.Add(i.Current);
                }
                userDataGrid.ItemsSource = users;
            }

            if (selection == "Last Name")
            {
                var usersGrouped = bl.GetUsersByLastName();
                foreach (var item in usersGrouped)
                {
                    var i = item.GetEnumerator();
                    while (i.MoveNext())
                        users.Add(i.Current);
                }
                userDataGrid.ItemsSource = users;
            }

            if (selection == "Finished Registration")
            {
                var usersGrouped = bl.GetUsersByFinishedRegistration();
                foreach (var item in usersGrouped)
                {
                    var i = item.GetEnumerator();
                    while (i.MoveNext())
                        users.Add(i.Current);
                }
                userDataGrid.ItemsSource = users;
            }

            if (selection == "Registration Date")
            {
                var usersGrouped = bl.GetUsersByRegistrationDate();
                foreach (var item in usersGrouped)
                {
                    var i = item.GetEnumerator();
                    while (i.MoveNext())
                        users.Add(i.Current);
                }
                userDataGrid.ItemsSource = users;
            }

            if (selection == "Type")
            {
                var usersGrouped = bl.GetUsersByType();
                foreach (var item in usersGrouped)
                {
                    var i = item.GetEnumerator();
                    while (i.MoveNext())
                        users.Add(i.Current);
                }
                userDataGrid.ItemsSource = users;
            }

            if(selection=="None")
                userDataGrid.ItemsSource = bl.GetUsers();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // userViewSource.Source = [generic data source]
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BE;
using BL;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for GuestRequestWindow.xaml
    /// </summary>
    public partial class GuestRequestWindow : Window
    {
        GuestRequest guestRequest;
        IBL bl;
        List<string> errorMessages;

        public GuestRequestWindow(Guest guest)
        {
            InitializeComponent();

            bl = SingletonFactoryBL.GetBL();
            errorMessages = new List<string>();

            entryDateDatePicker.SelectedDate = DateTime.Now;

            guestRequest = new GuestRequest();
            guestRequest.VacationProperties = new VacationProperties();
            guestRequest.Guest = guest;

            addGRGrid.DataContext = guestRequest;

            typeComboBox.ItemsSource = Enum.GetValues(typeof(BE.VacationType));
            areaComboBox.ItemsSource = Enum.GetValues(typeof(BE.Area));
            gardenComboBox.ItemsSource = Enum.GetValues(typeof(BE.Extension));
            bBQComboBox.ItemsSource = Enum.GetValues(typeof(BE.Extension));
            childernAttractionsComboBox.ItemsSource = Enum.GetValues(typeof(BE.Extension));
            jacuzziComboBox.ItemsSource = Enum.GetValues(typeof(BE.Extension));
            nearbyKosherFoodComboBox.ItemsSource = Enum.GetValues(typeof(BE.Extension));
            nearbyRestaurantComboBox.ItemsSource = Enum.GetValues(typeof(BE.Extension));
            nearbySynagogueComboBox.ItemsSource = Enum.GetValues(typeof(BE.Extension));
            poolComboBox.ItemsSource = Enum.GetValues(typeof(BE.Extension));

            guestRequest.EntryDate = DateTime.Today;
            guestRequest.ReleaseDate = DateTime.Today.AddDays(1);

        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            addGRGrid.DataContext = guestRequest;

            guestRequest.RegistrationDate = guestRequest.Guest.RegistrationDate;

            try
            {
                bl.AddGuestRequest(guestRequest);
                MessageBox.Show("Your request was added successfully.");
                guestRequest.GuestRequestKey = 0;
            }
            catch (BlArgumentNullException)
            {
                MessageBox.Show("There was a problem. Please try again later.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (BlEntryDateException)
            {
                MessageBox.Show("Entry date must be bigger than release date. Please try again.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (BlVacationInPastException)
            {
                MessageBox.Show("Your entry date is in the past. Please choose another one.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (BlNoVacationeersException)
            {
                MessageBox.Show("You don't have any vacationeer in your request.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (BlPriceLowException)
            {
                MessageBox.Show("The price you chose is too low.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Window guestMainWindow = new GuestMainWindow(guestRequest.Guest);
            guestMainWindow.Show();
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
            addButton.IsEnabled = !errorMessages.Any();
        }
    }
}

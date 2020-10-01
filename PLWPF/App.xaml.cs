using System.Windows;
using BE;
using BL;
using System;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            //InitAdmin();
        }

        //private void InitAdmin()
        //{
        //    User admin = new User();
        //    admin.Username = "Admin";
        //    admin.FirstName = "a";
        //    admin.Password = "123456";
        //    admin.MailAddress = "mail@admin.com";
        //    admin.Type = UserType.Admin;
        //    BL.SingletonFactoryBL.GetBL().AddUser(admin);

        //    Guest guest = new Guest();
        //    guest.FinishedRegistration = true;
        //    guest.FirstName = "amitai";
        //    guest.LastName = "farber";
        //    guest.MailAddress = "a@f";
        //    guest.Password = "123456";
        //    guest.RegistrationDate = DateTime.Today;
        //    guest.Type = UserType.Guest;
        //    guest.Username = "123456";
        //    SingletonFactoryBL.GetBL().AddGuest(guest);
        //    SingletonFactoryBL.GetBL().AddUser(guest);

        //    Host host = new Host();
        //    host.BankAccountNumber = 1;
        //    host.BankBranchDetails = SingletonFactoryBL.GetBL().GetBankBranches()[0];
        //    host.CollectionClearance = true;
        //    host.FinishedRegistration = true;
        //    host.FirstName = "a";
        //    host.LastName = "f";
        //    host.MailAddress = "a@d";
        //    host.PhoneNumber = "0581994712";
        //    host.RegistrationDate = DateTime.Today;
        //    host.Type = UserType.Host;
        //    host.Username = "654321";
        //    host.Password = "654321";
        //    SingletonFactoryBL.GetBL().AddHost(host);
        //    SingletonFactoryBL.GetBL().AddUser(host);
        //}
    }
}

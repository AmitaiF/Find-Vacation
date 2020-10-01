using System;
using BE;
using System.Collections.Generic;
using System.Linq;

namespace BL
{


    /// <summary>
    /// Interface for Business Layer, which responsible for all the logic of the software.
    /// </summary>
    public interface IBL
    {
        //---------------------------GuestRequest Methodes---------------------------//

        #region GuestRequest Methodes

        /// <summary>
        ///  Adds a new GuestRequest to the list in DataSource.
        /// </summary>
        /// <param name="guestRequest">New GuestRequest to be added.</param>
        void AddGuestRequest(GuestRequest guestRequest);

        /// <summary>
        /// Updates status for a given GuestRequest, using key.
        /// </summary>
        /// <param name="guestRequestKey">A given key for updating.</param>
        /// <param name="newStatus">New status for the guestRequest.</param>
        void UpdateGuestRequest(int guestRequestKey, GuestRequestStatus newStatus);
        
        /// <summary>
        /// Get the guest requests from data source.
        /// </summary>
        /// <returns>List of GuestRequest from data source.</returns>
        List<GuestRequest> GetGuestRequests();

        /// <summary>
        /// A function that gets specific condition (as a delegate) and finds all guest requests that fits this condition.
        /// </summary>
        /// <param name="conditionFunc">A delegate that define conditions for guest requests.</param>
        /// <returns>List of GuestRequest that fits the specific condition.</returns>
        List<GuestRequest> GetSpecificGuestRequests(Func<GuestRequest, bool> conditionFunc);

        #endregion

        //---------------------------HostingUnit Methodes---------------------------//

        #region HostingUnit Methodes

        /// <summary>
        /// Adds a new HostingUnit to the list in DataSource.
        /// </summary>
        /// <param name="hostingUnit">New HostingUnit to be added.</param>
        void AddHostingUnit(HostingUnit hostingUnit);

        /// <summary>
        /// Delete given HostingUnit from the list in DataSource, using key.
        /// </summary>
        /// <param name="hostingUnitKey">New HostingUnit to be deleted</param>
        void DeleteHostingUnit(int hostingUnitKey);

        /// <summary>
        /// Updates a given HostingUnit, using key.
        /// </summary>
        /// <param name="newHostingUnit">New parameters for the given HostingUnit.</param>
        void UpdateHostingUnit(HostingUnit newHostingUnit);

        /// <summary>
        /// Get hosting units from data source.
        /// </summary>
        /// <returns>List of HostingUnit from data source.</returns>
        List<HostingUnit> GetHostingUnits();

        /// <summary>
        /// A function that gets a date and number of vacation days and finds all available hosting units on that date along the length.
        /// </summary>
        /// <param name="entryDate">Entry date for the vacation.</param>
        /// <param name="vacationLength">Length for the vacation</param>
        /// <returns>List of all available hosting units on entryDate along the length.</returns>
        List<HostingUnit> GetAvailableHostingUnits(DateTime entryDate, int vacationLength);

        #endregion

        //---------------------------Order Methodes---------------------------//

        #region Order Methodes

        /// <summary>
        /// Adds a new order to the list in the DataSource
        /// </summary>
        /// <param name="order">New Order to be added.</param>
        void AddOrder(Order order);

        /// <summary>
        /// Updates status for a given Order, using key.
        /// </summary>
        /// <param name="orderKey">The key of Order we want to update.</param>
        /// <param name="newStatus">New status for the Order.</param>
        void UpdateOrder(int orderKey, OrderStatus newStatus);

        /// <summary>
        /// Get Orders from data source.
        /// </summary>
        /// <returns>List of Order from DataSource.</returns>
        List<Order> GetOrders();

        /// <summary>
        /// A function that gets a number of days for expiring.
        /// The function finds all orders that have expired since they were created/since the email was sent to the customer.
        /// </summary>
        /// <param name="daysToExpire">Number of days for expiring order.</param>
        /// <returns></returns>
        List<Order> GetExpiredOrders(int daysToExpire);

        /// <summary>
        /// Get Orders of specific host.
        /// </summary>
        /// <returns>List of Order from DataSource.</returns>
        List<Order> GetOrdersOfHost(Host host);

        /// <summary>
        /// A function that finds the number of orders that a guestRequest got.
        /// </summary>
        /// <param name="guestRequest">Guest request that we want to find how many orders it got.</param>
        /// <returns>Number of orders guestRequest got.</returns>
        int GetNumOfOrders(GuestRequest guestRequest);

        /// <summary>
        /// A function that finds how many orders were sent to a guest or closed a deal via the web, from specific HostingUnit.
        /// </summary>
        /// <param name="hostingUnit">The hosting unit we want to find how many orders it sent.</param>
        /// <returns>Number of orders which statused "MailSent".</returns>
        int GetNumOfMailSentOrDealClosedOrders(HostingUnit hostingUnit);

        /// <summary>
        /// A function that updates order status to "MailSent".
        /// </summary>
        /// <param name="orderKey">The key of the order we want to update.</param>
        void MailSent(int orderKey);

        /// <summary>
        /// Activate the expired orders remover thread.
        /// </summary>
        void ActivateExpiredOrdersThread();

        #endregion

        //---------------------------Grouping Methodes---------------------------//

        #region Grouping Methodes

        /// <summary>
        /// A function that returns all hosts grouping by number of the hosting units they have.
        /// </summary>
        /// <returns>All Hosts grouped by number of hosting units.</returns>
        List<IGrouping<int, Host>> GetHostsByNumOfHostingUnits();

        /// <summary>
        /// A function that returns all guest requests grouping by area.
        /// </summary>
        /// <returns>All GuestsRequests grouped by area.</returns>
        List<IGrouping<Area, GuestRequest>> GetGuestRequestsByArea();

        /// <summary>
        /// A function that returns all guest requests grouping by Status.
        /// </summary>
        /// <returns>All GuestsRequests grouped by Status.</returns>
        List<IGrouping<GuestRequestStatus, GuestRequest>> GetGuestRequestsByStatus();

        /// <summary>
        /// A function that returns all guest requests grouping by EntryDate.
        /// </summary>
        /// <returns>All GuestsRequests grouped by EntryDate.</returns>
        List<IGrouping<DateTime, GuestRequest>> GetGuestRequestsByEntryDate();

        /// <summary>
        /// A function that returns all guest requests grouping by ReleaseDate.
        /// </summary>
        /// <returns>All GuestsRequests grouped by ReleaseDate.</returns>
        List<IGrouping<DateTime, GuestRequest>> GetGuestRequestsByReleaseDate();

        /// <summary>
        /// A function that returns all GuestRequests grouping by number of vacationers.
        /// </summary>
        /// <returns>All GuestRequests grouped by num of vacationers.</returns>
        List<IGrouping<int, GuestRequest>> GetGuestRequestsByNumOfVacationers();

        /// <summary>
        /// A function that returns all Hosting Units grouping by area.
        /// </summary>
        /// <returns>All hosting units grouped by area.</returns>
        List<IGrouping<Area, HostingUnit>> GetHostingUnitsByArea();

        /// <summary>
        /// A function that returns all GuestRequests grouping by type of vacation.
        /// </summary>
        /// <returns>All GuestRequests grouped by type of vacation.</returns>
        List<IGrouping<VacationType, GuestRequest>> GetGuestRequestByType();

        /// <summary>
        /// A function that returns all Users grouping by type of user.
        /// </summary>
        /// <returns>All Users grouped by type of user.</returns>
        List<IGrouping<UserType, User>> GetUsersByType();

        /// <summary>
        /// A function that returns all Users grouping by First Name.
        /// </summary>
        /// <returns>All Users grouped by First Name.</returns>
        List<IGrouping<string, User>> GetUsersByFirstName();

        /// <summary>
        /// A function that returns all Users grouping by Last Name.
        /// </summary>
        /// <returns>All Users grouped by Last Name.</returns>
        List<IGrouping<string, User>> GetUsersByLastName();

        /// <summary>
        /// A function that returns all Users grouping by Registration Date.
        /// </summary>
        /// <returns>All Users grouped by Registration Date.</returns>
        List<IGrouping<DateTime, User>> GetUsersByRegistrationDate();

        /// <summary>
        /// A function that returns all Users grouping by Finished Registration.
        /// </summary>
        /// <returns>All Users grouped by Finished Registration.</returns>
        List<IGrouping<bool, User>> GetUsersByFinishedRegistration();

        /// <summary>
        /// A function that returns all Oders grouping by a hostingUnitKey.
        /// </summary>
        /// <returns>All orders grouped by a hostingUnitKey.</returns>
        List<IGrouping<int, Order>> GetOrdersByHostingUnitKey();

        /// <summary>
        /// A function that returns all Oders grouping by a GuestRequestKey.
        /// </summary>
        /// <returns>All orders grouped by a GuestRequestKey.</returns>
        List<IGrouping<int, Order>> GetOrdersByGuestRequestKey();

        /// <summary>
        /// A function that returns all Oders grouping by a Status.
        /// </summary>
        /// <returns>All orders grouped by a Status.</returns>
        List<IGrouping<OrderStatus, Order>> GetOrdersByStatus();

        #endregion

        //---------------------------BankBranch Methodes---------------------------//

        #region BankBranch Methodes

        /// <summary>
        /// Get list of bank branches.
        /// </summary>
        /// <returns>List of BankBranch.</returns>
        List<BankBranch> GetBankBranches();

        #endregion

        //---------------------------Date Methodes---------------------------//

        #region Date Methodes

        /// <summary>
        /// A function that gets one or two dates. The function finds the number of days that have passed from the first date to the second.
        /// If only one date has been received - calculates length from the first date to the present.
        /// </summary>
        /// <param name="dates">Dates for calculating.</param>
        /// <returns>Length from one date to another, or to the presents.</returns>
        int GetNumOfDays(params DateTime[] dates);

        #endregion

        //---------------------------User Methodes---------------------------//

        #region User Methodes

        /// <summary>
        /// Get list of users.
        /// </summary>
        /// <returns>List of User.</returns>
        List<User> GetUsers();

        /// <summary>
        /// Get a user (if exists) by a username.
        /// </summary>
        /// <param name="username">The username of the user we are looking for.</param>
        /// <returns>User we looked for.</returns>
        User GetUser(string username);

        /// <summary>
        /// Send confirmation code to user in mail.
        /// </summary>
        /// <param name="user">The user we send to.</param>
        void SendCodeMail(User user);

        #endregion

        //----------------------------------Host Methodes----------------------------------//

        #region Host Methodes

        /// <summary>
        /// Get list of Hosts.
        /// </summary>
        /// <returns>List of host.</returns>
        List<Host> GetHosts();

        /// <summary>
        /// Adds new hosts to the hosts in the system.
        /// </summary>
        /// <param name="host">New host to be added.</param>
        void AddHost(Host host);

        /// <summary>
        /// Updates a host.
        /// </summary>
        /// <param name="host">New host to be updated.</param>
        void UpdateHost(Host host);

        /// <summary>
        /// Get list of all hosting units belong to host.
        /// </summary>
        /// <returns>List of all hosting units belong to host.</returns>
        List<HostingUnit> GetHostingUnitsOfHost(Host host);

        /// <summary>
        /// Get list of all suitable requests for specific hosting unit.
        /// </summary>
        /// <param name="hostingUnit">The hosting unit we need to find requests for.</param>
        /// <returns>List of all suitable requests for specific hosting unit.</returns>
        List<GuestRequest> GetSuitableRequests(HostingUnit hostingUnit);

        /// <summary>
        /// A function that gets specific condition (as a delegate) and finds all Hosts that fits this condition.
        /// </summary>
        /// <param name="conditionFunc">A delegate that define conditions for host.</param>
        /// <returns>List of Host that fits the specific condition.</returns>
        List<Host> GetSpecificHosts(Func<Host, bool> conditionFunc);


        #endregion

        //----------------------------------Guest Methodes----------------------------------//

        #region Guest Methodes

        /// <summary>
        /// Get list of guests.
        /// </summary>
        /// <returns>List of guest.</returns>
        List<Guest> GetGuests();

        /// <summary>
        /// Adds new guest to the guests in the system.
        /// </summary>
        /// <param name="guest">New guest to be added.</param>
        void AddGuest(Guest guest);

        #endregion
        
        //----------------------------------Admin Methodes----------------------------------//

        #region Admin Methodes

        /// <summary>
        /// Gets the password of the admin from the XML file.
        /// </summary>
        /// <returns>The password of the Admin</returns>
        string GetAdminPassword();

        #endregion
    }
}
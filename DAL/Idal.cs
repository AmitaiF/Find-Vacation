using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DAL
{
    /// <summary>
    /// Interface for Data Access Layer, which responsible to load data from data sources.
    /// </summary>
    public interface Idal
    {
        //----------------------------------GuestRequest Methodes----------------------------------//

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

        #endregion

        //----------------------------------HostingUnit Methodes----------------------------------//

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

        #endregion

        //----------------------------------Order Methodes----------------------------------//

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
        /// <returns>List of Order from data source.</returns>
        List<Order> GetOrders();

        #endregion

        //----------------------------------BankBranch Methodes----------------------------------//

        #region BankBranch Methodes

        /// <summary>
        /// Get list of bank branches.
        /// </summary>
        /// <returns>List of BankBranch.</returns>
        List<BankBranch> GetBankBranches();

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
        /// <param name="user">New guest to be added.</param>
        void AddGuest(Guest guest);

        /// <summary>
        /// Updates a guest.
        /// </summary>
        /// <param name="guest">The new guest values.</param>
        void UpdateGuest(Guest guest);

        #endregion

        //----------------------------------Admin Methodes----------------------------------//

        #region Admin Methodes

        /// <summary>
        /// Gets the password of the admin, as was define in the XML file.
        /// </summary>
        /// <returns>String that represents the Admin's password</returns>
        string GetAdminPassword();
        
        #endregion
    }
}

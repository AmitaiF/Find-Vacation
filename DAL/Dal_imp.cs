using System;
using System.Collections.Generic;
using System.Linq;
using BE;
using DS;

namespace DAL
{
    /// <summary>
    /// Implementaion of Idal. Manages all the data resources.
    /// Old version, new version is Dal_XML_Imp
    /// </summary>
    public class Dal_imp : Idal
    {

        //----------------------------------GuestRequest Methodes----------------------------------//

        #region GuestRequest Methodes

        public void AddGuestRequest(GuestRequest guestRequest)
        {
            // check invalid key
            if (IsGRKeyInvalid(guestRequest.GuestRequestKey) || IsGuestRequestExists(guestRequest.GuestRequestKey))
                guestRequest.GuestRequestKey = Configuration.GuestRequestKey++;

            if (guestRequest.Status == GuestRequestStatus.NotAddedYet)
                guestRequest.Status = GuestRequestStatus.Active;

            DataSource.GuestRequests.Add(guestRequest.Copy()); // add to the list
        }

        public void UpdateGuestRequest(int guestRequestKey, GuestRequestStatus newStatus)
        {
            // check invalid key
            if (IsGRKeyInvalid(guestRequestKey))
                throw new DalInvalidKeyException();

            // find guestRequest
            var GRForUpdate = (from item in DataSource.GuestRequests
                               where item.GuestRequestKey == guestRequestKey
                               select item).ToList();

            if (GRForUpdate.Count() == 0)
                throw new DalKeyNotFoundException();

            else
            {
                DeleteGuestRequest(GRForUpdate.First().GuestRequestKey);
                GRForUpdate.First().Status = newStatus;
                AddGuestRequest(GRForUpdate.First());
            }
        }

        private void DeleteGuestRequest(int guestRequestKey)
        {
            if (guestRequestKey < 10000000)
                throw new DalInvalidKeyException();

            var GRForDelete = (from item in DataSource.GuestRequests
                               where item.GuestRequestKey == guestRequestKey
                               select item).ToList();

            if (GRForDelete.Count() == 0)
                throw new DalKeyNotFoundException();
            else
                DataSource.GuestRequests.Remove(GRForDelete.First());
        }

        public List<GuestRequest> GetGuestRequests()
        {
            return (from item in DataSource.GuestRequests
                    select item.Copy()).ToList();
        }

        private bool IsGuestRequestExists(int guestRequestKey)
        {
            int NumOfExistArg = (from item in DataSource.GuestRequests
                                 where item.GuestRequestKey == guestRequestKey
                                 select item).Count();
            if (NumOfExistArg > 0)
                return true;
            return false;
        }

        private static bool IsGRKeyInvalid(int guestRequestKey)
        {
            return guestRequestKey < 10000000;
        }

        #endregion

        //----------------------------------HostingUnit Methodes----------------------------------//

        #region HostingUnit Methodes

        public void AddHostingUnit(HostingUnit hostingUnit)
        {
            // check invalid key
            if (IsHUKeyInvalid(hostingUnit.HostingUnitKey) || IsHostingUnitExists(hostingUnit.HostingUnitKey))
                hostingUnit.HostingUnitKey = Configuration.HostingUnitKey++;

            hostingUnit.Diary = new bool[12, 31];
            InitDiary(hostingUnit.Diary);
            DataSource.HostingUnits.Add(hostingUnit.Copy()); // add to the list
        }

        private void InitDiary(bool[,] diary)
        {
            for (int i = 0; i < 12; i++)
                for (int j = 0; j < 31; j++)
                    diary[i, j] = false;
        }

        public void DeleteHostingUnit(int hostingUnitKey)
        {
            // check invalid key
            if (hostingUnitKey < 10000000)
                throw new DalInvalidKeyException();

            // find the hostingUnit
            var HUForDelete = (from item in DataSource.HostingUnits
                               where item.HostingUnitKey == hostingUnitKey
                               select item).ToList();

            if (HUForDelete.Count() == 0)
                throw new DalKeyNotFoundException();
            else
                DataSource.HostingUnits.Remove(HUForDelete.First());
        }

        public void UpdateHostingUnit(HostingUnit newHostingUnit)
        {
            // check invalid HostingUnitkey.
            if (newHostingUnit.HostingUnitKey < 10000000)
                throw new DalInvalidKeyException();
            
            // it causes that diary always full of false, since we initialize diary inside AddHostingUnit.
            // but i fixed it in Dal_XML_Imp
            DeleteHostingUnit(newHostingUnit.HostingUnitKey);
            AddHostingUnit(newHostingUnit);
        }

        public List<HostingUnit> GetHostingUnits()
        {
            return (from item in DataSource.HostingUnits
                    select item.Copy()).ToList();
        }

        private bool IsHostingUnitExists(int hostingUnitKey)
        {
            int NumOfExistArg = (from item in DataSource.HostingUnits
                                 where item.HostingUnitKey == hostingUnitKey
                                 select item).Count();
            if (NumOfExistArg > 0)
                return true;
            return false;
        }

        private static bool IsHUKeyInvalid(int hostingUnitKey)
        {
            return hostingUnitKey < 10000000;
        }

        #endregion

        //----------------------------------Order Methodes----------------------------------//

        #region Order Methodes

        public void AddOrder(Order order)
        {
            // check invalid key
            if (IsOKeyInvalid(order.OrderKey) || IsOrderKeyExist(order.OrderKey))
                order.OrderKey = Configuration.OrderKey++;

            DataSource.Orders.Add(order.Copy()); // add to the list
        }

        public void UpdateOrder(int orderKey, OrderStatus newStatus)
        {
            // check invalid orderKey
            if (IsOKeyInvalid(orderKey))
                throw new DalInvalidKeyException();

            var OForUpdate = (from item in DataSource.Orders
                              where item.OrderKey == orderKey
                              select item).ToList();

            if (OForUpdate.Count() == 0)
                throw new DalKeyNotFoundException();
            else
            {
                DeleteOrder(orderKey);
                OForUpdate.First().Status = newStatus;
                AddOrder(OForUpdate.First());
            }
        }

        private void DeleteOrder(int orderKey)
        {
            // check invalid key
            if (orderKey < 1)
                throw new DalInvalidKeyException();

            // find the order
            var OForDelete = (from item in DataSource.Orders
                              where item.OrderKey == orderKey
                              select item).ToList();

            if (OForDelete.Count() == 0)
                throw new DalKeyNotFoundException();
            else
                DataSource.Orders.Remove(OForDelete.First());
        }

        public List<Order> GetOrders()
        {
            return (from item in DataSource.Orders
                    select item.Copy()).ToList();
        }

        private bool IsOrderKeyExist(int orderKey)
        {
            int NumOfExistArg = (from item in DataSource.Orders
                                 where item.OrderKey == orderKey
                                 select item).Count();
            if (NumOfExistArg > 0)
                return true;
            return false;
        }

        private static bool IsOKeyInvalid(int orderKey)
        {
            return orderKey < 10000000;
        }

        #endregion

        //----------------------------------BankBranch Methodes----------------------------------//

        #region BankBranch Methodes

        public List<BankBranch> GetBankBranches()
        {
            // create list of bank branches
            List<BankBranch> bankBranches = new List<BankBranch>();

            // Old version, that returns 5 banks.
            // New and working implementantion in Dal_XML_Imp
            for (int i = 0; i < 5; ++i)
            {
                // insert parameters
                bankBranches.Add(new BankBranch
                {
                    BankNumber = (i + 1),
                    BankName = "Bank " + (i + 1),
                    BranchAddress = "Street " + (i + 1),
                    BranchCity = "City " + (i + 1),
                    BranchNumber = 100 + (i + 1)
                });
            }

            return bankBranches;
        }

        #endregion

        //----------------------------------Host Methodes----------------------------------//

        #region Host Methodes

        public List<Host> GetHosts()
        {
            return DataSource.Hosts;
        }

        public void AddHost(Host host)
        {
            if (IsHKeyInvalid(host.HostKey) || IsHostExists(host.HostKey))
                host.HostKey = Configuration.HostKey++;

            DataSource.Hosts.Add(host);
        }

        public void UpdateHost(Host host)
        {
            if (host.HostKey < 10000000)
                throw new DalInvalidKeyException();

            DeleteHost(host);

            AddHost(host);
        }

        public void DeleteHost(Host host)
        {
            if (IsHostExists(host.HostKey))
                DataSource.Hosts.Remove(host);
            else throw new DalKeyNotFoundException();
        }

        private bool IsHostExists(int hostKey)
        {
            return (from item in GetHosts()
                    where item.HostKey == hostKey
                    select item).ToList().Count > 0;
        }

        private bool IsHKeyInvalid(int hostKey)
        {
            return hostKey < 1000000;
        }

        #endregion

        //----------------------------------Guest Methodes----------------------------------//

        #region Guest Methodes

        public List<Guest> GetGuests()
        {
            return DataSource.Guests;
        }

        void Idal.AddGuest(Guest guest)
        {
            DataSource.Guests.Add(guest);
        }

        public void UpdateGuest(Guest newGuest)
        {
            var guest = GetGuests().Find(x => x.Username == newGuest.Username);
            if (guest == null)
                throw new DalKeyNotFoundException();

            DataSource.Guests.Remove(guest);
            DataSource.Guests.Add(newGuest);
        }

        #endregion

        //----------------------------------Admin Methodes----------------------------------//

        #region Admin Methodes

        public string GetAdminPassword()
        {
            return DataSource.AdminPassword;
        }

        #endregion
    }
}

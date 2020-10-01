using BE;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DAL
{
    /// <summary>
    /// Implementaion of Idal. Manages all the data resources.
    /// </summary>
    class Dal_XML_imp : Idal
    {
        Config Config;

        XElement guestRequestRoot;
        string GuestRequestPath = @"..\..\..\Data\GuestRequestXML.xml";

        XElement hostingUnitRoot;
        string HostingUnitPath = @"..\..\..\Data\HostingUnitXML.xml";

        XElement orderRoot;
        string OrderPath = @"..\..\..\Data\OrderXML.xml";

        XElement guestRoot;
        string GuestPath = @"..\..\..\Data\GuestXML.xml";

        XElement hostRoot;
        string HostPath = @"..\..\..\Data\HostXML.xml";

        public Dal_XML_imp()
        {
            Config = new Config();

            CreateOrLoadFiles();
        }

        private void CreateOrLoadFiles()
        {
            if (!File.Exists(GuestRequestPath))
                CreateGuestRequestFile();
            else
                LoadGuestRequest();

            if (!File.Exists(HostingUnitPath))
                CreateHostingUnitFile();
            else
                LoadHostingUnit();

            if (!File.Exists(OrderPath))
                CreateOrderFile();
            else
                LoadOrder();

            if (!File.Exists(HostPath))
                CreateHostFile();
            else
                LoadHost();

            if (!File.Exists(GuestPath))
                CreateGuestFile();
            else
                LoadGuest();
        }

        //----------------------------------GuestRequest Methodes----------------------------------//

        #region GuestRequest Methodes

        public void AddGuestRequest(GuestRequest guestRequest)
        {
            // check invalid key
            if (IsGRKeyInvalid(guestRequest.GuestRequestKey) || IsGuestRequestExists(guestRequest.GuestRequestKey))
                guestRequest.GuestRequestKey = Config.GetGuestRequestKey();

            if (guestRequest.Status == GuestRequestStatus.NotAddedYet)
                guestRequest.Status = GuestRequestStatus.Active;

            List<GuestRequest> GRList = GetGuestRequests();
            GRList.Add(guestRequest);

            FileStream file = new FileStream(GuestRequestPath, FileMode.Create);
            XmlSerializer xmlSerializer = new XmlSerializer(GRList.GetType());
            xmlSerializer.Serialize(file, GRList);
            file.Close();
        }

        public void UpdateGuestRequest(int guestRequestKey, GuestRequestStatus newStatus)
        {
            // check invalid key
            if (IsGRKeyInvalid(guestRequestKey))
                throw new DalInvalidKeyException();
            if (new FileInfo(GuestRequestPath).Length > 0)
                guestRequestRoot = XElement.Load(GuestRequestPath);
            else
                throw new DalKeyNotFoundException();

            XElement GRForUpdate = (from item in guestRequestRoot.Elements()
                                    where int.Parse(item.Element("GuestRequestKey").Value) == guestRequestKey
                                    select item).FirstOrDefault();

            if (GRForUpdate == null)
                throw new DalKeyNotFoundException();

            GRForUpdate.Element("Status").SetValue(newStatus);
            guestRequestRoot.Save(GuestRequestPath);
        }

        public List<GuestRequest> GetGuestRequests()
        {
            List<GuestRequest> GRList;

            FileStream file = new FileStream(GuestRequestPath, FileMode.OpenOrCreate);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<GuestRequest>));
            try
            {
                GRList = (List<GuestRequest>)xmlSerializer.Deserialize(file);
            }
            catch
            {
                GRList = new List<GuestRequest>();
            }
            file.Close();
            return GRList;
        }

        void CreateGuestRequestFile()
        {
            FileStream file = new FileStream(GuestRequestPath, FileMode.Create);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<GuestRequest>));
            xmlSerializer.Serialize(file, new List<GuestRequest>());
            file.Close();
        }

        void LoadGuestRequest()
        {
            try
            {
                guestRequestRoot = XElement.Load(GuestRequestPath);
            }
            catch
            {
                throw new DalFileErrorException();
            }
        }

        private bool IsGuestRequestExists(int guestRequestKey)
        {
            if (!(new FileInfo(GuestRequestPath).Length > 0))
                return false;
            guestRequestRoot = XElement.Load(GuestRequestPath);
            return (from item in guestRequestRoot.Elements()
                    where int.Parse(item.Element("GuestRequestKey").Value) == guestRequestKey
                    select item).ToList().Count() > 0;
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
                hostingUnit.HostingUnitKey = Config.GetHostingUnitKey();

            hostingUnit.Diary = new bool[12, 31];
            InitDiary(hostingUnit.Diary);

            List<HostingUnit> HUList = GetHostingUnits();
            HUList.Add(hostingUnit);

            FileStream file = new FileStream(HostingUnitPath, FileMode.Create);
            XmlSerializer xmlSerializer = new XmlSerializer(HUList.GetType());
            xmlSerializer.Serialize(file, HUList);
            file.Close();
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

            if (!(new FileInfo(HostingUnitPath).Length > 0))
                throw new DalKeyNotFoundException();

            // find the hostingUnit            
            List<HostingUnit> HUList = GetHostingUnits();
            HUList.RemoveAll(item => item.HostingUnitKey == hostingUnitKey);

            FileStream file = new FileStream(HostingUnitPath, FileMode.Create);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<HostingUnit>));
            xmlSerializer.Serialize(file, HUList);
            file.Close();
        }

        public void UpdateHostingUnit(HostingUnit newHostingUnit)
        {
            // check invalid HostingUnitkey.
            if (newHostingUnit.HostingUnitKey < 10000000)
                throw new DalInvalidKeyException();

            List<HostingUnit> HUList = GetHostingUnits();
            foreach (var item in HUList)
                if (item.HostingUnitKey == newHostingUnit.HostingUnitKey)
                {
                    item.DebtToAdmin = newHostingUnit.DebtToAdmin;
                    item.Diary = newHostingUnit.Diary;
                    item.HostingUnitName = newHostingUnit.HostingUnitName;
                    item.VacationProperties = newHostingUnit.VacationProperties;
                }

            FileStream file = new FileStream(HostingUnitPath, FileMode.Create);
            XmlSerializer xmlSerializer = new XmlSerializer(HUList.GetType());
            xmlSerializer.Serialize(file, HUList);
            file.Close();
        }

        public List<HostingUnit> GetHostingUnits()
        {
            List<HostingUnit> HUList;

            FileStream file = new FileStream(HostingUnitPath, FileMode.Open);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<HostingUnit>));
            try
            {
                HUList = (List<HostingUnit>)xmlSerializer.Deserialize(file);
            }
            catch
            {
                HUList = new List<HostingUnit>();
            }
            file.Close();
            return HUList;
        }

        void CreateHostingUnitFile()
        {
            FileStream file = new FileStream(HostingUnitPath, FileMode.Create);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<HostingUnit>));
            xmlSerializer.Serialize(file, new List<HostingUnit>());
            file.Close();
        }

        void LoadHostingUnit()
        {
            try
            {
                hostingUnitRoot = XElement.Load(HostingUnitPath);
            }
            catch
            {
                throw new DalFileErrorException();
            }
        }

        private bool IsHostingUnitExists(int hostingUnitKey)
        {
            if (!(new FileInfo(HostingUnitPath).Length > 0))
                return false;
            hostingUnitRoot = XElement.Load(HostingUnitPath);
            return (from item in hostingUnitRoot.Elements()
                    where int.Parse(item.Element("HostingUnitKey").Value) == hostingUnitKey
                    select item).ToList().Count() > 0;
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
                order.OrderKey = Config.GetOrderKey();

            XElement OrderKey = new XElement("OrderKey", order.OrderKey);
            XElement HostingUnitKey = new XElement("HostingUnitKey", order.HostingUnitKey);
            XElement GuestRequestKey = new XElement("GuestRequestKey", order.GuestRequestKey);
            XElement Status = new XElement("Status", order.Status);
            XElement CreateDate = new XElement("CreateDate", order.CreateDate);
            XElement OrderDate = new XElement("OrderDate", order.OrderDate);

            orderRoot.Add(new XElement("Order", OrderKey, HostingUnitKey, GuestRequestKey, Status, CreateDate, OrderDate));
            orderRoot.Save(OrderPath);
        }

        public void UpdateOrder(int orderKey, OrderStatus newStatus)
        {
            // check invalid orderKey
            if (IsOKeyInvalid(orderKey))
                throw new DalInvalidKeyException();
            // find Order
            if (!(new FileInfo(OrderPath).Length > 0))
                throw new DalKeyNotFoundException();

            orderRoot = XElement.Load(OrderPath);
            XElement OrderForUpdate = (from item in orderRoot.Elements()
                                       where int.Parse(item.Element("OrderKey").Value) == orderKey
                                       select item).FirstOrDefault();

            if (OrderForUpdate is null)
                throw new DalKeyNotFoundException();

            OrderForUpdate.Element("Status").SetValue(newStatus);
            orderRoot.Save(OrderPath);
        }

        public List<Order> GetOrders()
        {
            LoadOrder();
            List<Order> orders = new List<Order>();
            try
            {
                orders = (from item in orderRoot.Elements()
                          select new Order()
                          {
                              OrderKey = int.Parse(item.Element("OrderKey").Value),
                              GuestRequestKey = int.Parse(item.Element("GuestRequestKey").Value),
                              HostingUnitKey = int.Parse(item.Element("HostingUnitKey").Value),
                              Status = (OrderStatus)Enum.Parse(typeof(OrderStatus), item.Element("Status").Value),
                              CreateDate = DateTime.Parse(item.Element("CreateDate").Value),
                              OrderDate = DateTime.Parse(item.Element("OrderDate").Value)
                          }).ToList();
            }
            catch { }

            return orders;
        }

        void CreateOrderFile()
        {
            FileStream file = new FileStream(OrderPath, FileMode.Create);
            //XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Order>));
            //xmlSerializer.Serialize(file, new List<Order>());
            file.Close();
            orderRoot = new XElement("Orders");
            orderRoot.Save(OrderPath);
        }

        void LoadOrder()
        {
            try
            {
                orderRoot = XElement.Load(OrderPath);
            }
            catch
            {
                throw new DalFileErrorException();
            }
        }

        private bool IsOrderKeyExist(int orderKey)
        {
            if (!(new FileInfo(OrderPath).Length > 0))
                return false;
            orderRoot = XElement.Load(OrderPath);
            return (from item in orderRoot.Elements()
                    where int.Parse(item.Element("OrderKey").Value) == orderKey
                    select item).ToList().Count() > 0;

        }

        private static bool IsOKeyInvalid(int orderKey)
        {
            return orderKey < 1000000;
        }

        #endregion

        //----------------------------------BankBranch Methodes----------------------------------//

        #region BankBranch Methodes

        public List<BankBranch> GetBankBranches()
        {
            // create list of bank branches
            List<BankBranch> bankBranches = new List<BankBranch>();

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
            List<Host> HList;

            FileStream file = new FileStream(HostPath, FileMode.Open);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Host>));
            try
            {
                HList = (List<Host>)xmlSerializer.Deserialize(file);
            }
            catch
            {
                HList = new List<Host>();
            }
            file.Close();
            return HList;
        }

        public void AddHost(Host host)
        {
            if (IsHKeyInvalid(host.HostKey) || IsHostExists(host.HostKey))
                host.HostKey = Config.GetHostKey();

            List<Host> HList = GetHosts();
            HList.Add(host);

            try
            {
                FileStream file = new FileStream(HostPath, FileMode.Create);
                XmlSerializer xmlSerializer = new XmlSerializer(HList.GetType());
                xmlSerializer.Serialize(file, HList);
                file.Close();
            }
            catch
            {
                throw new DalFileErrorException();
            }
        }

        public void UpdateHost(Host host)
        {
            if (IsHKeyInvalid(host.HostKey))
                throw new DalInvalidKeyException();

            DeleteHost(host);
            AddHost(host);
        }

        public void DeleteHost(Host host)
        {
            List<Host> HList = GetHosts();
            HList.RemoveAll(item => item.HostKey == host.HostKey);

            FileStream file = new FileStream(HostPath, FileMode.Create);
            XmlSerializer xmlSerializer = new XmlSerializer(HList.GetType());
            xmlSerializer.Serialize(file, HList);
            file.Close();
        }

        void CreateHostFile()
        {
            FileStream file = new FileStream(HostPath, FileMode.Create);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Host>));
            xmlSerializer.Serialize(file, new List<Host>());
            file.Close();
        }

        void LoadHost()
        {
            try
            {
                hostRoot = XElement.Load(HostPath);
            }
            catch
            {
                throw new DalFileErrorException();
            }
        }

        private bool IsHostExists(int hostKey)
        {
            if (!(new FileInfo(HostPath).Length > 0))
                return false;

            hostRoot = XElement.Load(HostPath);
            return (from item in hostRoot.Elements()
                    where int.Parse(item.Element("HostKey").Value) == hostKey
                    select item).ToList().Count > 0;
        }

        private bool IsHKeyInvalid(int hostKey)
        {
            return hostKey < 10000000;
        }

        #endregion

        //----------------------------------Guest Methodes----------------------------------//

        #region Guest Methodes

        public List<Guest> GetGuests()
        {
            List<Guest> GList;

            FileStream file = new FileStream(GuestPath, FileMode.Open);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Guest>));
            try
            {
                GList = (List<Guest>)xmlSerializer.Deserialize(file);
            }
            catch
            {
                GList = new List<Guest>();
            }
            file.Close();
            return GList;
        }

        public void AddGuest(Guest guest)
        {
            List<Guest> GList = GetGuests();
            GList.Add(guest);
            try
            {
                FileStream file = new FileStream(GuestPath, FileMode.Create);
                XmlSerializer xmlSerializer = new XmlSerializer(GList.GetType());
                xmlSerializer.Serialize(file, GList);
                file.Close();
            }
            catch
            {
                throw new DalFileErrorException();
            }
        }

        private void LoadGuest()
        {
            try
            {
                guestRoot = XElement.Load(GuestPath);
            }
            catch
            {
                throw new DalFileErrorException();
            }
        }

        private void CreateGuestFile()
        {
            FileStream file = new FileStream(GuestPath, FileMode.Create);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Guest>));
            xmlSerializer.Serialize(file, new List<Guest>());
            file.Close();
        }

        public void UpdateGuest(Guest guest)
        {
            if (!(new FileInfo(GuestPath).Length > 0))
                throw new DalKeyNotFoundException();

            guestRoot = XElement.Load(GuestPath);
            XElement GuestForUpdate = (from item in guestRoot.Elements()
                                       where item.Element("Username").Value == guest.Username
                                       select item).FirstOrDefault();

            if (GuestForUpdate is null)
                throw new DalKeyNotFoundException();

            GuestForUpdate.Element("Code").SetValue(guest.Code);
            guestRoot.Save(GuestPath);
        }

        #endregion

        //----------------------------------Admin Methodes----------------------------------//

        #region Admin Methodes

        public string GetAdminPassword()
        {
            return Config.GetAdminPassword();
        }

        #endregion
    }
}

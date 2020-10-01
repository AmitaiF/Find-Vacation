using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using BE;
using DAL;
using System.Windows;

namespace BL
{
    public class MyBL : IBL
    {
        Idal DataAccess;

        Random random = new Random();

        public MyBL()
        {
            try
            {
                DataAccess = SingletonFactoryDAL.GetDal();
            }
            catch (DalFileErrorException)
            {
                throw new BlFileErrorException();
            }
        }

        //---------------------------GuestRequest Methodes---------------------------//

        #region GuestRequest Methodes

        public void AddGuestRequest(GuestRequest guestRequest)
        {
            if (guestRequest == null)
                throw new BlArgumentNullException();
            if (guestRequest.EntryDate >= guestRequest.ReleaseDate)
                throw new BlEntryDateException();
            if (guestRequest.EntryDate < DateTime.Today) // vacation in the past
                throw new BlVacationInPastException();
            if (guestRequest.VacationProperties.Children + guestRequest.VacationProperties.Adults < 1) // no vacationeers
                throw new BlNoVacationeersException();
            if (guestRequest.VacationProperties.MaxPrice < 1) // too low price
                throw new BlPriceLowException();

            DataAccess.AddGuestRequest(guestRequest);
        }

        public void UpdateGuestRequest(int guestRequestKey, GuestRequestStatus newStatus)
        {
            try
            {
                DataAccess.UpdateGuestRequest(guestRequestKey, newStatus);
            }
            catch (DalInvalidKeyException)
            {
                throw new BlInvalidKeyException();
            }
            catch (DalKeyNotFoundException)
            {
                throw new BlKeyNotFoundException();
            }
        }

        public List<GuestRequest> GetGuestRequests()
        {
            return DataAccess.GetGuestRequests();
        }

        public List<GuestRequest> GetSpecificGuestRequests(Func<GuestRequest, bool> conditionFunc)
        {
            return (from item in DataAccess.GetGuestRequests()
                    where conditionFunc(item)
                    select item).ToList();
        }

        #endregion

        //---------------------------HostingUnit Methodes---------------------------//

        #region HostingUnit Methodes

        public void AddHostingUnit(HostingUnit hostingUnit)
        {
            if (hostingUnit == null)
                throw new BlArgumentNullException();
            if (hostingUnit.VacationProperties.Adults + hostingUnit.VacationProperties.Children < 1) // no vacationeers
                throw new BlNoVacationeersException();
            if (hostingUnit.VacationProperties.MaxPrice < 1) // too low price
                throw new BlPriceLowException();
            if (hostingUnit.HostingUnitName.Length < 5) // too short name
                throw new BlNameTooShortException();
            if (IsNameAlreadyExist(hostingUnit.HostingUnitName)) // name that already exist
                throw new BlNameAlreadyExistException();

            DataAccess.AddHostingUnit(hostingUnit);
        }

        private bool IsNameAlreadyExist(string hostingUnitName)
        {
            return GetHostingUnits().FindAll(x => x.HostingUnitName == hostingUnitName).Count > 0;
        }

        public void UpdateHostingUnit(HostingUnit newHostingUnit)
        {
            if (newHostingUnit == null)
                throw new BlArgumentNullException();

            try
            {
                DataAccess.UpdateHostingUnit(newHostingUnit);
            }
            catch (DalInvalidKeyException)
            {
                throw new BlInvalidKeyException();
            }
            catch (DalKeyNotFoundException)
            {
                throw new BlKeyNotFoundException();
            }
        }

        public void DeleteHostingUnit(int hostingUnitKey)
        {
            // count all orders that their hosting unit is the one we are trying to delete,
            // and a mail to the guest was sent, or the deal closed.
            int count = (from order in GetOrders()
                         where order.HostingUnitKey == hostingUnitKey
                         where IsMailSent(order) || IsClientRespondedYes(order)
                         select new { order.OrderKey }).ToList().Count;

            // if it's not equal zero, than we can't delete the hosting unit.
            if (count != 0)
                throw new BlHostingUnitHasOpenOrderException();

            try
            {
                DataAccess.DeleteHostingUnit(hostingUnitKey);
            }
            catch (DalInvalidKeyException)
            {
                throw new BlInvalidKeyException();
            }
            catch (DalKeyNotFoundException)
            {
                throw new BlKeyNotFoundException();
            }
        }

        public List<HostingUnit> GetHostingUnits()
        {
            return DataAccess.GetHostingUnits();
        }

        public List<HostingUnit> GetAvailableHostingUnits(DateTime entryDate, int vacationLength)
        {
            return GetHostingUnits().FindAll(hU => IsVacationAvailable(hU, entryDate, vacationLength));
        }

        private static bool IsClientRespondedYes(Order order)
        {
            return order.Status == OrderStatus.ClientRespondedYes;
        }

        private bool IsMailSent(Order order)
        {
            return order.Status == OrderStatus.MailSent;
        }

        private bool IsVacationAvailable(HostingUnit item, DateTime entryDate, int vacationLength)
        {
            // We are check everyday in the requested vacation, if one day is already
            // occupied, we return false, means we can't make a vacation in this dates.

            DateTime currentDate = new DateTime(entryDate.Year, entryDate.Month, entryDate.Day);

            for (int i = 0; i < vacationLength; i++)
            {
                if (item.Diary[currentDate.Month - 1, currentDate.Day - 1])
                    return false;
                currentDate = currentDate.AddDays(1);
            }

            return true;
        }

        #endregion

        //---------------------------Order Methodes---------------------------//

        #region Order Methodes

        public void AddOrder(Order order)
        {
            if (order == null)
                throw new BlArgumentNullException();

            // find the Hosting Unit
            var HostingUnit = (from item in DataAccess.GetHostingUnits()
                               where item.HostingUnitKey == order.HostingUnitKey
                               select item).ToList();
            // the hosting unit doesn't exist => error
            if (HostingUnit.Count == 0)
                throw new BlHostingUnitDoesntExistException();

            // find the Guest Request
            var GuestRequest = (from item in DataAccess.GetGuestRequests()
                                where item.GuestRequestKey == order.GuestRequestKey
                                select item).ToList();
            // the guest request doesn't exist => error
            if (GuestRequest.Count == 0)
                throw new BlGuestRequestDoesntExistException();

            int lengthOfVacation = GetNumOfDays(GuestRequest.First().EntryDate, GuestRequest.First().ReleaseDate);

            if (!IsVacationAvailable(HostingUnit.First(), GuestRequest.First().EntryDate, lengthOfVacation))
                throw new BlVacationDatesAlreadyOccupiedException();

            if (!IsSignedClearance(order))
                throw new BlNotSignedClearanceException();

            order.OrderDate = DateTime.Today;
            DataAccess.AddOrder(order);
            SendMailToGuest(order); // send mail to guest, that he got new order
        }

        public void MailSent(int orderKey)
        {
            // Called after we sent a mail to the guest.
            // Changes the order status to MailSent
            Order order = GetOrders().Find(x => x.OrderKey == orderKey);
            try
            {
                UpdateOrder(orderKey, OrderStatus.MailSent);
            }
            catch (BlInvalidKeyException)
            {
                MessageBox.Show("We couldn't update the order to 'MailSent'. Please try again later.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (BlKeyNotFoundException)
            {
                MessageBox.Show("We couldn't update the order to 'MailSent'. Please try again later.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void UpdateOrder(int orderKey, OrderStatus newStatus)
        {
            // find the order
            var order = (from item in GetOrders()
                         where item.OrderKey == orderKey
                         select item).ToList().First();
            // the order doesn't exist
            if (order == null)
                throw new BlKeyNotFoundException();

            // If the deal was closed, we can't change the status.
            if (order.Status == OrderStatus.ClientRespondedYes)
                throw new BlDealClosedException();

            try
            {
                DataAccess.UpdateOrder(orderKey, newStatus);
            }
            catch (DalInvalidKeyException)
            {
                throw new BlInvalidKeyException();
            }
            catch (DalKeyNotFoundException)
            {
                throw new BlKeyNotFoundException();
            }

            // if client responded yes => deal closed
            if (newStatus == OrderStatus.ClientRespondedYes)
            {
                // find the guest request
                GuestRequest guestRequest = GetGuestRequest(order);
                UpdateGuestRequest(guestRequest.GuestRequestKey, GuestRequestStatus.DealClosed);

                // find the hosting unit
                HostingUnit hostingUnit = GetHostingUnit(order);
                // calculate the money the host earned.
                hostingUnit.Owner.MoneyEarned += hostingUnit.VacationProperties.MaxPrice * GetLengthOfVacation(order);
                UpdateHost(hostingUnit.Owner);

                // occupy the dates of the vacation we approved
                OccupyDates(hostingUnit.Diary, guestRequest.EntryDate, guestRequest.ReleaseDate);
                hostingUnit.DebtToAdmin += Configuration.Commision * GetLengthOfVacation(order);
                UpdateHostingUnit(hostingUnit);
            }
        }

        private void OccupyDates(bool[,] diary, DateTime entryDate, DateTime releaseDate)
        {
            DateTime date = entryDate;
            while (date != releaseDate)
            {
                diary[date.Month - 1, date.Day - 1] = true;
                date = date.AddDays(1);
            }
        }

        public List<Order> GetOrders()
        {
            return DataAccess.GetOrders();
        }

        public List<Order> GetOrdersOfHost(Host host)
        {
            // For every order, we find the host by the hosting unit key (of the order).
            // then, if there is a host, and if it's the host we are looking for,
            // we select the order.
            // The result is that we have list of orders of specific host.
            return (from item in GetOrders()
                    let Host = GetHostByHostingUnitKey(item.HostingUnitKey)
                    where (Host != null) && (Host.HostKey == host.HostKey)
                    select item).ToList();
        }

        public List<Order> GetExpiredOrders(int daysToExpire)
        {
            // For every order, we check if a mail was sent, and it was before the number of days
            // we chose. If true, we select the order.
            var orders = GetOrders();
            if (orders == null)
                return null;
            return (from item in orders
                    where (item.Status == OrderStatus.MailSent) && (GetNumOfDays(item.OrderDate) >= daysToExpire)
                    select item).ToList();
        }

        public int GetNumOfMailSentOrDealClosedOrders(HostingUnit hostingUnit)
        {
            // for every order, if it's hosting unit is the hosting unit we want,
            // if it's status is MailSent or ClientRespondedYes (Deal closed), we select it,
            // and return the count.
            return (from item in GetOrders()
                    where item.HostingUnitKey == hostingUnit.HostingUnitKey
                    where (item.Status == OrderStatus.MailSent) || (item.Status == OrderStatus.ClientRespondedYes)
                    select item).ToList().Count;
        }

        public int GetNumOfOrders(GuestRequest guestRequest)
        {
            return (from item in GetOrders()
                    where item.GuestRequestKey == guestRequest.GuestRequestKey
                    select item).ToList().Count;
        }

        private void SendMailToGuest(Order order)
        {
            // Find the request.
            var guestRequest = GetSpecificGuestRequests(x => x.GuestRequestKey == order.GuestRequestKey).First();

            // Construct the mail message.
            MailMessage mail = new MailMessage();
            mail.To.Add(guestRequest.Guest.MailAddress);
            mail.From = new MailAddress("noreply.VacationSystem@gmail.com");
            mail.Subject = "You got a new order!";
            mail.Body = "<div style = 'direction:ltr'><br><br><h3>Hello ";
            mail.Body += guestRequest.Guest.FirstName + "!<br><br>Your request got a new order from a host.<br><br>";
            mail.Body += "You can check it out on the website.<br><br><br>Regards,<br><br><em>Nathan&amp;Amitai Vacation System.</em></h3>";
            mail.IsBodyHtml = true;

            // send the message
            MailSender sender = new MailSender();
            sender.SendMail(mail, order.OrderKey);
        }

        private bool IsSignedClearance(Order order)
        {
            return GetHostingUnit(order).Owner.CollectionClearance;
        }

        private GuestRequest GetGuestRequest(Order order)
        {
            return GetGuestRequests().Find(item => item.GuestRequestKey == order.GuestRequestKey);
        }

        private HostingUnit GetHostingUnit(Order order)
        {
            return GetHostingUnits().Find(item => item.HostingUnitKey == order.HostingUnitKey);
        }

        private int GetLengthOfVacation(Order order)
        {
            // For every request, if it's the request of the order,
            // we select the Entry date and the Release Date.
            // Then, we find the length of the vacation.
            var vacation = (from item in GetGuestRequests()
                            where item.GuestRequestKey == order.GuestRequestKey
                            select new { eD = item.EntryDate, rD = item.ReleaseDate }).ToList().First();

            return vacation.rD.Subtract(vacation.eD).Days;
        }

        public void ActivateExpiredOrdersThread()
        {
            new Thread(() => DeleteExpiredOrders()).Start();
        }

        private void DeleteExpiredOrders()
        {
            while (true)
            {
                // Find the orders we should update.
                List<Order> expiredOrders = GetExpiredOrders(Configuration.DaysToExpire);
                if (expiredOrders != null)
                {
                    // Update every order.
                    foreach (Order order in expiredOrders)
                        UpdateOrder(order.OrderKey, OrderStatus.ClientNotResponded);
                    // Clear the day before a month in the diary of all hosting units.
                    List<HostingUnit> HostingUnits = GetHostingUnits();
                    DateTime lastMonthDate = DateTime.Today.AddDays(-31);
                    foreach (HostingUnit hostingUnit in HostingUnits)
                    {
                        hostingUnit.Diary[lastMonthDate.Month - 1, lastMonthDate.Day - 1] = false;
                        UpdateHostingUnit(hostingUnit);
                    }
                }
                Thread.Sleep(1000 * 60 * 60 * 24); // wait a day, milliseconds*seconds*minutes*hours
            }
        }

        #endregion

        //---------------------------BankBranch Methodes---------------------------//

        #region BankBranch Methodes

        public List<BankBranch> GetBankBranches()
        {
            return DataAccess.GetBankBranches();
        }

        #endregion

        //---------------------------Grouping Methodes---------------------------//

        #region Grouping Methodes

        public List<IGrouping<Area, GuestRequest>> GetGuestRequestsByArea()
        {
            return (from item in GetGuestRequests()
                    group item by item.VacationProperties.Area).ToList();
        }

        public List<IGrouping<GuestRequestStatus, GuestRequest>> GetGuestRequestsByStatus()
        {
            return (from item in GetGuestRequests()
                    group item by item.Status).ToList();
        }

        public List<IGrouping<DateTime, GuestRequest>> GetGuestRequestsByEntryDate()
        {
            return (from item in GetGuestRequests()
                    group item by item.EntryDate).ToList();
        }

        public List<IGrouping<DateTime, GuestRequest>> GetGuestRequestsByReleaseDate()
        {
            return (from item in GetGuestRequests()
                    group item by item.ReleaseDate).ToList();
        }

        public List<IGrouping<int, GuestRequest>> GetGuestRequestsByNumOfVacationers()
        {
            return (from item in GetGuestRequests()
                    group item by (item.VacationProperties.Adults + item.VacationProperties.Children) into l
                    orderby l.Key
                    select l).ToList();
        }

        public List<IGrouping<VacationType, GuestRequest>> GetGuestRequestByType()
        {
            return (from item in GetGuestRequests()
                    group item by item.VacationProperties.Type).ToList();
        }

        public List<IGrouping<Area, HostingUnit>> GetHostingUnitsByArea()
        {
            return (from item in GetHostingUnits()
                    group item by item.VacationProperties.Area).ToList();
        }

        public List<IGrouping<int, Host>> GetHostsByNumOfHostingUnits()
        {
            var HUs = (from item in GetHostingUnits()
                       group item by item.Owner).ToList();

            var Hosts = (from item in HUs
                         group item.Key by item.Count()).ToList();

            return Hosts;
        }

        public List<IGrouping<UserType, User>> GetUsersByType()
        {
            return (from item in GetUsers()
                    group item by item.Type).ToList();
        }

        public List<IGrouping<string, User>> GetUsersByFirstName()
        {
            return (from item in GetUsers()
                    group item by item.FirstName).ToList();
        }

        public List<IGrouping<string, User>> GetUsersByLastName()
        {
            return (from item in GetUsers()
                    group item by item.LastName).ToList();
        }

        public List<IGrouping<DateTime, User>> GetUsersByRegistrationDate()
        {
            return (from item in GetUsers()
                    group item by item.RegistrationDate).ToList();
        }

        public List<IGrouping<bool, User>> GetUsersByFinishedRegistration()
        {
            return (from item in GetUsers()
                    group item by item.FinishedRegistration).ToList();
        }

        public List<IGrouping<int, Order>> GetOrdersByHostingUnitKey()
        {
            return (from item in GetOrders()
                    group item by item.HostingUnitKey).ToList();
        }

        public List<IGrouping<int, Order>> GetOrdersByGuestRequestKey()
        {
            return (from item in GetOrders()
                    group item by item.GuestRequestKey).ToList();
        }

        public List<IGrouping<OrderStatus, Order>> GetOrdersByStatus()
        {
            return (from item in GetOrders()
                    group item by item.Status).ToList();
        }

        #endregion

        //---------------------------Date Methodes---------------------------//

        #region Date Methodes

        public int GetNumOfDays(params DateTime[] dates)
        {
            // If we got 2 dates, find the length between them.
            // If we got 1 date, find the length between the date and today.
            if (dates.Length == 2)
                return dates[1].Subtract(dates[0]).Days;

            return DateTime.Today.Subtract(dates[0]).Days;
        }

        #endregion

        //---------------------------User Methodes---------------------------//

        #region User Methodes

        public List<User> GetUsers()
        {
            // We get list of users by adding the list of the guests and the list of the hosts.

            List<User> users = new List<User>();
            var guests = GetGuests();
            if (guests != null)
                foreach (var item in guests)
                    users.Add(item);

            var hosts = GetHosts();
            if (hosts != null)
                foreach (var item in hosts)
                    users.Add(item);

            return users;
        }

        private bool IsPasswordValid(string password)
        {
            return IsUsernameVaild(password);
        }

        private bool IsUsernameVaild(string username)
        {
            if (username.Length < 5)
                return false;
            if (HasInvalidChar(username))
                return false;
            return true;
        }

        private bool HasInvalidChar(string username)
        {
            Regex reg = new Regex(@"^[a-zA-Z0-9_]+$");
            return !reg.IsMatch(username);
        }

        private bool IsValidEmail(string mailAddress)
        {
            var regex = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
            return Regex.IsMatch(mailAddress, regex, RegexOptions.IgnoreCase);
        }

        private bool IsMailExist(string mailAddress)
        {
            var mail = (from item in GetUsers()
                        where item.MailAddress == mailAddress
                        select item).ToList();
            if (mail.Count > 0)
                return true;

            return false;
        }

        private bool IsUserExist(string username)
        {
            var nick = (from item in GetUsers()
                        where item.Username == username
                        select item).ToList();
            if (nick.Count > 0)
                return true;

            return false;
        }

        public User GetUser(string username)
        {
            User user = GetUsers().Find(item => item.Username == username);
            if (user == null)
                throw new BlUserDoesNotExistException();
            return user;
        }

        public void SendCodeMail(User user)
        {
            int code = random.Next(10000, 99999);

            if (user.Type == UserType.Host)
            {
                Host host = GetSpecificHosts(x => x.Username == user.Username).First();
                host.Code = code;
                UpdateHost(host);
            }
            else
            {
                Guest guest = GetGuests().Find(x => x.Username == user.Username);
                guest.Code = code;
                UpdateGuest(guest);
            }

            MailMessage mail = new MailMessage();
            mail.To.Add(user.MailAddress);
            mail.From = new MailAddress("noreply.VacationSystem@gmail.com");
            mail.Subject = "Confirmation code";
            mail.Body = "<div style = 'direction:ltr'><br><br><h3>Hello ";
            mail.Body += user.FirstName + "!<br><br>Your code is: " + code + ".<br><br>";
            mail.Body += "The code will expire soon.<br><br><br>Regards,<br><br><em>Nathan&amp;Amitai Vacation System.</em></h3>";
            mail.IsBodyHtml = true;

            MailSender sender = new MailSender();
            sender.SendMail(mail, 0);

            new Thread(() => ClearCode(user)).Start();
        }

        private void ClearCode(User user)
        {
            Thread.Sleep(1000 * 60 * 2); // wait a 2 minutes, milliseconds*seconds*minutes

            if (user.Type == UserType.Host)
            {
                Host host = GetSpecificHosts(x => x.Username == user.Username).First();
                host.Code = 0;
                UpdateHost(host);
            }
            else
            {
                Guest guest = GetGuests().Find(x => x.Username == user.Username);
                guest.Code = 0;
                UpdateGuest(guest);
            }
        }

        #endregion

        //----------------------------------Host Methodes----------------------------------//

        #region Host Methodes

        public List<Host> GetHosts()
        {
            return DataAccess.GetHosts();
        }

        public void AddHost(Host host)
        {
            if (host == null)
                throw new BlArgumentNullException();
            if (IsUserExist(host.Username))
                throw new BlNickAlreadyExistException();
            if (IsMailExist(host.MailAddress))
                throw new BlMailAlreadyExistException();
            if (!IsUsernameVaild(host.Username))
                throw new BlUsernameInvalidException();
            if (!IsPasswordValid(host.Password))
                throw new BlPasswordInvalidException();
            if (!IsValidEmail(host.MailAddress))
                throw new BlInvalidEmailException();

            try
            {
                DataAccess.AddHost(host);
            }
            catch (DalFileErrorException)
            {
                throw new BlFileErrorException();
            }
        }

        private bool IsInvaildPhoneNumber(string phoneNumber)
        {
            if (phoneNumber.Length != 10)
                return true;
            if (phoneNumber[0] != '0' || phoneNumber[1] != '5')
                return true;
            if (!IsDigitsOnly(phoneNumber))
                return true;
            return false;
        }

        private bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
                if (c < '0' || c > '9')
                    return false;
            return true;
        }

        public void UpdateHost(Host newHost)
        {
            if (newHost == null)
                throw new BlArgumentNullException();
            Host host;
            var hosts = GetHosts();
            if (hosts != null)
                host = hosts.Find(x => x.HostKey == newHost.HostKey);
            else
                throw new BlKeyNotFoundException();

            if (newHost.CollectionClearance == false && host.CollectionClearance == true)
                if (GetOrdersOfHost(host).Find(x => x.Status == OrderStatus.MailSent) != null)
                    throw new BlOpenOrderException();

            if (IsInvaildPhoneNumber(host.PhoneNumber))
                throw new BlInvalidPhoneNumberException();

            try
            {
                DataAccess.UpdateHost(newHost);
            }
            catch (DalInvalidKeyException)
            {
                throw new BlInvalidKeyException();
            }
            catch (DalKeyNotFoundException)
            {
                throw new BlKeyNotFoundException();
            }
            catch (DalFileErrorException)
            {
                throw new BlFileErrorException();
            }
        }

        public List<HostingUnit> GetHostingUnitsOfHost(Host host)
        {
            return (from item in GetHostingUnits()
                    where item.Owner.HostKey == host.HostKey
                    select item).ToList();
        }

        public List<GuestRequest> GetSuitableRequests(HostingUnit hostingUnit)
        {
            return GetSpecificGuestRequests(item => IsRequestSuitable(hostingUnit, item));
        }

        private bool IsRequestSuitable(HostingUnit hostingUnit, GuestRequest item)
        {
            if (item.Status != GuestRequestStatus.Active)
                return false;
            if (!IsVacationAvailable(hostingUnit, item.EntryDate, Math.Abs(item.EntryDate.Subtract(item.ReleaseDate).Days)))
                return false;
            if (!IsPropertiesFits(hostingUnit.VacationProperties, item.VacationProperties))
                return false;
            return true;
        }

        private bool IsPropertiesFits(VacationProperties HUvP, VacationProperties GRvP)
        {
            if (GRvP.Adults > HUvP.Adults)
                return false;
            if (GRvP.Area != Area.All)
                if (GRvP.Area != HUvP.Area)
                    return false;
            if (GRvP.BBQ == Extension.Necessary && HUvP.BBQ == Extension.Unintrested)
                return false;
            if (GRvP.ChildernAttractions == Extension.Necessary && HUvP.ChildernAttractions == Extension.Unintrested)
                return false;
            if (GRvP.Garden == Extension.Necessary && HUvP.Garden == Extension.Unintrested)
                return false;
            if (GRvP.Jacuzzi == Extension.Necessary && HUvP.Jacuzzi == Extension.Unintrested)
                return false;
            if (GRvP.NearbyKosherFood == Extension.Necessary && HUvP.NearbyKosherFood == Extension.Unintrested)
                return false;
            if (GRvP.NearbyRestaurant == Extension.Necessary && HUvP.NearbyRestaurant == Extension.Unintrested)
                return false;
            if (GRvP.NearbySynagogue == Extension.Necessary && HUvP.NearbySynagogue == Extension.Unintrested)
                return false;
            if (GRvP.Pool == Extension.Necessary && HUvP.Pool == Extension.Unintrested)
                return false;
            if (GRvP.Children > HUvP.Children)
                return false;
            if (GRvP.MaxPrice < HUvP.MaxPrice)
                return false;
            if (GRvP.Type != VacationType.Undefined)
                if (GRvP.Type != HUvP.Type)
                    return false;
            return true;
        }

        private Host GetHostByHostingUnitKey(int hostingUnitKey)
        {
            var hostingUnits = GetHostingUnits();
            if (hostingUnits.Count == 0)
                return null;
            return (from item in hostingUnits
                    where item.HostingUnitKey == hostingUnitKey
                    select item).ToList().First().Owner;
        }

        public List<Host> GetSpecificHosts(Func<Host, bool> conditionFunc)
        {
            return (from item in DataAccess.GetHosts()
                    where conditionFunc(item)
                    select item).ToList();
        }

        #endregion

        //----------------------------------Guest Methodes----------------------------------//

        #region Guest Methodes

        public List<Guest> GetGuests()
        {
            return DataAccess.GetGuests();
        }

        public void AddGuest(Guest guest)
        {
            if (guest == null)
                throw new BlArgumentNullException();
            if (IsUserExist(guest.Username))
                throw new BlNickAlreadyExistException();
            if (IsMailExist(guest.MailAddress))
                throw new BlMailAlreadyExistException();
            if (!IsUsernameVaild(guest.Username))
                throw new BlUsernameInvalidException();
            if (!IsPasswordValid(guest.Password))
                throw new BlPasswordInvalidException();
            if (!IsValidEmail(guest.MailAddress))
                throw new BlInvalidEmailException();

            try
            {
                DataAccess.AddGuest(guest);
            }
            catch (DalFileErrorException)
            {
                throw new BlFileErrorException();
            }
        }

        public string GetAdminPassword()
        {
            return DataAccess.GetAdminPassword();
        }

        private void UpdateGuest(Guest guest)
        {
            try
            {
                DataAccess.UpdateGuest(guest);
            }
            catch (DalKeyNotFoundException)
            {
                throw new BlKeyNotFoundException();
            }
            catch (DalFileErrorException)
            {
                throw new BlFileErrorException();
            }
        }

        #endregion

    }
}

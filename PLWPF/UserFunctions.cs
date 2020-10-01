using BE;
using BL;
using System.Linq;

namespace PLWPF
{
    public static class UserFunctions
    {

        public static User GetUser(string user)
        {
            return SingletonFactoryBL.GetBL().GetUsers().Find(item => item.Username == user);
        }
        public static bool IsUserExist(string username, string password)
        {
            var user = (from item in SingletonFactoryBL.GetBL().GetUsers()
                        where item.Username == username && item.Password == password
                        select item).ToList();

            if (user.Count > 0)
                return true;

            return false;
        }
        public static Host GetHost(int Id)
        {
            return SingletonFactoryBL.GetBL().GetHosts().Find(item => item.HostKey ==Id );

        }
    }
}

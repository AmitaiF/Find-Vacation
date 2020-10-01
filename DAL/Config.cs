using System.IO;
using System.Xml.Linq;

namespace DAL
{
    /// <summary>
    /// Class for save and load configuration settings.
    /// </summary>
    class Config
    {
        XElement ConfigRoot;
        string ConfigPath = @"..\..\..\Data\ConfigXML.xml";

        public Config()
        {
            if (!File.Exists(ConfigPath))
                CreateFile();
            else
                LoadFile();
        }

        private void LoadFile()
        {
            try
            {
                ConfigRoot = XElement.Load(ConfigPath);
            }
            catch
            {
                throw new DalFileErrorException();
            }
        }

        private void CreateFile()
        {
            FileStream ConfigFile = new FileStream(ConfigPath, FileMode.Create);
            ConfigFile.Close();

            ConfigRoot = new XElement("Config");
            ConfigRoot.Add(new XElement("GuestRequestKey", 10000000),
                           new XElement("HostingUnitKey", 10000000),
                           new XElement("HostKey", 10000000),
                           new XElement("OrderKey", 10000000),
                           new XElement("Commision", 10),
                           new XElement("DaysToExpire", 31),
                           new XElement("AdminPassword", "123456"));
            ConfigRoot.Save(ConfigPath);
        }

        public int GetGuestRequestKey()
        {
            return GetAndUpdateKey("GuestRequestKey");
        }

        public int GetHostingUnitKey()
        {
            return GetAndUpdateKey("HostingUnitKey");
        }

        public int GetOrderKey()
        {
            return GetAndUpdateKey("OrderKey");
        }

        public int GetHostKey()
        {
            return GetAndUpdateKey("HostKey");
        }

        public int GetCommision()
        {
            return GetValue("Commision");
        }

        public int GetDaysToExpire()
        {
            return GetValue("DaysToExpire");
        }

        public string GetAdminPassword()
        {
            return GetValue("AdminPassword").ToString();
        }

        public int GetValue(string atribute)
        {
            //ConfigRoot = XElement.Load(ConfigPath);
            return int.Parse(ConfigRoot.Element(atribute).Value);
        }

        public int GetAndUpdateKey(string atribute)
        {
            ConfigRoot = XElement.Load(ConfigPath);
            int key = int.Parse(ConfigRoot.Element(atribute).Value);
            ConfigRoot.Element(atribute).SetValue(key + 1);
            ConfigRoot.Save(ConfigPath);
            return key;
        }
    }
}
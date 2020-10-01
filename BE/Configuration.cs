using System;
using System.Collections.Generic;
using System.Text;

namespace BE
{
    // 
    /// <summary>
    /// Configurations for BE classes. static fields. 
    /// </summary>
    public class Configuration
    {
        //----------------------------------Running Code Config----------------------------------//

        public static int GuestRequestKey { get; set; } = 10000000;
        public static int HostKey { get; set; } = 10000000;
        public static int HostingUnitKey { get; set; } = 10000000;
        public static int OrderKey { get; set; } = 10000000;

        //----------------------------------Const Config----------------------------------//

        public static int Commision { get; set; } = 10; // commision for a deal
        public static int DaysToExpire { get; set; } = 31; // number of days for an order to expire
    }
}

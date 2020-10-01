using System;
using System.Collections.Generic;
using System.Text;

namespace BE
{
    [Serializable]
    public class Order
    {
        //----------------------------------Order Info----------------------------------//
        public int OrderKey { get; set; } = 0;// running ID code
        public int HostingUnitKey { get; set; } // hosting unit key of the unit offered
        public int GuestRequestKey { get; set; } // guest request key of the intended request
        public OrderStatus Status { get; set; } = OrderStatus.NotHandledYet; // order's status
        public DateTime CreateDate { get; set; } // order's creation date
        public DateTime OrderDate { get; set; } // mail to client date


        //----------------------------------Order Methodes----------------------------------//
        public override string ToString()
        {
            string InfoToPrint = "";

            InfoToPrint += "Order ID: " + OrderKey + "\n";
            InfoToPrint += "Order status: " + Status + "\n";
            InfoToPrint += "Order's hosting unit ID: " + HostingUnitKey + "\n";
            InfoToPrint += "Order's guest request ID: " + GuestRequestKey + "\n";
            InfoToPrint += "Creation date: " + CreateDate + "\n";
            InfoToPrint += "Order date: " + OrderDate + "\n";

            return InfoToPrint;
        }



    }
}

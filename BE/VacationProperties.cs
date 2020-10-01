using System;

namespace BE
{
    [Serializable]
    public class VacationProperties
    {
        public Area Area { get; set; } // area for the vacation
        public VacationType Type { get; set; } // type of vacation
        public int Adults { get; set; } // number of adults on the vacation
        public int Children { get; set; } // number of children on the vacation
        public Extension Pool { get; set; } // pool extension
        public Extension Jacuzzi { get; set; } // jacuzzi extension
        public Extension Garden { get; set; } // garden extension
        public Extension ChildernAttractions { get; set; } // attractions for children extension
        public Extension NearbyRestaurant { get; set; } // nearby restaurant extension
        public Extension NearbySynagogue { get; set; } // nearby synagogue extension
        public Extension BBQ { get; set; } // BBQ in the hosting unit extension
        public Extension NearbyKosherFood { get; set; } // nearby kosher food extension
        public double MaxPrice { get; set; }

        public override string ToString()
        {
            string infoToPrint = "";
            infoToPrint += Type + " in " + Area + ", for " + Adults + " adults and " + Children + " children.";
            return infoToPrint;
        }
    }
}

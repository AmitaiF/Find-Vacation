using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace BE
{
    [Serializable]
    public class HostingUnit
    {
        //----------------------------------Hosting unit Info----------------------------------//
        public int HostingUnitKey { get; set; } = 0; // running ID code
        public Host Owner { get; set; } // the owner of the unit
        public string HostingUnitName { get; set; } // unit's name
        [XmlIgnore]
        public bool[,] Diary { get; set; } // represents capacity for a year
        public string DiaryForXML
        {
            get
            {
                if (Diary == null)
                    return null;

                string result = "";
                int months = Diary.GetLength(0);
                int days = Diary.GetLength(1);
                result += "" + months + "," + days;

                for (int i = 0; i < months; ++i)
                    for (int j = 0; j < days; ++j)
                        result += "," + Diary[i, j];

                return result;
            }

            set
            {
                if (value != null && value.Length > 0)
                {
                    string[] values = value.Split(',');

                    int months = int.Parse(values[0]);
                    int days = int.Parse(values[1]);
                    Diary = new bool[months, days];

                    int index = 2;

                    for (int i = 0; i < months; ++i)
                        for (int j = 0; j < days; ++j)
                            Diary[i, j] = bool.Parse(values[index++]);
                }
            }
        } // get and set for making Diary fit for XML
        public VacationProperties VacationProperties { get; set; } // vacation properties (Jacuzzi, garder, kosher food, etc...)
        public int DebtToAdmin { get; set; }  // days*commission

        //----------------------------------Hosting unit Methodes----------------------------------//
        public override string ToString()
        {
            string InfoToPrint = "";

            InfoToPrint += "Hosting unit name: " + HostingUnitName + "\n";
            InfoToPrint += "Hosting unit ID: " + HostingUnitKey + "\n";
            InfoToPrint += "Owner: " + Owner.FirstName + " " + Owner.LastName + "\n";

            return InfoToPrint;

        }

    }
}


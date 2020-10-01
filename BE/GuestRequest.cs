using BE;
using System;
using System.Collections.Generic;
using System.Text;

namespace BE
{
    /// <summary>
    /// Class that represents a vacation request from a guest.
    /// </summary>
    [Serializable]
    public class GuestRequest
    {

        //----------------------------------Request Info----------------------------------//
        public int GuestRequestKey { get; set; } = 0; // uniqe running code with 8 digits
        public GuestRequestStatus Status { get; set; } // status of the request (active, deal closed, expired)
        public DateTime RegistrationDate { get; set; } // registration date of the guest
        public DateTime EntryDate { get; set; } // beginning date of the vacation
        public DateTime ReleaseDate { get; set; } // ending date of the vacation
        public VacationProperties VacationProperties { get; set; } // vacation properties (Jacuzzi, garder, kosher food, etc...)
        public Guest Guest { get; set; }

        //----------------------------------GuestRequest   Methodes----------------------------------//

        public override string ToString()
        {
            string infoToPrint = "";

            infoToPrint += "Request key: " + GuestRequestKey + "\n";
            infoToPrint += "Request status: " + Status + "\n";
            infoToPrint += "Guest name: " + Guest.LastName + " " + Guest.FirstName + "\n";
            infoToPrint += "Guest mail: " + Guest.MailAddress + "\n";
            infoToPrint += "Vacation dates: from " + EntryDate.ToString("dd/MM/yyyy") + " to " + ReleaseDate.ToString("dd/MM/yyyy") + "\n";
            infoToPrint += "Vacation properties: " + VacationProperties.ToString();

            return infoToPrint;
        }

    }

}




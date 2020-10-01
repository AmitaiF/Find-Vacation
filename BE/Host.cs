using System;
using System.Collections.Generic;
using System.Text;

namespace BE
{
    /// <summary>
    /// Class that represents host that offers you an hosting unit.
    /// </summary>
    [Serializable]
    public class Host : User
    {
        //----------------------------------Host Info----------------------------------//
        public int HostKey { get; set; } // host's ID
        public string PhoneNumber { get; set; } // host's phone number
        public BankBranch BankBranchDetails { get; set; } // host's bank details
        public int BankAccountNumber { get; set; } // host's bank account number
        public bool CollectionClearance { get; set; } // is there an authorization to collect money from host's bank account
        public double MoneyEarned { get; set; } = 0;

        //----------------------------------Host Methodes----------------------------------//
        public override string ToString()
        {
            string infoToPrint = "";

            infoToPrint += "Host name: " + FirstName + " " + LastName + "(" + HostKey + ")" + "\n";
            infoToPrint += "Phone number: " + PhoneNumber + "\n";
            infoToPrint += "Mail address: " + MailAddress + "\n";
            infoToPrint += "Bank account details: " + BankBranchDetails + "\n";
            infoToPrint += "Bank account number: " + BankAccountNumber + "\n";
            infoToPrint += "Collection clearance: " + CollectionClearance + "\n";

            return infoToPrint;
        }
    }
}

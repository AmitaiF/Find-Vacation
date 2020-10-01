using System;
using System.Collections.Generic;
using System.Text;

namespace BE
{
    /// <summary>
    /// Class that represents bank branch for the host.
    /// </summary>
    [Serializable]
    public class BankBranch
    {

        //----------------------------------Bank Info----------------------------------//
        public int BankNumber { get; set; } // bank number for account
        public string BankName { get; set; } // bank number for account
        public int BranchNumber { get; set; } // branch number for account
        public string BranchAddress { get; set; } // branch address for account
        public string BranchCity { get; set; } // branch city for account

        //----------------------------------BankBranch Methodes----------------------------------//

        public override string ToString()
        {
            return BankName + " (" + BankNumber + "). " + BranchAddress + ", " + BranchCity + " (" + BranchNumber + ").";
        }

    }
}

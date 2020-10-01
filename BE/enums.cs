using System;
using System.Collections.Generic;
using System.Text;

//-------------------------------------------enums for BE namespace-------------------------------------------//

namespace BE
{
    // status for the request
    [Serializable]
    public enum GuestRequestStatus
    {
        NotAddedYet,
        Active,
        DealClosed,
        Expired
    }

    // status for the order
    [Serializable]
    public enum OrderStatus
    {
        NotHandledYet, // default
        MailSent, // host sent mail to guest
        ClientNotResponded, // expired
        ClientRespondedYes, // closed deal
        ClientRespondedNo // not closed deal
    }

    // area for the vacation
    [Serializable]
    public enum Area
    {
        All,
        North,
        South,
        Center,
        Jerusalem
    }


    // type for vacation
    [Serializable]
    public enum VacationType
    {
        Undefined,
        Zimmer,
        Hotel,
        Camping,
        Appartment,
        Vila,
        Caravan,
        Yacht
    }

    // extension for vacation
    [Serializable]
    public enum Extension
    {
        Unintrested,
        Possible,
        Necessary
    }

    // type of user.
    [Serializable]
    public enum UserType
    {
        Guest,
        Host
    }
}

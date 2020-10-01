using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// File with all BL exceptions.
/// </summary>
namespace BL
{
    /// <summary>
    /// Throw when an argument in function is null.
    /// </summary>
    public class BlArgumentNullException : Exception
    {
        public BlArgumentNullException() { }
    }

    /// <summary>
    /// Throw when entry date of the vacation isn't smaller than release date.
    /// </summary>
    public class BlEntryDateException : Exception
    {
        public BlEntryDateException() { }
    }

    /// <summary>
    /// Throw when key of object (e.g. GuestRequest, Order, etc...) is invalid (smaller than 10000000)
    /// </summary>
    public class BlInvalidKeyException : Exception
    {
        public BlInvalidKeyException() { }
    }

    /// <summary>
    /// Throw when the key of the new object we add is already taken and exists in the data source.
    /// </summary>
    public class BlKeyAlreadyExistsException : Exception
    {
        public BlKeyAlreadyExistsException() { }
    }

    /// <summary>
    /// Throw when the key of the object we are looking for doesn't exist.
    /// </summary>
    public class BlKeyNotFoundException : Exception
    {
        public BlKeyNotFoundException() { }
    }

    /// <summary>
    /// Throw when we try to erase HostingUnit which has open order (mail sent or deal closed).
    /// </summary>
    public class BlHostingUnitHasOpenOrderException : Exception
    {
        public BlHostingUnitHasOpenOrderException() { }
    }

    /// <summary>
    /// Throw when we try to update order which alreay closed a deal.
    /// </summary>
    public class BlDealClosedException : Exception
    {
        public BlDealClosedException() { }
    }

    /// <summary>
    /// Throw when we try to take a vacation in dates that alreay occupied.
    /// </summary>
    public class BlVacationDatesAlreadyOccupiedException : Exception
    {
        public BlVacationDatesAlreadyOccupiedException() { }
    }

    /// <summary>
    /// Throw when the host didn't sign the collection clearance.
    /// </summary>
    public class BlNotSignedClearanceException : Exception
    {
        public BlNotSignedClearanceException() { }
    }

    /// <summary>
    /// Throw when email is invalid.
    /// </summary>
    public class BlInvalidEmailException : Exception
    {
        public BlInvalidEmailException() { }
    }

    /// <summary>
    /// Throw when new user's mail already exist in the system.
    /// </summary>
    public class BlMailAlreadyExistException : Exception
    {
        public BlMailAlreadyExistException() { }
    }

    /// <summary>
    /// Throw when new user's nick already exist in the system.
    /// </summary>
    public class BlNickAlreadyExistException : Exception
    {
        public BlNickAlreadyExistException() { }
    }

    /// <summary>
    /// Throw when new user's nick invalid.
    /// </summary>
    public class BlUsernameInvalidException : Exception
    {
        public BlUsernameInvalidException() { }
    }

    /// <summary>
    /// Throw when new user's password invalid.
    /// </summary>
    public class BlPasswordInvalidException : Exception
    {
        public BlPasswordInvalidException() { }
    }

    /// <summary>
    /// Throw when a user we searched doesn't exist.
    /// </summary>
    public class BlUserDoesNotExistException : Exception
    {
        public BlUserDoesNotExistException() { }
    }

    /// <summary>
    /// Throws when there is open order while we are trying to delete hosting unit.
    /// </summary>
    public class BlOpenOrderException : Exception
    {
        public BlOpenOrderException() { }
    }

    /// <summary>
    /// Throws when we are looking for hosting unit which doesn't exist.
    /// </summary>
    public class BlHostingUnitDoesntExistException : Exception
    {
        public BlHostingUnitDoesntExistException() { }
    }

    /// <summary>
    /// Throws when we are looking for guest request which doesn't exist.
    /// </summary>
    public class BlGuestRequestDoesntExistException : Exception
    {
        public BlGuestRequestDoesntExistException() { }
    }

    /// <summary>
    /// Throws when we are trying to add guest request with entry date in the past.
    /// </summary>
    public class BlVacationInPastException : Exception
    {
        public BlVacationInPastException() { }
    }

    /// <summary>
    /// Throws when we are trying to add guest request with no vacationeers.
    /// </summary>
    public class BlNoVacationeersException : Exception
    {
        public BlNoVacationeersException() { }
    }

    /// <summary>
    /// Throws when we are trying to add guest request with price lower than 1.
    /// </summary>
    public class BlPriceLowException : Exception
    {
        public BlPriceLowException() { }
    }

    /// <summary>
    /// Throws when we are trying to add object with too short name.
    /// </summary>
    public class BlNameTooShortException : Exception
    {
        public BlNameTooShortException() { }
    }

    /// <summary>
    /// Throws when we are trying to add object with already exist name.
    /// </summary>
    public class BlNameAlreadyExistException : Exception
    {
        public BlNameAlreadyExistException() { }
    }

    /// <summary>
    /// Throws when we are trying to add host with invalid phone number.
    /// </summary>
    public class BlInvalidPhoneNumberException : Exception
    {
        public BlInvalidPhoneNumberException() { }
    }

    /// <summary>
    /// Throws when there is file error.
    /// </summary>
    public class BlFileErrorException : Exception
    {
        public BlFileErrorException() { }
    }
}
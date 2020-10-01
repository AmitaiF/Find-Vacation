using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Classes for exceptions in DAL.
/// </summary>
namespace DAL
{
    /// <summary>
    /// Throw when we got invalid key in a function.
    /// </summary>
    public class DalInvalidKeyException : Exception
    {
        public DalInvalidKeyException() { }
    }

    /// <summary>
    /// Throw when we trying to add object that already exist.
    /// </summary>
    public class DalKeyAlreadyExistsException : Exception
    {
        public DalKeyAlreadyExistsException() { }
    }

    /// <summary>
    /// Throw when we dindn't find a key we got.
    /// </summary>
    public class DalKeyNotFoundException : Exception
    {
        public DalKeyNotFoundException() { }
    }

    /// <summary>
    /// Throw when we couldn't open a file for XML.
    /// </summary>
    public class DalFileErrorException : Exception
    {
        public DalFileErrorException() { }
    }
}

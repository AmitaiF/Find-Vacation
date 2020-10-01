using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    /// <summary>
    /// Class for making only one copy of DAL.
    /// Using singleton method, and factory method.
    /// </summary>
    public class SingletonFactoryDAL
    {
        private static Idal instance = null;
        public static Idal GetDal()
        {
            if (instance == null)
                instance = new Dal_XML_imp();
            return instance;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace BL
{
    public class SingletonFactoryBL
    {
        private SingletonFactoryBL() { }

        private static IBL instance = null;

        public static IBL GetBL()
        {
            if (instance == null)
                instance = new MyBL();
            return instance;
        }
    }
}

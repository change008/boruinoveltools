using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tiexue.Framework.Data
{
    public class DefaultDataFacadeFactory:IDataFacadeFactory
    {

        IDataFacade IDataFacadeFactory.CreateDataFacade(string connectionstring)
        {
            if (string.IsNullOrEmpty(connectionstring))
            {
                throw new ArgumentNullException("connectstring", "Database connect string can not be null or empty.");
            }
             return new DefaultDataFacade(connectionstring);
        }
    }
}

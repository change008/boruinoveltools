using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tiexue.Framework.Data
{
    public interface IDataFacadeFactory
    {
        IDataFacade CreateDataFacade(string connectionString);
    }
}

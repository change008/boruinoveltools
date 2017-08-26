using System;
namespace Tiexue.Framework.Data
{
    public interface IRetrieveRawData
    {
        System.Data.DataTable SelectRawDataTable(string query);
        System.Data.DataTable SelectRawDataTable(string query, params System.Data.SqlClient.SqlParameter[] parameters);
    }
}

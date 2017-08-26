using System;
namespace Tiexue.Framework.Data
{
    public interface IDataFacade : IRetrieveRawData
    {
        int ExecuteCommand(string command, params object[] parameters);
        void ExecuteStoredProdedure(string spName);
        void ExecuteStoredProdedure(string spName, params System.Data.SqlClient.SqlParameter[] parameters);
        T ExecuteScalar<T>(string query);
        T ExecuteScalar<T>(string query, params System.Data.SqlClient.SqlParameter[] parameters);
        System.Collections.Generic.IDictionary<string, object> ExecuteStoredProdedureWithOutput(string spName, params System.Data.SqlClient.SqlParameter[] parameters);
        T ExecuteStoredProdedure<T>(string spName);
        T ExecuteStoredProdedure<T>(string spName, params System.Data.SqlClient.SqlParameter[] parameters);

        System.Collections.Generic.IEnumerable<T> ExecuteStoredProdedureSelect<T>(string spName);
        System.Collections.Generic.IEnumerable<T> ExecuteStoredProdedureSelect<T>(string spName, params System.Data.SqlClient.SqlParameter[] parameters);
        System.Collections.Generic.IEnumerable<T> ExecuteStoredProdedureSelectOrDefault<T>(string spName);
        System.Collections.Generic.IEnumerable<T> ExecuteStoredProdedureSelectOrDefault<T>(string spName, params System.Data.SqlClient.SqlParameter[] parameters);



        System.Collections.Generic.IEnumerable<T> Select<T>(string query);
        System.Collections.Generic.IEnumerable<T> Select<T>(string query, params System.Data.SqlClient.SqlParameter[] parameters);
        System.Collections.Generic.IDictionary<K, V> SelectDictionary<K, V>(string query)
            where K : IComparable
            where V : IComparable;
        System.Collections.Generic.IDictionary<K, V> SelectDictionary<K, V>(string query, params System.Data.SqlClient.SqlParameter[] parameters)
            where K : IComparable
            where V : IComparable;
        System.Collections.Generic.IDictionary<K, V> SelectDictionaryOrDefault<K, V>(string query)
            where K : IComparable
            where V : IComparable;

        System.Collections.Generic.IDictionary<K, V> SelectDictionaryOrDefault<K, V>(string query, params System.Data.SqlClient.SqlParameter[] parameters)
            where K : IComparable
            where V : IComparable;

        System.Collections.Generic.IEnumerable<T> SelectOrDefault<T>(string query, params System.Data.SqlClient.SqlParameter[] parameters);
        System.Collections.Generic.IEnumerable<T> SelectOrDefault<T>(string query);

        T Single<T>(string query);
        T Single<T>(string query, params System.Data.SqlClient.SqlParameter[] parameters);

    }
}

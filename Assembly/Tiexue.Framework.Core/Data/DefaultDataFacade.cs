/*
 * This Data Access Layer is Design Based on How the data is used.
 * 
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Data;

namespace Tiexue.Framework.Data
{
    public sealed class DefaultDataFacade : MarshalByRefObject, IDataFacade
    {
        #region log4net
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("TiexueFrameworkCore.Data");
        private Log4netAdapter _logAdapter;
        #endregion

        private string _connectstring;

        /// <summary>
        /// command timeout 
        /// </summary>
        private const int commandTimeout = 30;


        /// <summary>
        /// Initializes a new instance of the DefaultDataFacade (-->IDataFacade) class when given a string that contains the connection string.
        /// </summary>
        /// <param name="connectstring">The connection used to open the SQL Server database.</param>
        public DefaultDataFacade(string connectstring)
        {
            if (string.IsNullOrEmpty(connectstring))
            {
                throw new ArgumentNullException("connectstring", "Database connect string can not be null or empty.");
            }
            _connectstring = connectstring;
            if (log.IsDebugEnabled)
            {
                _logAdapter = new Log4netAdapter(log);
            }
        }

        /// <summary>
        ///  Returns the only object instance of class T, and return null  if there
        ///  is not exactly one row data  in the database. 
        ///  if the sequence If you only need to retrive a   single value,use the
        ///  ExecuteScalar method aviod the opeartions that are required to create a result set.
        /// </summary>
        /// <typeparam name="T">The type of the return instance.</typeparam>
        /// <param name="query"> The text of the query. </param>
        /// <returns> Instance of class T or default(T) for ValueType and  null for reference type.</returns>
        public T Single<T>(string query)
        {
            return SingleInternal<T>(_connectstring, query);
        }

        /// <summary>
        ///  Returns the only object instance of class T, and return null  if there
        ///  is not exactly one row data  in the database.
        /// </summary>
        /// <typeparam name="T">class</typeparam>
        /// <param name="query"> The text of the query.</param>
        /// <param name="parameters">The array of parameters to be passed to the command.</param>
        /// <returns>Instance of class T or default(T) for ValueType and  null for reference type.</returns>
        public T Single<T>(string query, params SqlParameter[] parameters)
        {
            return SingleInternal<T>(_connectstring, query, parameters);
        }

        /// <summary>
        ///  Executes SQL queries directly on the database and returns objects.
        /// </summary>
        /// <typeparam name="T"> The type of the elements in the returned collection.</typeparam>
        /// <param name="query">The SQL query to be executed.</param>
        /// <returns>A collection of objects returned by the query.</returns>
        public IEnumerable<T> Select<T>(string query)
        {
            return Select<T>(query, null);
        }

        /// <summary>
        ///  Executes SQL queries directly on the database and returns objects.
        /// </summary>
        /// <typeparam name="T"> The type of the elements in the returned collection.</typeparam>
        /// <param name="query">The SQL query to be executed.</param>
        /// <param name="parameters"> The array of parameters to be passed to the command. </param>
        /// <returns>A collection of objects returned by the query.</returns>
        public IEnumerable<T> Select<T>(string query, params SqlParameter[] parameters)
        {
            var list = SelectInternal<T>(_connectstring, query, parameters);
            return list.Count == 0 ? null : list;
        }

        /// <summary>
        ///   Executes SQL queries directly on the database and returns objects.
        /// </summary>
        /// <typeparam name="T"> The type of the elements in the returned collection.</typeparam>
        /// <param name="query">The SQL query to be executed.</param>
        /// <returns>A collection of objects returned by the query.</returns>
        public IEnumerable<T> SelectOrDefault<T>(string query)
        {
            return SelectInternal<T>(_connectstring, query);
        }

        /// <summary>
        ///   Executes SQL queries directly on the database and returns objects.
        /// </summary>
        /// <typeparam name="T"> The type of the elements in the returned collection.</typeparam>
        /// <param name="query">The SQL query to be executed.</param>
        /// <param name="parameters"> The array of parameters to be passed to the command. </param>
        /// <returns>A collection of objects returned by the query.</returns>
        public IEnumerable<T> SelectOrDefault<T>(string query, params SqlParameter[] parameters)
        {
            return SelectInternal<T>(_connectstring, query, parameters);
        }

        /// <summary>
        /// Executes SQL queries directly on the database and returns IDictionary.
        /// </summary>
        /// <typeparam name="K">  The type of the key.</typeparam>
        /// <typeparam name="V"> The type of the value.</typeparam>
        /// <param name="query">The SQL query to be executed.</param>
        /// <exception cref="System.ArgumentException:An element with the same key already exists in the  Dictionary."></exception>
        /// <returns> IDictionary</returns>
        public IDictionary<K, V> SelectDictionary<K, V>(string query)
            where K : IComparable
            where V : IComparable
        {
            var result = SelectDictionaryInternal<K, V>(_connectstring, query);
            return result.Count == 0 ? null : result;
        }

        /// <summary>
        /// Executes SQL queries directly on the database and returns IDictionary.
        /// </summary>
        /// <typeparam name="K">  The type of the key.</typeparam>
        /// <typeparam name="V"> The type of the value.</typeparam>
        /// <param name="query">The SQL query to be executed.</param>
        /// <param name="parameters"> The array of parameters to be passed to the command.</param>
        /// <exception cref="System.ArgumentException:An element with the same key already exists in the  Dictionary."></exception>
        /// <returns> IDictionary</returns>
        public IDictionary<K, V> SelectDictionary<K, V>(string query, params SqlParameter[] parameters)
            where K : IComparable
            where V : IComparable
        {
            var result = SelectDictionaryInternal<K, V>(_connectstring, query, parameters);
            return result.Count == 0 ? null : result;
        }

        /// <summary>
        /// Executes SQL queries directly on the database and returns IDictionary.
        /// </summary>
        /// <typeparam name="K">  The type of the key.</typeparam>
        /// <typeparam name="V"> The type of the value.</typeparam>
        /// <param name="query">The SQL query to be executed.</param>
        /// <param name="parameters"> The array of parameters to be passed to the command.</param>
        /// <exception cref="System.ArgumentException:An element with the same key already exists in the  Dictionary."></exception>
        /// <returns> IDictionary</returns>
        public IDictionary<K, V> SelectDictionaryOrDefault<K, V>(string query, params SqlParameter[] parameters)
            where K : IComparable
            where V : IComparable
        {
            return SelectDictionaryInternal<K, V>(_connectstring, query, parameters);
        }
        /// <summary>
        /// Executes SQL queries directly on the database and returns IDictionary.
        /// </summary>
        /// <typeparam name="K">  The type of the key.</typeparam>
        /// <typeparam name="V"> The type of the value.</typeparam>
        /// <param name="query">The SQL query to be executed.</param>
        /// <exception cref="System.ArgumentException:An element with the same key already exists in the  Dictionary."></exception>
        /// <returns> IDictionary</returns>
        public IDictionary<K, V> SelectDictionaryOrDefault<K, V>(string query)
            where K : IComparable
            where V : IComparable
        {
            return SelectDictionaryInternal<K, V>(_connectstring, query);
        }

        /// <summary>
        ///  Executes SQL commands directly on the database.
        /// </summary>
        /// <param name="command"> The SQL command to be executed.</param>
        /// <returns>An int representing the number of rows modified by the executed command.</returns>
        public int ExecuteCommand(string command)
        {
            if (string.IsNullOrEmpty(command))
            {
                throw new ArgumentNullException("command", "command cannot be null or empty.");
            }
            using (DataContext dc = new DataContext(_connectstring))
            {
                if (log.IsDebugEnabled)
                {
                    dc.Log = _logAdapter;
                    log.DebugFormat("DataContext ExecuteCommand Begin:{0}", command);
                }

                var ret = dc.ExecuteCommand(command, new string[] { });

                if (log.IsDebugEnabled)
                {
                    log.DebugFormat("DataContext ExecuteCommand End:{0}", command);
                }

                return ret;
            }
        }

        /// <summary>
        ///  Executes SQL commands directly on the database.
        /// </summary>
        /// <param name="command"> The SQL command to be executed.</param>
        /// <param name="parameters">The array of parameters to be passed to the command. Note the following behavior:If
        ///     the number of objects in the array is less than the highest number identified
        ///     in the command string, an exception is thrown.If the array contains objects
        ///     that are not referenced in the command string, no exception is thrown.If
        ///     any one of the parameters is null, it is converted to DBNull.Value.</param>
        /// <returns>An int representing the number of rows modified by the executed command.</returns>
        public int ExecuteCommand(string command, params object[] parameters)
        {
            if (string.IsNullOrEmpty(command))
            {
                throw new ArgumentNullException("command", "command cannot be null or empty.");
            }
            using (DataContext dc = new DataContext(_connectstring))
            {
                if (log.IsDebugEnabled)
                {
                    dc.Log = _logAdapter;
                    log.DebugFormat("DataContext ExecuteCommand Begin:{0}", command);
                }
                var ret = dc.ExecuteCommand(command, parameters);

                if (log.IsDebugEnabled)
                {
                    log.DebugFormat("DataContext ExecuteCommand End:{0}", command);
                }

                return ret;
            }
        }

        /// <summary>
        /// Execute a query that returns a raw DataTable resultset and takes no parameters.
        /// </summary>
        /// <param name="query">A valid T-SQL query.</param>
        /// <returns> A System.Data.DataTable that contains resultset.</returns>
        public DataTable SelectRawDataTable(string query)
        {
            return SelectRawDataTableInternal(_connectstring, query);
        }

        /// <summary>
        /// Execute a query that returns a raw DataTable resultset and takes no parameters.
        /// </summary>
        /// <param name="query">A valid T-SQL query.</param>
        /// <param name="parameters"> The array of parameters to be passed to the command.</param>
        /// <returns> A System.Data.DataTable that contains resultset.</returns>
        public DataTable SelectRawDataTable(string query, params SqlParameter[] parameters)
        {
            return SelectRawDataTableInternal(_connectstring, query, parameters);
        }

        /// <summary>
        /// Execute a query that returns a 1x1 resultset and takes no parameters .
        /// 
        /// e.g.: DateTime now=ExecuteScalar&lt;DateTime &gt;("select getdate();");
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns>An object containing the value in the 1x1 resultset generated by the query.</returns>
        public T ExecuteScalar<T>(string query)
        {
            return SingleInternal<T>(_connectstring, query);
        }

        /// <summary>
        /// Execute a query that returns a 1x1 resultset.
        /// using the provided parameters.
        /// </summary>
        /// <typeparam name="T">Type that the return value will be  Converted to.</typeparam>
        /// <param name="query">A valid T-SQL command</param>
        /// <param name="parameters">An array of SqlParamters used to execute the query.</param>
        /// <returns>An object containing the value in the 1x1 resultset generated by the query.</returns>
        public T ExecuteScalar<T>(string query, params SqlParameter[] parameters)
        {
            return SingleInternal<T>(_connectstring, query, parameters);
        }

        #region  StoredProdedure
        /// <summary>
        /// Execute the Stored Procedure against the  Stored Procedure Name specified in the parameter and take no parameters.
        /// </summary>
        /// <param name="storedProdedureName">The Stored Procedure name.</param>
        public void ExecuteStoredProdedure(string storedProdedureName)
        {
            ExecuteNonOutputStoredProdedureInternal(_connectstring, storedProdedureName);
        }

        /// <summary>
        /// Execute the Stored Procedure against the  Stored Procedure Name specified in the parameter and using the provided parameters.
        /// </summary>
        /// <param name="storedProdedureName">The Stored Procedure Name</param>
        /// <param name="parameters">An array of SqlParamters used to execute the query.</param>
        public void ExecuteStoredProdedure(string storedProdedureName, params SqlParameter[] parameters)
        {
            ExecuteNonOutputStoredProdedureInternal(_connectstring, storedProdedureName, parameters);
        }


        /// <summary>
        ///  Execute Store Procedure Return Raw DataTable , The DataTable may contains mutli Result Datatable. 
        /// </summary>
        /// <param name="storedProdedureName"></param>
        /// <param name="parameters"></param>
        public DataTable ExecuteStoredProdedureRaw(string storedProdedureName, params SqlParameter[] parameters)
        {
            return ExecuteStoredProdedureWithRawData(_connectstring, storedProdedureName, parameters);
        }



        /// <summary>
        ///  Creates a System.Collections.Generic.Dictionary  from an SqlParameter Array
        ///  according to the parameter direction.
        /// </summary>
        /// <param name="storedProdedureName">The Stored Procedure Name</param>
        /// <param name="parameters">An array of SqlParamters used to execute the query.</param>
        /// <returns>Output Parameter Result set </returns>
        public IDictionary<string, object> ExecuteStoredProdedureWithOutput(string storedProdedureName, params SqlParameter[] parameters)
        {
            return ExecuteOutputStoredProdedureInternal(_connectstring, storedProdedureName, parameters);
        }

        /// <summary>
        /// Execute a query that returns a 1x1 resultset and takes no parameters.
        /// </summary>
        /// <typeparam name="T">Type that the return value will be  Converted to.</typeparam>
        /// <param name="query">A valid T-SQL command</param>
        /// <param name="parameters">An array of SqlParamters used to execute the query.</param>
        /// <returns>An object containing the value in the 1x1 resultset generated by the query.</returns>
        public T ExecuteStoredProdedure<T>(string storedProdedureName)
        {
            return ExecuteSingleOutputStoredProdedureInternal<T>(_connectstring, storedProdedureName);
        }

        /// <summary>
        /// Execute a query that returns a 1x1 resultset.
        /// using the provided parameters.
        /// </summary>
        /// <typeparam name="T">Type that the return value will be  Converted to.</typeparam>
        /// <param name="query">A valid T-SQL command</param>
        /// <param name="parameters">An array of SqlParamters used to execute the query.</param>
        /// <returns>An object containing the value in the 1x1 resultset generated by the query.</returns>
        public T ExecuteStoredProdedure<T>(string storedProdedureName, params SqlParameter[] parameters)
        {
            return ExecuteSingleOutputStoredProdedureInternal<T>(_connectstring, storedProdedureName, parameters);
        }

        /// <summary>
        ///   Execute the Stored Procedure against the  Stored Procedure Name specified in the parameter and take no parameters.
        /// </summary>
        /// <typeparam name="T"> The type of the elements in the returned collection.</typeparam>
        /// <param name="spName">The Stored Procedure Name</param>
        /// <returns>A collection of objects returned by the query.</returns>
        public IEnumerable<T> ExecuteStoredProdedureSelect<T>(string spName)
        {
            var result = SelectInternal<T>(_connectstring, spName, null, true);
            return result.Count == 0 ? null : result;
        }
        /// <summary>
        ///   Execute the Stored Procedure against the  Stored Procedure Name specified in the parameter and using the provided parameters.
        /// </summary>
        /// <typeparam name="T"> The type of the elements in the returned collection.</typeparam>
        /// <param name="spName">The Stored Procedure Name</param>
        /// <param name="parameters">An array of SqlParamters used to execute the query.</param>
        /// <returns>A collection of objects returned by the query.</returns>
        public IEnumerable<T> ExecuteStoredProdedureSelect<T>(string spName, params SqlParameter[] parameters)
        {
            var result = SelectInternal<T>(_connectstring, spName, parameters, true);
            return result.Count == 0 ? null : result;
        }
        /// <summary>
        ///   Execute the Stored Procedure against the  Stored Procedure Name specified in the parameter and take no parameters.
        /// </summary>
        /// <typeparam name="T"> The type of the elements in the returned collection.</typeparam>
        /// <param name="spName">The Stored Procedure Name</param>
        /// <returns>A collection of objects returned by the query.</returns>
        public IEnumerable<T> ExecuteStoredProdedureSelectOrDefault<T>(string spName)
        {
            return SelectInternal<T>(_connectstring, spName, null, true);
        }
        /// <summary>
        ///   Execute the Stored Procedure against the  Stored Procedure Name specified in the parameter and using the provided parameters.
        /// </summary>
        /// <typeparam name="T"> The type of the elements in the returned collection.</typeparam>
        /// <param name="spName">The Stored Procedure Name</param>
        /// <param name="parameters">An array of SqlParamters used to execute the query.</param>
        /// <returns>A collection of objects returned by the query.</returns>
        public IEnumerable<T> ExecuteStoredProdedureSelectOrDefault<T>(string spName, params SqlParameter[] parameters)
        {
            return SelectInternal<T>(_connectstring, spName, parameters, true);
        }

        #endregion

        #region Internal Implementation

        private static void LogSqlParameters(SqlParameter[] parameters)
        {
            if (!log.IsDebugEnabled) return;

            int length = parameters.Length;
            for (int i = 0; i < length; i++)
            {
                log.DebugFormat("Parameter[{0}:{1}]", parameters[i].ParameterName, parameters[i].Value);
            }
        }
        private static DataTable SelectRawDataTableInternal(string connectstring, string query, SqlParameter[] parameters = null)
        {
            if (string.IsNullOrEmpty(query))
            {
                throw new ArgumentNullException("query", "query cannot be null or empty.");
            }
            if (log.IsDebugEnabled)
            {
                log.DebugFormat("Query Begin:{0}", query);
            }
            SqlConnection connection = new SqlConnection(connectstring);
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.CommandTimeout = commandTimeout;
            if (parameters != null && parameters.Length > 0)
            {
                cmd.Parameters.AddRange(parameters);
                LogSqlParameters(parameters);
            }

            DataTable result = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            //Notice:Do not explicitly open a Connection if you use Fill or update for a Single operation
            adapter.Fill(result);
            if (log.IsDebugEnabled)
            {
                log.DebugFormat("Query End:{0}", query);
            }
            return result;
        }

        private static IDictionary<string, object> ExecuteOutputStoredProdedureInternal(string connectstring, string storedProdedureName, SqlParameter[] parameters)
        {
            if (string.IsNullOrEmpty(storedProdedureName))
            {
                throw new ArgumentNullException("storedProdedureName", "storedProdedureName cannot be null or empty.");
            }

            if (parameters == null || parameters.Length == 0)
            {
                throw new ArgumentNullException("parameters", "parameters cannot be null or empty.");
            }
            if (log.IsDebugEnabled)
            {
                log.DebugFormat("Query Begin:{0}", storedProdedureName);
            }
            SqlConnection connection = new SqlConnection(connectstring);
            SqlCommand cmd = new SqlCommand(storedProdedureName, connection);
            cmd.CommandTimeout = commandTimeout;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddRange(parameters);
            LogSqlParameters(parameters);

            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                connection.Open();
                cmd.ExecuteNonQuery();
                if (log.IsDebugEnabled)
                {
                    log.DebugFormat("Query End:{0}", storedProdedureName);
                }
                return parameters.Where(n => n.Direction != ParameterDirection.Input).ToDictionary(n => n.ParameterName, v => cmd.Parameters[v.ParameterName].Value);
            }
            finally
            {
                connection.Close();
            }
        }


        private static T ExecuteSingleOutputStoredProdedureInternal<T>(string connectstring, string storedProdedureName, SqlParameter[] parameters = null)
        {
            if (string.IsNullOrEmpty(storedProdedureName))
            {
                throw new ArgumentNullException("storedProdedureName", "storedProdedureName cannot be null or empty.");
            }
            if (log.IsDebugEnabled)
            {
                log.DebugFormat("Query Begin:{0}", storedProdedureName);
            }
            return SingleInternal<T>(connectstring, storedProdedureName, parameters, true);
        }

        private static DataTable ExecuteStoredProdedureWithRawData(string connectstring, string storedProdedureName, SqlParameter[] parameters = null)
        {
            if (string.IsNullOrEmpty(storedProdedureName))
            {
                throw new ArgumentNullException("storedProdedureName", "storedProdedureName cannot be null or empty.");
            }
            if (log.IsDebugEnabled)
            {
                log.DebugFormat("Query Begin:{0}", storedProdedureName);
            }

            SqlConnection connection = new SqlConnection(connectstring);
            SqlCommand cmd = new SqlCommand(storedProdedureName, connection);
            cmd.CommandTimeout = commandTimeout;
            cmd.CommandType = CommandType.StoredProcedure;

            if (parameters != null && parameters.Length > 0)
            {
                cmd.Parameters.AddRange(parameters);
                LogSqlParameters(parameters);
            }

            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = cmd;
            DataTable dataTable = new DataTable();
            try
            {
                adapter.Fill(dataTable);
            }
            finally
            {
                connection.Close();
            }
            return dataTable;
        }



        private static void ExecuteNonOutputStoredProdedureInternal(string connectstring, string storedProdedureName, SqlParameter[] parameters = null)
        {
            if (string.IsNullOrEmpty(storedProdedureName))
            {
                throw new ArgumentNullException("storedProdedureName", "storedProdedureName cannot be null or empty.");
            }
            if (log.IsDebugEnabled)
            {
                log.DebugFormat("Query Begin:{0}", storedProdedureName);
            }

            SqlConnection connection = new SqlConnection(connectstring);
            SqlCommand cmd = new SqlCommand(storedProdedureName, connection);
            cmd.CommandTimeout = commandTimeout;
            cmd.CommandType = CommandType.StoredProcedure;
            if (parameters != null && parameters.Length > 0)
            {
                cmd.Parameters.AddRange(parameters);
                LogSqlParameters(parameters);
            }

            try
            {
                connection.Open();
                cmd.ExecuteNonQuery();
                if (log.IsDebugEnabled)
                {
                    log.DebugFormat("Query End:{0}", storedProdedureName);
                }
            }
            finally
            {
                connection.Close();
            }
        }


        private static List<T> SelectInternal<T>(string connectstring, string query, SqlParameter[] parameters = null, bool isStoredProdedure = false)
        {
            if (string.IsNullOrEmpty(query))
            {
                throw new ArgumentNullException("query", "query cannot be null or empty.");
            }
            if (log.IsDebugEnabled)
            {
                log.DebugFormat("Query Begin:{0}", query);
            }
            SqlConnection connection = new SqlConnection(connectstring);
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.CommandTimeout = commandTimeout;
            cmd.CommandType = isStoredProdedure ? CommandType.StoredProcedure : CommandType.Text;
            if (parameters != null && parameters.Length > 0)
            {
                cmd.Parameters.AddRange(parameters);
                LogSqlParameters(parameters);
            }
            List<T> list = new List<T>();
            DynamicBuilder<T> _builder = null;
            connection.Open();
            using (SqlDataReader sqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
            {
                Type t = typeof(T);
                while (sqlDataReader.Read())
                {
                    if (t.IsValueType || t == typeof(string) || t == typeof(byte[]))
                    {
                        list.Add((T)Convert.ChangeType(sqlDataReader[0], typeof(T)));
                    }
                    else
                    {
                        if (_builder == null)
                        {
                            _builder = DynamicBuilderCache.ResolveDynamicBuilder<T>(sqlDataReader);
                        }

                        T entity = _builder.BuildEntity(sqlDataReader);

                        if (entity != null)
                        {
                            list.Add(entity);
                        }
                    }
                }
                cmd.Cancel();
            }
            if (log.IsDebugEnabled)
            {
                log.DebugFormat("Query End:{0}", query);
            }
            return list;
        }


        private static T SingleInternal<T>(string connectstring, string query, SqlParameter[] parameters = null, bool isStoredProdedure = false)
        {
            if (string.IsNullOrEmpty(query))
            {
                throw new ArgumentNullException("query", "query cannot be null or empty.");
            }

            if (log.IsDebugEnabled)
            {
                log.DebugFormat("Query Begin:{0}", query);
            }
            SqlConnection connection = new SqlConnection(connectstring);
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.CommandTimeout = commandTimeout;
            cmd.CommandType = isStoredProdedure ? CommandType.StoredProcedure : CommandType.Text;

            if (parameters != null && parameters.Length > 0)
            {
                cmd.Parameters.AddRange(parameters);
                LogSqlParameters(parameters);
            }
            Type t = typeof(T);

            if (t.IsValueType || t == typeof(string) || t == typeof(byte[]))
            {
                object obj = null;
                try
                {
                    connection.Open();
                    obj = cmd.ExecuteScalar();
                    if (log.IsDebugEnabled)
                    {
                        log.DebugFormat("Query End:{0}", query);
                    }
                }
                finally
                {
                    connection.Close();
                }
                if (obj != null)
                {
                    return (T)Convert.ChangeType(obj, typeof(T));
                }
                return default(T);
            }

            DynamicBuilder<T> _builder = null;
            connection.Open();
            using (SqlDataReader sqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
            {

                while (sqlDataReader.Read())
                {


                    if (_builder == null)
                    {
                        _builder = DynamicBuilderCache.ResolveDynamicBuilder<T>(sqlDataReader);
                    }
                    return _builder.BuildEntity(sqlDataReader);

                }
                cmd.Cancel();
            }
            if (log.IsDebugEnabled)
            {
                log.DebugFormat("Query End:{0}", query);
            }
            return default(T);
        }

        private static IDictionary<K, V> SelectDictionaryInternal<K, V>(string connectstring, string query, SqlParameter[] parameters = null)
            where K : IComparable
            where V : IComparable
        {
            if (string.IsNullOrEmpty(query))
            {
                throw new ArgumentNullException("query", "query cannot be null or empty.");
            }
            if (log.IsDebugEnabled)
            {
                log.DebugFormat("Query Begin:{0}", query);
            }
            SqlConnection connection = new SqlConnection(connectstring);
            SqlCommand cmd = new SqlCommand(query, connection);
            if (parameters != null && parameters.Length > 0)
            {
                cmd.Parameters.AddRange(parameters);
                LogSqlParameters(parameters);
            }
            connection.Open();
            Dictionary<K, V> dict = new Dictionary<K, V>();
            cmd.CommandTimeout = commandTimeout;
            using (SqlDataReader sqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
            {
                Type tk = typeof(K);
                Type tv = typeof(V);
                while (sqlDataReader.Read())
                {
                    if (tk.IsValueType || tk == typeof(string))
                    {
                        var key = (K)Convert.ChangeType(sqlDataReader[0], typeof(K));
                        if (tv.IsValueType || tv == typeof(string))
                        {
                            var value = (V)Convert.ChangeType(sqlDataReader[1], typeof(V));
                            dict.Add(key, value);
                        }
                    }
                }
                cmd.Cancel();
            }
            if (log.IsDebugEnabled)
            {
                log.DebugFormat("Query End:{0}", query);
            }
            return dict;
        }
        #endregion



    }
}

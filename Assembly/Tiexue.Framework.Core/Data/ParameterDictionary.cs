using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Tiexue.Framework.Data
{
    /// <summary>
    ///      Represents a Dictionary of parameters associated with SqlParameters.
    ///      key:parameterName value:SqlParameter
    ///      This class cannot be inherited and can be used only internal.
    /// </summary>
    public sealed class SqlParameterDictionary
    {
        IDictionary<string, SqlParameter> _dict;

        internal SqlParameterDictionary(SqlParameter[] parameters)
        {
            if (parameters == null || parameters.Length == 0)
            {
                throw new ArgumentNullException("Parameters", "There is no parameters in this context.");
            }
            _dict = parameters.ToDictionary(n => n.ParameterName);
        }

        /// <summary>
        ///  Gets the System.Data.SqlClient.SqlParameter with the specified name.
        /// </summary>
        /// <param name="parameterName">The name of the parameter to retrieve.</param>
        ///<exception cref="KeyNotFoundException"></exception>
        /// <returns> The System.Data.SqlClient.SqlParameter with the specified name.</returns>
        public SqlParameter this[string parameterName]
        {
            get
            {
                return _dict[parameterName];
            }
        }

        /// <summary>
        /// SqlParameters in the SqlParameterDictionary.
        /// </summary>
        public SqlParameter[] SqlParameters
        {
            get
            {
                return _dict.Values.ToArray();
            }
        }
    }
}

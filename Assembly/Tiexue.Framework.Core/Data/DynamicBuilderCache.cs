using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Collections;

namespace Tiexue.Framework.Data
{
    internal static class DynamicBuilderCache
    {
        //dynamicBuilderCache will be shared between sessions within the same work process.
        private static Hashtable dynamicBuilderCache = Hashtable.Synchronized(new Hashtable());

        static string ResolveDynamicBuilderCacheKeyInternal(SqlDataReader sqlDataReader, Type type)
        {
            string cacheKey = string.Empty;
            if (!type.IsValueType && type != typeof(string) && type != typeof(byte[]))
            {
                StringBuilder sb = new StringBuilder(type.Name);
                sb.Append("-");
                for (int i = 0; i < sqlDataReader.FieldCount; i++)
                {
                    sb.Append(sqlDataReader.GetName(i));
                    sb.Append("_");
                }

                cacheKey = sb.ToString();
            }
            return cacheKey;
        }

        public static DynamicBuilder<T> ResolveDynamicBuilder<T>(SqlDataReader sqlDataReader)
        {
            string cacheKey = string.Empty;
            cacheKey = ResolveDynamicBuilderCacheKeyInternal(sqlDataReader, typeof(T));
            DynamicBuilder<T> builder = null;
            if (dynamicBuilderCache.ContainsKey(cacheKey))
            {
                builder = dynamicBuilderCache[cacheKey] as DynamicBuilder<T>;
            }
            if (builder == null)
            {
                builder = DynamicBuilder<T>.CreateEntityBuilder(sqlDataReader);
                dynamicBuilderCache[cacheKey] = builder;
            }
            return builder;
        }
    }
}

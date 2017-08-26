using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace NovelCollProjectutils
{
    public static class ObjectExtension
    {
        const string packFormat = "\"{0}\"";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string PackQuotation(this string str)
        {
            return string.Format(packFormat, str);
        }

        public static string IfNullToEmpty(this string src)
        {
            if (string.IsNullOrEmpty(src))
            {
                return string.Empty;
            }
            return src;
        }

        /// <summary>
        /// string 字符串截取前面长度
        /// </summary>
        /// <param name="str"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string CutStr(this string str, int len)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            int finalLen = Math.Min(str.Length, len);
            return str.Substring(0, finalLen);
        }


        public static T ToSimpleT<T>(this object value, T defaultValue)
        {
            if (value == null)
            {
                return defaultValue;
            }
            else if (value is T)
            {
                return (T)value;
            }
            try
            {
                if (typeof(T).BaseType == typeof(Enum))
                {
                    object objValue = Enum.Parse(typeof(T), value.ToString());
                    return (T)objValue;
                }
                Type typ = typeof(T);
                if (typ.BaseType == typeof(ValueType) && typ.IsGenericType)//可空泛型
                {
                    Type[] typs = typ.GetGenericArguments();
                    return (T)Convert.ChangeType(value, typs[0]);
                }
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch
            {
                return defaultValue;
            }
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static byte[] Serialize<T>(this T obj)
        {
            using (MemoryStream fs = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, obj);
                return fs.ToArray();
            }
        }

        public static T DeSerialize<T>(this byte[] data)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            T t;
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    ms.Write(data, 0, data.Length);
                    ms.Position = 0;
                    t = (T)formatter.Deserialize(ms);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return t;
        }




        /// <summary>
        /// 不允许为Null。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="instance">对象实例。</param>
        /// <param name="name">参数名称。</param>
        /// <returns>对象实例。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="instance"/> 为null。</exception>
        public static T NotNull<T>(this T instance, string name) where T : class
        {
            if (instance == null)
                throw new ArgumentNullException(name.NotEmpty("name"));
            return instance;
        }

        /// <summary>
        /// 不允许空字符串。
        /// </summary>
        /// <param name="str">字符串。</param>
        /// <param name="name">参数名称。</param>
        /// <returns>字符串。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="str"/> 为空。</exception>
        public static string NotEmpty(this string str, string name)
        {
            if (string.IsNullOrEmpty(str))
                throw new ArgumentNullException(name.NotEmpty("name"));
            return str;
        }

        /// <summary>
        /// 不允许空和只包含空格的字符串。
        /// </summary>
        /// <param name="str">字符串。</param>
        /// <param name="name">参数名称。</param>
        /// <returns>字符串。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="str"/> 为空或者全为空格。</exception>
        public static string NotEmptyOrWhiteSpace(this string str, string name)
        {
            if (string.IsNullOrWhiteSpace(str))
                throw new ArgumentNullException(name.NotEmpty("name"));
            return str;
        }
    }
}

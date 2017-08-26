using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Tiexue.Framework.Extension
{
    public static class BaseTypeExtension
    {
        /// <summary>
        /// When a double d < 0 ? 0 : d;
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static double SelfCorrecting(this double d)
        {
            return d < 0 ? 0 : d;
        }

        /// <summary>
        /// When a int d < 0 ? 0 : d;
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static int SelfCorrecting(this int d)
        {
            return d < 0 ? 0 : d;
        }

        /// <summary>
        /// Indicates whether a specified ICollection is null, empty
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this ICollection collection)
        {
            if (collection == null || collection.Count == 0)
            {
                return true;
            }
            return false;
        }

        public static void OnEvent<T>(this EventHandler<T> handler, object obj, T e) where T : EventArgs
        {
            var handle = handler;
            if (handle != null)
                handle.Invoke(obj, e);
        }


        /// <summary>
        ///  Converts the int array to its equivalent string array
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string[] ToStringArray(this int[] source)
        {
            if (source == null || source.Length == 0)
            {
                return null;
            }
            return Array.ConvertAll<int, string>(source, new Converter<int, string>(n => n.ToString()));
        }

        /// <summary>
        ///  Concatenates the specified elements of a string array, using the specified separator between each element.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string JoinToString(this int[] source, string separator)
        {
            if (string.IsNullOrEmpty(separator))
            {
                throw new ArgumentNullException("separator", "JoinToString(this int[] source, string separaptor) Separator cannot be null.");
            }

            if (source == null || source.Length == 0)
            {
                return string.Empty;
            }
            return string.Join(separator, Array.ConvertAll<int, string>(source, new Converter<int, string>(n => n.ToString())));
        }

        /// <summary>
        /// Ensure List not null to facilate using foreach
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<T> EnsureNotEmptyListOf<T>(this List<T> list)
        {
            if (list == null)
            {
                return new List<T>();
            }
            return list;
        }


    }
}

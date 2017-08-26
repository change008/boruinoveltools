using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tiexue.Framework.Extension
{
    public class EnumUtil
    {
        public static List<T> ToList<T>()
        {
            Type enumType = typeof(T);

            // Can't use type constraints on value types, so have to do check like this
            if (enumType.BaseType != typeof(Enum))
                throw new ArgumentException("T must be of type System.Enum");

            Array enumArray = Enum.GetValues(enumType);

            List<T> enumList = new List<T>(enumArray.Length);

            foreach (int val in enumArray)
            {
                enumList.Add((T)Enum.Parse(enumType, val.ToString()));
            }

            return enumList;
        }
    }
}

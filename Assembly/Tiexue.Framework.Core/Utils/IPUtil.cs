using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Tiexue.Framework.Utils {
    public static class IPUtil {
        private static readonly Regex IPv4Regex = new Regex(RegexRules.IPv4);

        public static string ConvertToString(long ipLong) {
            var ip = string.Empty;
            for(var i = 0; i < 4; i++) {
                var num = (int) (ipLong/Math.Pow(256, (3 - i)));
                ipLong = ipLong - (long) (num*Math.Pow(256, (3 - i)));
                ip = i == 0 ? num.ToString(CultureInfo.InvariantCulture) : ip + "." + num;
            }
            return ip;
        }

        public static long ConvertToLong(string ipString) {
            if (!IPv4Regex.IsMatch(ipString)) return 0;
            double num = 0;
            var ipBytes = ipString.Split('.');
            for(var i = ipBytes.Length - 1; i >= 0; i--) {
                num += ((int.Parse(ipBytes[i])%256)*Math.Pow(256, (3 - i)));
            }
            return (long) num;
        }

        [Obsolete("Use ConvertToString(long ipLong) instead")]
        public static string ConvertToString(uint ip) {
            if(ip == 0) return "0.0.0.0";

            string temp = (ip & 0xff).ToString(CultureInfo.InvariantCulture);

            ip = ip >> 8;
            temp = string.Format("{0}.{1}", ip & 0xff, temp);

            ip = ip >> 8;
            temp = string.Format("{0}.{1}", ip & 0xff, temp);

            ip = ip >> 8;
            temp = string.Format("{0}.{1}", ip & 0xff, temp);

            return temp;
        }

        [Obsolete("Use ConvertToLong(String ipString) instead")]
        public static uint ConvertToUInt(string ipString) {
            var ipzone = ipString.Split('.');
            if(ipzone.Length != 4) {
                return 0;
            }
            uint digitalIP;
            try {
                digitalIP = Convert.ToUInt32(ipzone[0]);
                digitalIP = digitalIP << 8;
                digitalIP += Convert.ToUInt32(ipzone[1]);
                digitalIP = digitalIP << 8;
                digitalIP += Convert.ToUInt32(ipzone[2]);
                digitalIP = digitalIP << 8;
                digitalIP += Convert.ToUInt32(ipzone[3]);
            }
            catch {
                return 0;
            }
            return digitalIP;
        }
    }
}

using System;
using System.Text.RegularExpressions;

namespace NovelCollProject.utils
{
    public class DateUtil
    {
        public static DateTime FormatDateTime(Int64 timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);

            return dtStart.Add(toNow);
        }
        public static string FormatDateTime(Int64 timeStamp,string format)
        {
            return FormatDateTime(timeStamp).ToString(format);
        }
        public static string FormatTimeDisplay(Int64 timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);

            return FormatTimeDisplay(dtStart.Add(toNow));
        }

        public static string FormatTimeDisplay(DateTime dtBegin)
        {
            string result = string.Empty;
            TimeSpan ts = DateTime.Now.Subtract(dtBegin);

            int day = ts.Days;
            int hour = ts.Hours;
            int min = ts.Minutes;
            int sec = ts.Seconds;

            if (day > 0 && day < 4)
            {
                result = day + "天前";
            }
            else if (hour > 0)
            {
                result = hour + "小时前";
            }
            else if (min > 0)
            {
                result = min + "分钟前";
            }
            else if(sec > 0)
            {
                result = sec + "秒前";
            }
            else
            {
                result = dtBegin.ToString("yyyy-MM-dd HH:mm:ss");
            }

            return result;

        }

        public static Int64 GetTimeStamp()
        {
            return GetTimeStamp(DateTime.Now);
        }
        public static Int64 GetTimeStamp(DateTime dt)
        {
            long nowtime = Convert.ToInt64((dt.ToUniversalTime().Ticks - 621355968000000000) / 10000000);
            return nowtime;
        }

        public static string GetDateFromString(params string[] strs)
        {
            Regex r1 = new Regex(@"(20\d{1,3})[\-\.\/\\\s年](\d{1,2})[\-\.\/\\\s月](\d{1,2})");
            Regex r2 = new Regex(@"(20[0,1]\d)(\d{1,2})(\d{1,2})");
            Regex r3 = new Regex(@"(20[01]\d)\s*年");
            Regex r4 = new Regex(@"20[01]\d");

            for (int i = 0; i < strs.Length; i++)
            {
                if (string.IsNullOrEmpty(strs[i])) continue;

                Match match = r1.Match(strs[i]);
                if (match.Success)
                {
                    string y = r1.Replace(match.Value, "$1");
                    string m = r1.Replace(match.Value, "$2");
                    string d = r1.Replace(match.Value, "$3");

                    if (y.Length == 3) y = y.Insert(2, "0");
                    else if (y.Length == 5) y = y.Substring(0, 4);

                    if (m.Length == 1) m = "0" + m;
                    else if (m.Length == 0) m = "01";

                    if (d.Length == 1) d = "0" + d;
                    else if (d.Length == 0) d = "01";

                    return string.Format("{0}-{1}-{2}", y, m, d);
                }
            }

            for (int i = 0; i < strs.Length; i++)
            {
                if (string.IsNullOrEmpty(strs[i])) continue;

                Match match = r2.Match(strs[i]);
                if (match.Success)
                {
                    string y = r2.Replace(match.Value, "$1");
                    string m = r2.Replace(match.Value, "$2");
                    string d = r2.Replace(match.Value, "$3");

                    if (m.Length == 1) m = "0" + m;

                    if (d.Length == 1) d = "0" + d;

                    return string.Format("{0}-{1}-{2}", y, m, d);

                }
            }

            for (int i = 0; i < strs.Length; i++)
            {
                if (string.IsNullOrEmpty(strs[i])) continue;

                Match match = r3.Match(strs[i]);
                if (match.Success)
                {
                    string y = r3.Replace(match.Value, "$1");

                    return string.Format("{0}-01-01", y);

                }
            }

            for (int i = 0; i < strs.Length; i++)
            {
                if (string.IsNullOrEmpty(strs[i])) continue;

                Match match = r4.Match(strs[i]);
                if (match.Success)
                {
                    return string.Format("{0}-01-01", match.Value);
                }
            }

            return string.Empty;
        }
    }
}

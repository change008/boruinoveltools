using System;
using System.Web;

namespace Tiexue.Framework.Utils {
    public static class CookiesUtil {
        public static void SetCookie(string name, string value) {
            SetCookie(name, value, 1, null);
        }

        public static void SetCookie(string name, string value, int expiresDays) {
            SetCookie(name, value, expiresDays, null, "/");
        }

        public static void SetCookie(string name, string value, int expiresDays, string domain) {
            SetCookie(name, value, expiresDays, domain, "/");
        }

        public static void SetCookie(string name, string value, int expiresDays, string domain, string path) {
            DateTime expires = DateTime.Now.AddDays(expiresDays);
            SetCookie(name, value, domain, expires, path);
        }

        public static void SetCookie(string name, string value, string domain, DateTime expires, string path) {
            if(value == null) {
                return;
            }
            HttpCookie cookie = new HttpCookie(name, value);
            cookie.Expires = expires;
            if(domain != null) {
                cookie.Domain = domain;
            }
            if(path != null) {
                cookie.Path = path;
            }
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public static string GetCookie(string name) {
            var cookie = HttpContext.Current.Request.Cookies[name];
            return cookie != null ? cookie.Value : null;
        }

        public static void RemoveCookie(string name) {
            HttpContext.Current.Response.Cookies.Remove(name);
        }
    }
}
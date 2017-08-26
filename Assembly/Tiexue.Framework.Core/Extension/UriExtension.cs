using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tiexue.Framework.Extension {
    public static class UriExtension {
        /// <summary>
        /// 从URI中获取一级域名
        /// </summary>
        /// <returns></returns>
        public static string GetDomain(this Uri uri) {
            if(uri.HostNameType != UriHostNameType.Dns)
                return uri.Host;

            string domain = uri.Host;
            string[] domainsplit = domain.Split(new[] {'.'});
            int domainlength = domainsplit.Length;

            if(domainlength > 1)
                domain = domainsplit[domainlength - 2] + "." + domainsplit[domainlength - 1];

            return domain;
        }
    }
}

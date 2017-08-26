using System.Web;

namespace Tiexue.Framework.Web {
    public static class HttpRequestExtension {
        //public static string GetClientIP(this HttpRequestBase request) {
        //    //ServerVariables参数参考资料
        //    //http://blog.csdn.net/Damon_King/archive/2007/12/06/1920856.aspx

        //    string ipString;

        //    if(request.ServerVariables["REMOTE_ADDR"] != null) {
        //        //发出请求的远程主机的IP地址
        //        ipString = request.ServerVariables["REMOTE_ADDR"];
        //    }
        //    else if(request.ServerVariables["HTTP_VIA"] != null) {
        //        //判断是否设置代理，若使用了代理
        //        ipString = request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? request.UserHostAddress;
        //    }
        //    else {
        //        ipString = request.UserHostAddress;
        //    }

        //    return ipString;
        //}

        public static string GetClientIP(this HttpRequestBase request) {
            string ipAddress = "";
            //ServerVariables参数参考资料
            //http://blog.csdn.net/Damon_King/archive/2007/12/06/1920856.aspx
            if(HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FORTIEXUE"] != null) //发出请求的远程主机的IP地址
            {
                ipAddress = request.ServerVariables["HTTP_X_FORWARDED_FORTIEXUE"];
            } else if(request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null) //获取代理服务器的IP
            {
                ipAddress = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            } else if(request.ServerVariables["REMOTE_ADDR"] != null) //获取代理服务器的IP
            {
                ipAddress = request.ServerVariables["REMOTE_ADDR"];
            } else {
                ipAddress = request.UserHostAddress;
            }
            return ipAddress;
        }


        public static string GetClientIP(this HttpRequest request)
        {
            string ipAddress = "";
            //ServerVariables参数参考资料
            //http://blog.csdn.net/Damon_King/archive/2007/12/06/1920856.aspx
            if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FORTIEXUE"] != null) //发出请求的远程主机的IP地址
            {
                ipAddress = request.ServerVariables["HTTP_X_FORWARDED_FORTIEXUE"];
            }
            else if (request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null) //获取代理服务器的IP
            {
                ipAddress = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            }
            else if (request.ServerVariables["REMOTE_ADDR"] != null) //获取代理服务器的IP
            {
                ipAddress = request.ServerVariables["REMOTE_ADDR"];
            }
            else
            {
                ipAddress = request.UserHostAddress;
            }
            return ipAddress;
        }
    }
}

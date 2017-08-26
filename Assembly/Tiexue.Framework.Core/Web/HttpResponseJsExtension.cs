using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Tiexue.Framework.Web {
    public static class HttpResponseJsExtension {

        private const string JS_SCRIPT_BEGIN = "<script>";
        private const string JS_SCRIPT_END = "</script>";
        private const string JS_CLOSE_WINDOW = "window.close();";
        private const string JS_HISTORY_BACK = "history.back();";
        private const string JS_DOCUMENT_LOCATION_FORMAT = "document.location='{0}'";
        private const string JS_ALERT_FORMAT = "alert('{0}')";
        private const string JS_WINDOW_OPEN_FORMAT = "window.open('{0}','{1}','{2}');";

        public static void AppendJsAlertAndClose(this HttpResponse response, string message) {
            AppendJsAlert(response, message, JS_CLOSE_WINDOW);
        }

        public static void AppendJsAlertAndRedirect(this HttpResponse response, string message, string redirectTo) {
            AppendJsAlert(response, message, String.IsNullOrWhiteSpace(redirectTo) ? null : String.Format(JS_DOCUMENT_LOCATION_FORMAT, redirectTo));
        }

        public static void AppendJsAlertAndBackHistory(this HttpResponse response, string message) {
            AppendJsAlert(response, message, JS_HISTORY_BACK);
        }

        public static void AppendJsAlert(this HttpResponse response, string message, string jsAction) {
            var sb = new StringBuilder();
            sb.Append(JS_SCRIPT_BEGIN);
            sb.AppendFormat(JS_ALERT_FORMAT, message);
            if(jsAction != null)
                sb.Append(jsAction);
            sb.Append(JS_SCRIPT_END);
            response.Write(sb.ToString());
        }

        public static void AppendJsConfirm(this HttpResponse response, string message, string url, bool backHistory,
                                           bool channelmode = false, bool directories = true, bool fullscreen = false, bool location = true,
                                           bool menubar = true, bool resizable = true, bool scrollbars = true, bool status = true, bool titlebar = true,
                                           int height = 530, int width = 500, int left = 0, int top = 0) {

            // Append a parameter when its value is not default, see http://www.w3schools.com/jsref/met_win_open.asp
            StringBuilder parasBuilber = new StringBuilder();
            if(channelmode) parasBuilber.Append("channelmode=yes,");
            if(!directories) parasBuilber.Append("directories=no,");
            if(fullscreen) parasBuilber.Append("fullscreen=yes");
            if(!location) parasBuilber.Append("location=no,");
            if(!menubar) parasBuilber.Append("menubar=no,");
            if(!resizable) parasBuilber.Append("resizable=no,");
            if(!scrollbars) parasBuilber.Append("scrollbars=no,");
            if(!status) parasBuilber.Append("status=no,");
            if(!titlebar) parasBuilber.Append("titlebar=no,");
            if(height != 530) parasBuilber.AppendFormat("height={0},", height);
            if(width != 500) parasBuilber.AppendFormat("width={0},", width);
            if(left != 0) parasBuilber.AppendFormat("left={0},", left);
            if(top != 0) parasBuilber.AppendFormat("top={0},", top);
            string paras = parasBuilber.ToString();
            if(paras.Length > 0 && paras.EndsWith(","))
                paras = paras.Remove(paras.Length - 1);

            StringBuilder sb = new StringBuilder();
            sb.Append(JS_SCRIPT_BEGIN);
            sb.AppendFormat("if(confirm('{0}'))", message);
            sb.Append("{");
            if(backHistory) {
                sb.Append(JS_HISTORY_BACK);
            }
            sb.AppendFormat(JS_WINDOW_OPEN_FORMAT, url, String.Empty, paras);
            sb.Append("}");
            sb.Append(JS_SCRIPT_END);
            response.Write(sb.ToString());
        }
    }
}

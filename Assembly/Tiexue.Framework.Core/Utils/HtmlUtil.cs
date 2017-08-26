using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Tiexue.Framework.Utils {
    public static class HtmlUtil {
        
        public static string UbbVideoToHtml(string encodedString) {
            const RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.Singleline;

            // WMV
            encodedString = Regex.Replace(encodedString, @"\[wmv(?:\s*)\]((.|\n)*?)\[/wmv(?:\s*)\]", "<embed src=\"$1\" width=\"314\" height=\"256\" autostart=\"0\"></embed>", options);
            encodedString = Regex.Replace(encodedString, @"\[wmv=((.|\n)*?)x((.|\n)*?)(?:\s*)\]((.|\n)*?)\[/wmv(?:\s*)\]", "<embed src=\"$5\" autostart=\"0\" width=\"$1\" height=\"$3\"></embed>", options);
            // mid
            encodedString = Regex.Replace(encodedString, @"\[mid(?:\s*)\]((.|\n)*?)\[/mid(?:\s*)\]", "<embed src=\"$1\" width=\"314\" height=\"45\" autostart=\"0\"></embed>", options);
            // ra
            encodedString = Regex.Replace(encodedString, @"\[ra(?:\s*)\]((.|\n)*?)\[/ra(?:\s*)\]", "<object classid=\"clsid:CFCDAA03-8BE4-11CF-B84B-0020AFBBCCFA\" id=\"RAOCX\" width=\"253\" height=\"60\"><param name=\"_ExtentX\" value=\"6694\"><param name=\"_ExtentY\" value=\"1588\"><param name=\"AUTOSTART\" value=\"0\"><param name=\"SHUFFLE\" value=\"0\"><param name=\"PREFETCH\" value=\"0\"><param name=\"NOLABELS\" value=\"0\"><param name=\"SRC\" value=\"$1\"><param name=\"CONTROLS\" value=\"StatusBar,ControlPanel\"><param name=\"LOOP\" value=\"0\"><param name=\"NUMLOOP\" value=\"0\"><param name=\"CENTER\" value=\"0\"><param name=\"MAINTAINASPECT\" value=\"0\"><param name=\"BACKGROUNDCOLOR\" value=\"#000000\"><embed src=\"$1\" width=\"253\" autostart=\"true\" height=\"60\"></embed></object>", options);
            // rm
            encodedString = Regex.Replace(encodedString, @"\[rm(?:\s*)\]((.|\n)*?)\[/rm(?:\s*)\]", "<object classid=\"clsid:CFCDAA03-8BE4-11cf-B84B-0020AFBBCCFA\" height=\"241\" id=\"Player\" width=\"316\" viewastext><param name=\"_ExtentX\" value=\"12726\"><param name=\"_ExtentY\" value=\"8520\"><param name=\"AUTOSTART\" value=\"0\"><param name=\"SHUFFLE\" value=\"0\"><param name=\"PREFETCH\" value=\"0\"><param name=\"NOLABELS\" value=\"0\"><param name=\"CONTROLS\" value=\"ImageWindow\"><param name=\"CONSOLE\" value=\"_master\"><param name=\"LOOP\" value=\"0\"><param name=\"NUMLOOP\" value=\"0\"><param name=\"CENTER\" value=\"0\"><param name=\"MAINTAINASPECT\" value=\"316\"><param name=\"BACKGROUNDCOLOR\" value=\"#000000\"></object><br><object classid=\"clsid:CFCDAA03-8BE4-11cf-B84B-0020AFBBCCFA\" height=\"32\" id=\"Player\" width=\"316\" VIEWASTEXT><param name=\"_ExtentX\" value=\"18256\"><param name=\"_ExtentY\" value=\"794\"><param name=\"AUTOSTART\" value=\"-1\"><param name=\"SHUFFLE\" value=\"0\"><param name=\"PREFETCH\" value=\"0\"><param name=\"NOLABELS\" value=\"0\"><param name=\"CONTROLS\" value=\"controlpanel\"><param name=\"CONSOLE\" value=\"_master\"><param name=\"LOOP\" value=\"0\"><param name=\"NUMLOOP\" value=\"0\"><param name=\"CENTER\" value=\"0\"><param name=\"MAINTAINASPECT\" value=\"0\"><param name=\"BACKGROUNDCOLOR\" value=\"#000000\"><param name=\"SRC\" value=\"$1\"></object>", options);
            encodedString = Regex.Replace(encodedString, @"\[rm=((.|\n)*?)x((.|\n)*?)(?:\s*)\]((.|\n)*?)\[/rm(?:\s*)\]", "<object classid=\"clsid:CFCDAA03-8BE4-11cf-B84B-0020AFBBCCFA\" height=\"$3\" id=\"Player\" width=\"$1\" viewastext><param name=\"_ExtentX\" value=\"12726\"><param name=\"_ExtentY\" value=\"8520\"><param name=\"AUTOSTART\" value=\"0\"><param name=\"SHUFFLE\" value=\"0\"><param name=\"PREFETCH\" value=\"0\"><param name=\"NOLABELS\" value=\"0\"><param name=\"CONTROLS\" value=\"ImageWindow\"><param name=\"CONSOLE\" value=\"_master\"><param name=\"LOOP\" value=\"0\"><param name=\"NUMLOOP\" value=\"0\"><param name=\"CENTER\" value=\"0\"><param name=\"MAINTAINASPECT\" value=\"$5\"><param name=\"BACKGROUNDCOLOR\" value=\"#000000\"></object><br><object classid=\"clsid:CFCDAA03-8BE4-11cf-B84B-0020AFBBCCFA\" height=\"32\" id=\"Player\" width=\"$1\" VIEWASTEXT><param name=\"_ExtentX\" value=\"18256\"><param name=\"_ExtentY\" value=\"794\"><param name=\"AUTOSTART\" value=\"-1\"><param name=\"SHUFFLE\" value=\"0\"><param name=\"PREFETCH\" value=\"0\"><param name=\"NOLABELS\" value=\"0\"><param name=\"CONTROLS\" value=\"controlpanel\"><param name=\"CONSOLE\" value=\"_master\"><param name=\"LOOP\" value=\"0\"><param name=\"NUMLOOP\" value=\"0\"><param name=\"CENTER\" value=\"0\"><param name=\"MAINTAINASPECT\" value=\"0\"><param name=\"BACKGROUNDCOLOR\" value=\"#000000\"><param name=\"SRC\" value=\"$5\"></object>", options);
            // Flash
            encodedString = Regex.Replace(encodedString, @"\[flash(?:\s*)\]((.|\n)*?)\[/flash(?:\s*)\]", "<embed src=\"$1\" quality=\"high\" pluginspage=\"http://www.macromedia.com/go/getflashplayer\" type=\"application/x-shockwave-flash\" width=\"550\" height=\"400\"></embed>", options);
            encodedString = Regex.Replace(encodedString, @"\[flash=((.|\n)*?)x((.|\n)*?)(?:\s*)\]((.|\n)*?)\[/flash(?:\s*)\]", "<embed src=\"$5\" quality=\"high\" pluginspage=\"http://www.macromedia.com/go/getflashplayer\" type=\"application/x-shockwave-flash\" width=\"$1\" height=\"$3\"></embed>", options);
            encodedString = Regex.Replace(encodedString, @"\[swf(?:\s*)\]((.|\n)*?)\[/swf(?:\s*)\]", "<embed src=\"$1\" quality=\"high\" pluginspage=\"http://www.macromedia.com/go/getflashplayer\" type=\"application/x-shockwave-flash\" width=\"550\" height=\"400\"></embed>", options);
            encodedString = Regex.Replace(encodedString, @"\[swf=((.|\n)*?)x((.|\n)*?)(?:\s*)\]((.|\n)*?)\[/swf(?:\s*)\]", "<embed src=\"$5\" quality=\"high\" pluginspage=\"http://www.macromedia.com/go/getflashplayer\" type=\"application/x-shockwave-flash\" width=\"$1\" height=\"$3\"></embed>", options);

            return encodedString;
        }

        public static string UbbToHtml(string encodedString) {
            // TODO move all this formatting to an external style

            const RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.Singleline;

            // Bold, Italic, Underline
            encodedString = Regex.Replace(encodedString, @"\[b(?:\s*)\]((.|\n)*?)\[/b(?:\s*)\]", "<b>$1</b>", options);
            encodedString = Regex.Replace(encodedString, @"\[i(?:\s*)\]((.|\n)*?)\[/i(?:\s*)\]", "<i>$1</i>", options);
            encodedString = Regex.Replace(encodedString, @"\[u(?:\s*)\]((.|\n)*?)\[/u(?:\s*)\]", "<u>$1</u>", options);
            // marquee
            encodedString = Regex.Replace(encodedString, @"\[fly(?:\s*)\]((.|\n)*?)\[/fly(?:\s*)\]", "<marquee>$1</marquee>", options);
            encodedString = Regex.Replace(encodedString, @"\[move(?:\s*)\]((.|\n)*?)\[/move(?:\s*)\]", "<marquee>$1</marquee>", options);
            // color
            encodedString = Regex.Replace(encodedString, @"\[color=""((.|\n)*?)(?:\s*)""\]((.|\n)*?)\[/color(?:\s*)\]", "<font color=\"$1\">$3</font>", options);
            encodedString = Regex.Replace(encodedString, @"\[color=((.|\n)*?)(?:\s*)\]((.|\n)*?)\[/color(?:\s*)\]", "<font color=\"$1\">$3</font>", options);
            // Left, Right, Center
            encodedString = Regex.Replace(encodedString, @"\[left(?:\s*)\]((.|\n)*?)\[/left(?:\s*)]", "<div style=\"text-align:left\">$1</div>", options);
            encodedString = Regex.Replace(encodedString, @"\[center(?:\s*)\]((.|\n)*?)\[/center(?:\s*)]", "<div style=\"text-align:center\">$1</div>", options);
            encodedString = Regex.Replace(encodedString, @"\[right(?:\s*)\]((.|\n)*?)\[/right(?:\s*)]", "<div style=\"text-align:right\">$1</div>", options);
            // Anchors
            encodedString = Regex.Replace(encodedString, @"\[url(?:\s*)\]www\.(.*?)\[/url(?:\s*)\]", "<a href=\"http://www.$1\" target=\"_blank\" title=\"$1\">$1</a>", options);
            encodedString = Regex.Replace(encodedString, @"\[url(?:\s*)\]((.|\n)*?)\[/url(?:\s*)\]", "<a href=\"$1\" target=\"_blank\" title=\"$1\">$1</a>", options);
            encodedString = Regex.Replace(encodedString, @"\[url=""((.|\n)*?)(?:\s*)""\]((.|\n)*?)\[/url(?:\s*)\]", "<a href=\"$1\" target=\"_blank\" title=\"$1\">$3</a>", options);
            encodedString = Regex.Replace(encodedString, @"\[url=((.|\n)*?)(?:\s*)\]((.|\n)*?)\[/url(?:\s*)\]", "<a href=\"$1\" target=\"_blank\" title=\"$1\">$3</a>", options);
            encodedString = Regex.Replace(encodedString, @"\[link(?:\s*)\]((.|\n)*?)\[/link(?:\s*)\]", "<a href=\"$1\" target=\"_blank\" title=\"$1\">$1</a>", options);
            encodedString = Regex.Replace(encodedString, @"\[link=((.|\n)*?)(?:\s*)\]((.|\n)*?)\[/link(?:\s*)\]", "<a href=\"$1\" target=\"_blank\" title=\"$1\">$3</a>", options);
            // Image
            encodedString = Regex.Replace(encodedString, @"\[img(?:\s*)\]((.|\n)*?)\[/img(?:\s*)\]", "<a target=\"_blank\" href=\"$1\"><img  src=\"$1\" border=\"0\" border=\"0\"  /></a>", options);
            // Color
            encodedString = Regex.Replace(encodedString, @"\[color=((.|\n)*?)(?:\s*)\]((.|\n)*?)\[/color(?:\s*)\]", "<span style=\"color=$1;\">$3</span>", options);
            // Email
            encodedString = Regex.Replace(encodedString, @"\[email(?:\s*)\]((.|\n)*?)\[/email(?:\s*)\]", "<a href=\"mailto:$1\">$1</a>", options);
            // Font size
            encodedString = Regex.Replace(encodedString, @"\[size=((.|\n)*?)(?:\s*)\]((.|\n)*?)\[/size(?:\s*)\]", "<span style=\"font-size:$1px\">$3</span>", options);
            encodedString = Regex.Replace(encodedString, @"\[face=((.|\n)*?)(?:\s*)\]((.|\n)*?)\[/face(?:\s*)\]", "<span style=\"font-family:$1;\">$3</span>", options);
            encodedString = Regex.Replace(encodedString, @"\[align=((.|\n)*?)(?:\s*)\]((.|\n)*?)\[/align(?:\s*)\]", "<div style=\"text-align:$1;\">$3</span>", options);
            encodedString = Regex.Replace(encodedString, @"\[float=((.|\n)*?)(?:\s*)\]((.|\n)*?)\[/float(?:\s*)\]", "<div style=\"float:$1;\">$3</div>", options);

            string sListFormat = "<ol class=\"anf_list\" style=\"list-style:{0};\">$1</ol>";
            // Lists
            encodedString = Regex.Replace(encodedString, @"\[\*(?:\s*)]\s*([^\[]*)", "<li>$1</li>", options);
            encodedString = Regex.Replace(encodedString, @"\[list(?:\s*)\]((.|\n)*?)\[/list(?:\s*)\]", "<ul class=\"anf_list\">$1</ul>", options);
            encodedString = Regex.Replace(encodedString, @"\[list=1(?:\s*)\]((.|\n)*?)\[/list(?:\s*)\]", string.Format(sListFormat, "decimal"), options);
            encodedString = Regex.Replace(encodedString, @"\[list=i(?:\s*)\]((.|\n)*?)\[/list(?:\s*)\]", string.Format(sListFormat, "lower-roman"));
            encodedString = Regex.Replace(encodedString, @"\[list=I(?:\s*)\]((.|\n)*?)\[/list(?:\s*)\]", string.Format(sListFormat, "upper-roman"));
            encodedString = Regex.Replace(encodedString, @"\[list=a(?:\s*)\]((.|\n)*?)\[/list(?:\s*)\]", string.Format(sListFormat, "lower-alpha"));
            encodedString = Regex.Replace(encodedString, @"\[list=A(?:\s*)\]((.|\n)*?)\[/list(?:\s*)\]", string.Format(sListFormat, "upper-alpha"));
            //other
            encodedString = Regex.Replace(encodedString, @"(\[SHADOW=*([0-9]*),*(#*[a-z0-9]*),*([0-9]*)\])(.[^\[]*)(\[\/SHADOW\])", "<table width=$2 ><tr><td style=\"filter:shadow(color=$3, strength=$4)\">$5</td></tr></table>", options);
            encodedString = Regex.Replace(encodedString, @"(\[GLOW=*([0-9]*),*(#*[a-z0-9]*),*([0-9]*)\])(.[^\[]*)(\[\/GLOW\])", "<table width=$2 ><tr><td style=\"filter:glow(color=$3, strength=$4)\">$5</td></tr></table>", options);

            //表情符号[em???] -> ??.gif
            encodedString = Regex.Replace(encodedString, @"(\[em(.[^\[]*)\])", "<img src=images/emotion/$2.gif border=0 align=middle>", options);
            return encodedString.Replace("\n", "<br/>");
            ;
        }
    }
}

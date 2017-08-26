using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Tiexue.Framework.Extension;

namespace NovelCollProjectutils
{
    public class HTMLUtil
    {
        /// <summary>
        /// 清除指定的html标签，不包含标签内的文字
        /// </summary>
        /// <param name="htmlString"></param>
        /// <param name="needClear"></param>
        /// <returns></returns>
        public static string ClearHtmlTag(string htmlString, params string[] needClear)
        {
            foreach (string s in needClear)
            {
                string regString = string.Format(@"</?{0}[^>]*>", s);
                Regex reg = new Regex(regString, RegexOptions.IgnoreCase);
                htmlString = reg.Replace(htmlString, "");
            }

            return htmlString;
        }


        /// <summary>
        /// 移除所有的html标签，不包含标签内的文字
        /// </summary>
        /// <param name="htmlString"></param>
        /// <returns></returns>
        public static string RemoveHtmlTag(string htmlString)
        {
            return Regex.Replace(htmlString, "<[^>]+>", "", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 移除所有的html标签，除了特殊的几个，不包含标签内的文字
        /// </summary>
        /// <param name="htmlString"></param>
        /// <param name="special"></param>
        /// <returns></returns>
        public static string RemoveHtmlTag(string htmlString, params string[] special)
        {
            string regString = "<(?!";
            bool first = true;
            foreach (string tag in special)
            {
                regString += first ? "" : "|";
                regString += @"\/?" + tag;
                first = false;
            }
            regString += @"|\/?!)[^<>]*>";

            return Regex.Replace(htmlString, regString, "", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 移除指定的html标签，并包含其中的文字
        /// </summary>
        /// <param name="htmlString"></param>
        /// <param name="removedTag"></param>
        /// <returns></returns>
        public static string RemoveHtmlContent(string htmlString, params string[] needRemoved)
        {
            foreach (string s in needRemoved)
            {
                string regString = string.Format(@"<{0}(\s[^>]*)?>((?!</{0}>).|\n)*</{0}>", s);
                Regex reg = new Regex(regString, RegexOptions.IgnoreCase);
                htmlString = reg.Replace(htmlString, "");
            }
            return htmlString;
        }

        /**
		 * 获取html节点的所有属性 
		 * @return 
		 * 
		 */
        public static Hashtable GetAttributes(string htmlnode)
        {
            //先去掉等号两边的空格
            htmlnode = Regex.Replace(htmlnode, @"\s*=\s*", "=", RegexOptions.Singleline);

            Hashtable result = new Hashtable();
            ArrayList atts = new ArrayList();
            MatchCollection matches = Regex.Matches(htmlnode, @"\s[\w\-:]+=", RegexOptions.IgnoreCase | RegexOptions.Singleline);

            foreach (Match m in matches)
            {
                string val = m.Groups[0].Value;
                atts.Add(val);
            }

            if (atts.Count == 0) return result;

            for (int j = 0; j < atts.Count; j++)
            {
                string name = ((string)atts[j]).Replace("=", "");
                name = name.Trim();
                name = name.ToLower();
                int pos = htmlnode.IndexOf((string)atts[j]) + ((string)atts[j]).Length;
                string ch = htmlnode.Substring(pos, 1);
                string symbol = null;
                string f = "";

                while (ch != symbol && pos < htmlnode.Length)
                {
                    if (symbol == null)
                    {
                        if (ch == "\"" || ch == "'")
                        {
                            symbol = ch;
                        }
                        else if (symbol != "\"" && symbol != "'")
                        {
                            symbol = " ";
                            f += ch;
                        }
                    }
                    else
                    {
                        f += ch;
                    }

                    ch = htmlnode.Substring(++pos, 1);
                    if (symbol == " " && ch == ">") break;
                }

                result[name] = f.Trim();

            }

            return result;
        }


        /// <summary>
        /// 移除html内容中的标签属性，除了特殊的
        /// </summary>
        /// <param name="htmlString"></param>
        /// <param name="special"></param>
        /// <returns></returns>
        public static string RemoveHtmlAttributes(string htmlString, params string[] special)
        {
            //找到所有的html标签
            Regex tagReg = new Regex(@"<\w{1,10}[^>]+>", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            MatchCollection tagMc = tagReg.Matches(htmlString);

            if (tagMc.Count > 0)
            {
                foreach (Match match in tagMc)
                {
                    string tag = Regex.Replace(match.Value, @"\s*=\s*", "=");
                    List<string> matches = new List<string>();

                    Regex attrReg = new Regex(@"\s[\w\-]+=""[^""]+""", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                    MatchCollection attrMc = attrReg.Matches(tag);
                    matches.AddRange(from Match match1 in attrMc select match1.Value);

                    attrReg = new Regex(@"\s[\w\-]+='[^']+'", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                    attrMc = attrReg.Matches(tag);
                    matches.AddRange(from Match match1 in attrMc select match1.Value);

                    foreach (string matchValue in matches)
                    {
                        string kv = matchValue.Trim();
                        if (special != null && special.Any(s => Regex.IsMatch(kv, $"^{s}=", RegexOptions.IgnoreCase))) continue;

                        tag = tag.Replace(kv, "");
                    }

                    htmlString = htmlString.Replace(match.Value, tag);
                }
            }

            return htmlString;
        }



        public static string GetAttribute(string htmlnode, string attributename)
        {
            attributename = attributename.ToLower();
            Hashtable obj = GetAttributes(htmlnode);

            if (obj.ContainsKey(attributename)) return (string)obj[attributename];

            return null;
        }

        public static string GetNodeName(string characters)
        {
            string nodename = "";

            if (characters.Substring(0, 1) == "<")
            {
                for (int j = 1; j < characters.Length; j++)
                {
                    string c = characters.Substring(j, 1);

                    if (c != ">" && c != " " && c != "/")
                    {
                        nodename += c;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {

            }

            return nodename;
        }

        /***********************************************************/
        public static string GetInnterHtml(string htmlString)
        {
            string nodename = GetNodeName(htmlString);
            if (nodename == "") return htmlString;

            string str = @"<\s*/?\s*" + nodename + "[^>]*>";
            Regex reg = new Regex(str, RegexOptions.IgnoreCase);
            MatchCollection matches = reg.Matches(htmlString);

            int qValue = 0;
            int startPos = 0;

            for (int i = 0; i < matches.Count; i++)
            {
                Match match = matches[i];
                Regex regg = new Regex(@"<\s*/[^>]+>");
                Match mm = regg.Match(match.Value);

                if (mm.Success)
                {
                    //闭合标签
                    qValue -= 1;
                    if (qValue == 0) return htmlString.Substring(startPos, match.Index - startPos);
                }
                else
                {
                    regg = new Regex(@"<[^>]+/\s*>");
                    mm = regg.Match(match.Value);
                    if (mm.Success)
                    {
                        //自封闭标签
                        return "";
                    }
                    else
                    {
                        //开始标签
                        qValue += 1;
                        startPos = match.Length;
                    }
                }
            }

            return htmlString.Substring(startPos);
        }

        public static string GetFullURL(string hostURL, string relativeURL, bool relativeRoot = false)
        {
            if (string.IsNullOrEmpty(hostURL) || string.IsNullOrEmpty(relativeURL))
                return null;

            if (Regex.IsMatch(relativeURL, "^data:image", RegexOptions.IgnoreCase))
                return null;

            if (Regex.IsMatch(relativeURL, "^javascript:", RegexOptions.IgnoreCase))
                return null;

            if (Regex.IsMatch(relativeURL, "^https?://", RegexOptions.IgnoreCase))
                return relativeURL;

            Uri host = new Uri(hostURL);
            if (relativeRoot)
            {
                string u = host.Scheme + "://" + host.Host;
                host = new Uri(u);
            }

            return new Uri(host, relativeURL).ToString();
        }


        public static bool IsCorrect(string url)
        {
            if (string.IsNullOrEmpty(url)) return false;
            if (Regex.IsMatch(url, "^data:image", RegexOptions.IgnoreCase)) return false;
            if (Regex.IsMatch(url, "^javascript:", RegexOptions.IgnoreCase)) return false;
            if (Regex.IsMatch(url, "^#+")) return false;

            return true;
        }

        public static string RemoveHtmlNote(string htmlString)
        {
            string regString = "<!--[^(-->)]*-->";
            return Regex.Replace(htmlString, regString, "", RegexOptions.IgnoreCase);
        }

    }
}

using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace NovelCollProjectutils
{
    class ParserUtil
    {
        public static float GetMatchingValueByCharacters(string input, List<string> characters)
        {
            if (characters == null || characters.Count == 0)return 1;

            float matchCount = 0;

            foreach (string str in characters)
            {
                if (str.IndexOf("REG:") == 0)
                {
                    Regex reg = new Regex(str.Substring(4), RegexOptions.IgnoreCase);
                    Match match = reg.Match(input);
                    if (match.Success)
                    {
                        matchCount += 1;
                    }
                }
                else if (str.IndexOf("NOTREG:") == 0)
                {
                    Regex reg = new Regex(str.Substring(7),RegexOptions.IgnoreCase);
                    Match match = reg.Match(input);
                    if (!match.Success)
                    {
                        matchCount += 1;
                    }
                }
                else if (str.IndexOf("NOT:") == 0)
                {
                    string s = str.Substring(4);
                    if (input.IndexOf(s) == -1)
                    {
                        matchCount += 1;
                    }
                }
                else
                {
                    if (input.IndexOf(str) > -1)
                    {
                        matchCount += 1;
                    }
                }
            }

            return matchCount/characters.Count;
        }

    }
}

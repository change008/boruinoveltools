using System.Collections.Generic;
using NovelCollProject.utils;
using NovelCollProjectutils;

namespace NovelCollProject.plugin
{
    class PageFeature
    {
        private Dictionary<PageTypeEnum, List<string>> _urlCharacters;
        private Dictionary<PageTypeEnum, List<string>> _htmlCharacters;

        public void InjectUrlRule(PageTypeEnum page, params string[] rules)
        {
            if(_urlCharacters == null)
                _urlCharacters = new Dictionary<PageTypeEnum, List<string>>();

            if(!_urlCharacters.ContainsKey(page))
                _urlCharacters[page] = new List<string>();

            foreach (string rule in rules)
            {
                if(_urlCharacters[page].IndexOf(rule) == -1)
                    _urlCharacters[page].Add(rule);
            }
        }

        public void InjectHtmlRule(PageTypeEnum page, params string[] rules)
        {
            if(_htmlCharacters == null)
                _htmlCharacters = new Dictionary<PageTypeEnum, List<string>>();

            if (!_htmlCharacters.ContainsKey(page))
                _htmlCharacters[page] = new List<string>();

            foreach (string rule in rules)
            {
                if (_htmlCharacters[page].IndexOf(rule) == -1)
                    _htmlCharacters[page].Add(rule);
            }
        }

        public PageTypeEnum MatchURL(string urlstring)
        {
            if(string.IsNullOrEmpty(urlstring)||_urlCharacters == null)return PageTypeEnum.NONE;

            float value = 0;
            PageTypeEnum pagetype = PageTypeEnum.NONE;

            foreach (var urlcharacter in _urlCharacters)
            {
                float temp = ParserUtil.GetMatchingValueByCharacters(urlstring, urlcharacter.Value);
                if (temp > value)
                {
                    value = temp;
                    pagetype = urlcharacter.Key;
                }

                if (temp == 1)
                    return pagetype;
            }

            return pagetype;
        }

        public PageTypeEnum MatchHtml(string htmlstring)
        {
            if (_htmlCharacters == null) return PageTypeEnum.NONE;

            float value = 0;
            PageTypeEnum pagetype = PageTypeEnum.NONE;

            foreach (var htmlcharacter in _htmlCharacters)
            {
                float temp = ParserUtil.GetMatchingValueByCharacters(htmlstring, htmlcharacter.Value);
                if (temp > value)
                {
                    value = temp;
                    pagetype = htmlcharacter.Key;
                }
            }

            return pagetype;
        }

    }
}

namespace Tiexue.Framework.Constants {
    public static class RegexRules {
        public const string UserName = @"^(?!_)(?!.*?_$)[a-zA-Z0-9_\u4e00-\u9fa5]{2,16}$";
        public const string Zip = @"[1-9]d{5}(?!d)";
        public const string Digital = @"^[+-]*\d+$";
        public const string QQ = @"^[1-9]*[1-9][0-9]*$";
        public const string IP = "^(\\d+)\\.(\\d+)\\.(\\d+)\\.(\\d+)$";
        public const string ChineseCharacters = @"[^a-z0-9\u4e00-\u9fa5\_]";
        public const string Mobile = @"^(1(([35][0-9])|(47)|[8][0126789]))\d{8}$";
        public const string Email = @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
        public const string Url = @"^(\w+):\/\/([^\:|\/]+)(\:\d*)?(.*\/)([^#|\?|\n]+)?(#.*)?(\?.*)?$";
        public const string DateTime = @"^(\d\d\d\d)[-./](1[0-2]|0?[1-9])[-./](0?[1-9]|[12][0-9]|3[01])$";
        public const string Phone = @"(^[0-9]{3,4}\-[0-9]{3,8}$)|(^[0-9]{3,8}$)|(^\([0-9]{3,4}\)[0-9]{3,8}$)|(^[0-9]{3,4}\-[0-9]{3,8}\-[0-9]{2,5}$)";

        public const string IPv4 = @"^(?:[0-9]{1,3}\.){3}[0-9]{1,3}$"; //@"^((25[0-5]|2[0-4]\d|[01]?\d\d?)\.){3}(25[0-5]|2[0-4]\d|[01]?\d\d?)$";
        // IPv4 Simple:  ^(?:[0-9]{1,3}\.){3}[0-9]{1,3}$
        // IPv4 Accurate: ^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$
        
    }

    public static class RegexRuleDescriptions {
        public const string UserName = "用户名只能包含中英文、数字及下划线，字符长度2到16，不能以下划线开头或结尾。";
    }
}

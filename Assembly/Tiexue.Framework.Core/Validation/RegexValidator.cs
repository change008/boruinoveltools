using System.Text.RegularExpressions;
using Tiexue.Framework.Constants;

namespace Tiexue.Framework.Validation {
    public static class RegexValidator {
        private static readonly Regex EmailRegex = new Regex(RegexRules.Email);
        private static readonly Regex MobileRegex = new Regex(RegexRules.Mobile);
        private static readonly Regex UserNameRegex = new Regex(RegexRules.UserName);

        public static bool ValidateEmail(string email) {
            return EmailRegex.IsMatch(email);
        }

        public static bool ValidateMobile(string mobile) {
            return MobileRegex.IsMatch(mobile);
        }

        public static bool ValidateUserName(string userName) {
            return UserNameRegex.IsMatch(userName);
        }
    }
}

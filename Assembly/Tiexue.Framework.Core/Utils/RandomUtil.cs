using System;
using System.Security.Cryptography;
using System.Text;
using Tiexue.Framework.Domain.DesignByContract;

namespace Tiexue.Framework.Utils {
    public static class RandomUtil {

        private static readonly string[] CharsPool = new[] {"1", "A", "B", "2", "C", "D", "E", "3", "F", "G", "H", "4", "I", "J", "K", "L", "5", "6", "M", "N", "P", "Q", "R", "9", "S", "T", "U", "V", "8", "W", "X", "Y", "7", "Z"};

        public static string GenerateRandomNumber(int length, int seed) {
            Check.Require(length > 0, "随机数字字符串长度必须大于0", new ArgumentException("length"));
            Random random = new Random(seed);
            StringBuilder sb = new StringBuilder(length);
            for(int i = 0; i < length; i++) {
                sb.Append(random.Next(0, 10));
            }
            return sb.ToString();
        }

        public static string GenerateCryptoRandomNumber(int length) {
            Check.Require(length > 0, "随机加密数字字符串长度必须大于0", new ArgumentException("length"));
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            byte[] bys = new byte[20];
            rng.GetBytes(bys);
            int data = Math.Abs(BitConverter.ToInt32(bys, 0)%(int) Math.Pow(10, length));
            return data.ToString(new String('0', length));
        }

        public static string GenerateRandomString(int length) {
            Check.Require(length > 0, "随机字符串长度必须大于0", new ArgumentException("length"));
            StringBuilder sb = new StringBuilder(length);
            Random random = new Random();
            for(int i = 0; i < length; i++) {
                int number = random.Next();
                number = number%34;
                sb.Append(CharsPool[number]);
            }
            return sb.ToString();
        }
    }
}

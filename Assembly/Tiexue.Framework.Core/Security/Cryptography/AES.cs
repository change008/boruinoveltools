using System;
using System.Security.Cryptography;
using System.Text;
using Tiexue.Framework.Domain.DesignByContract;

namespace Tiexue.Framework.Security.Cryptography {
    public class AES {
        /// <summary>
        /// AES加密用的向量
        /// </summary>
        private static readonly byte[] AES_IV = {0x61, 0x41, 0x6F, 0x53, 0x65, 0x79, 0x6F, 0x75, 0x6D, 0x79, 0x6E, 0x77, 0x6D, 0x72, 0x6E, 0x3F};

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="str">待加密字符串</param>
        /// <param name="key">密钥（64位、128位、256位）</param>
        /// <returns>经AES加密的字符串</returns>
        /// <exception cref="PreconditionException"></exception>
        public static string Encrypt(string str, string key) {
            Check.Require(str != null, "待加密字符串 str 不能为 null", new ArgumentNullException("str"));
            Check.Require(key != null, "密钥 key 不能为 null", new ArgumentNullException("key"));

            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] strBytes = Encoding.UTF8.GetBytes(str);

            var rijndaelManaged = new RijndaelManaged {Key = keyBytes, IV = AES_IV, Mode = CipherMode.CBC, Padding = PaddingMode.PKCS7};

            ICryptoTransform cryptoTransform = rijndaelManaged.CreateEncryptor();
            byte[] resultBytes = cryptoTransform.TransformFinalBlock(strBytes, 0, strBytes.Length);

            string encrypted = Convert.ToBase64String(resultBytes, 0, resultBytes.Length);

            return encrypted;
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="encrypted">待解密字符串</param>
        /// <param name="key">密钥（64位、128位、256位）</param>
        /// <returns>解密后的字符串</returns>
        /// <exception cref="PreconditionException"></exception>
        /// <exception cref="FormatException"></exception>
        public static string Decrypt(string encrypted, string key) {
            Check.Require(encrypted != null, "待解密字符串 encrypted 不能为 null", new ArgumentNullException("encrypted"));
            Check.Require(key != null, "密钥 key 不能为 null", new ArgumentNullException("key"));

            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] encryptedBytes = Convert.FromBase64String(encrypted);

            var rijndaelManaged = new RijndaelManaged {Key = keyBytes, IV = AES_IV, Mode = CipherMode.CBC, Padding = PaddingMode.PKCS7};

            ICryptoTransform cTransform = rijndaelManaged.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

            string decrypted = Encoding.UTF8.GetString(resultArray);

            return decrypted;
        }
    }
}

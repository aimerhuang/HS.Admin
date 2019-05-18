using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.Customs
{
    public class AESHelper
    {
        /// <summary>
        ///  密钥
        /// </summary>
        private static String Key = ApiConfig.GuangZhouCustomsKey;

        /// <summary>
        ///  AES 解密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Decrypt(string str)
        {
            if (string.IsNullOrEmpty(str)) return null;
            Byte[] toEncryptArray = Convert.FromBase64String(str);

            System.Security.Cryptography.RijndaelManaged rm = new System.Security.Cryptography.RijndaelManaged
            {
                Key = Convert.FromBase64String(Key),
                Mode = System.Security.Cryptography.CipherMode.ECB,
                Padding = System.Security.Cryptography.PaddingMode.PKCS7
            };

            System.Security.Cryptography.ICryptoTransform cTransform = rm.CreateDecryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Encoding.UTF8.GetString(resultArray);
        }

        /// <summary>
        /// AES 加密
        /// </summary>
        /// <param name="toEncrypt"></param>
        /// <returns></returns>
        public static string Encrypt(string toEncrypt)
        {
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);

            System.Security.Cryptography.RijndaelManaged rm = new System.Security.Cryptography.RijndaelManaged
            {
                Key = Convert.FromBase64String(Key),
                Mode = System.Security.Cryptography.CipherMode.ECB,
                Padding = System.Security.Cryptography.PaddingMode.PKCS7
            };
            System.Security.Cryptography.ICryptoTransform cTransform = rm.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

    }
}
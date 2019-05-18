using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Hyt.Util
{
    /// <summary>
    /// 加密解密工具
    /// </summary>
    /// <remarks>2013－06-08 罗雄伟 重构</remarks>
    public class EncryptionUtil
    {
        /// <summary>
        /// 对字符串进行MD5加密
        /// </summary>
        /// <param name="str">要加密的字符串</param>
        /// <returns>加密后的小写字符串</returns>
        /// <remarks>2013－06-08 罗雄伟 重构</remarks>
        public static string EncryptWithMd5(String str)
        {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "md5").ToLower();
        }

        /// <summary>
        /// 对字符串附加随机字符进行MD5加密
        /// </summary>
        /// <param name="str">要加密的字符串</param>
        /// <returns>加密后的小写字符串</returns>
        /// <remarks>2013－06-08 罗雄伟 重构</remarks>
        public static string EncryptWithMd5AndSalt(string str)
        {
            string password = "";
            password = new Random().Next(10000000, 99999999).ToString();
            string salt = EncryptWithMd5(password).Substring(0, 2);
            password = EncryptWithMd5(salt + str) + ":" + salt;
            return password;
        }

        /// <summary>
        /// 对加密字符串（字符串附加随机字符进行MD5加密后的字符串）进行验证
        /// </summary>
        /// <param name="unencrypted">明文</param>
        /// <param name="encrypted">密文</param>
        /// <returns>验证结果</returns>
        /// <remarks>2013－06-08 罗雄伟 重构</remarks>
        public static bool VerifyCiphetrextWithMd5AndSalt(string unencrypted, string encrypted)
        {
            if (!string.IsNullOrEmpty(unencrypted) && !string.IsNullOrEmpty(encrypted))
            {
                string[] stack = encrypted.Split(':');
                if (stack.Length != 2) return false;
                if (EncryptWithMd5(stack[1] + unencrypted) == stack[0])
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>  
        /// 给一个字符串进行MD5加密  
        /// </summary>  
        /// <param name="strText">待加密字符串</param>  
        /// <returns>加密后的字符串</returns>
        /// <remarks>2013－06-08 罗雄伟 重构</remarks>
        public static string MD5Encrypt(string strText)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(strText));

            string outString = "";
            for (int i = 0; i < result.Length; i++)
            {
                outString += result[i].ToString("x2");
            }

            return outString;

        }

        #region 洋车站加密文件
        private  static  int MAX_ENCRYPT_BLOCK = 117;

	    /** */
	    /**
	     * RSA最大解密密文大小
	     */
	    private static  int MAX_DECRYPT_BLOCK = 128;

	    /**
	     * 使用公钥加密字符串
	     * 
	     * @param content
	     *            需要加密的内容
	     * @param pulicKey
	     *            加密时需要的公钥
	     * @return
	     */
        public static String RasEncreptData(String content, String pulicKey)
        {
            /* Create the cipher */
            int inputLen = Encoding.UTF8.GetBytes(content).Length;
            int offSet = 0;
            byte[] cache = null;
            int i = 0;
            try
            {
                MemoryStream mStream = new MemoryStream();
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                byte[] cipherbytes;
                rsa.FromXmlString(pulicKey);
                cipherbytes = rsa.Encrypt(Encoding.UTF8.GetBytes(content), true);

                while (inputLen - offSet > 0)
                {
                    if (inputLen - offSet > MAX_ENCRYPT_BLOCK)
                    {
                        //cache = cipherbytes.(Encoding.UTF8.GetBytes(content),
                        //        offSet, MAX_ENCRYPT_BLOCK);
                        mStream.Write(cipherbytes,
                                offSet, MAX_ENCRYPT_BLOCK);
                    }
                    else
                    {
                        //cache = cipherbytes.doFinal(Encoding.UTF8.GetBytes(content),
                        //        offSet, inputLen - offSet);
                        mStream.Write(cipherbytes,
                                offSet, inputLen - offSet);
                    }
                    //mStream.Write(cache, 0, cache.Length);
                    i++;
                    offSet = i * MAX_ENCRYPT_BLOCK;
                }
                byte[] encryptedData = mStream.ToArray();
                mStream.Close();
                return Convert.ToBase64String(encryptedData);
            }
            catch
            {

            }
            return null;
        }
        public static string GetMd5Encode32(string encryptDataInfo)
        {
            byte[] result = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5"))
                .ComputeHash(Encoding.UTF8.GetBytes(encryptDataInfo));
            StringBuilder output = new StringBuilder(32);
            for (int i = 0; i < result.Length; i++)
            {
                // convert from hexa-decimal to character  
                output.Append((result[i]).ToString("x2", System.Globalization.CultureInfo.InvariantCulture));
            }
            return output.ToString();  
        }
        #endregion

        #region 简单对称加密

        //对称加密密钥 
        //azjmerbv  0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF
        //vzjmerbv  0xEF, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF
        private static byte[] Keys = { 0xEF, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        public static string EncryptKey = "vzjmerbv";

        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="encryptString">待加密的字符串</param>
        /// <param name="encryptKey">加密密钥,要求为8位</param>
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>
        /// <remarks>2014-1-21 黄波 创建</remarks>
        public static string EncryptDES(string encryptString, string encryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch
            {
                return encryptString;
            }
        }

        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param>
        /// <returns>解密成功返回解密后的字符串，失败返源串</returns>
        /// <remarks>2014-1-21 黄波 创建</remarks>
        public static string DecryptDES(string decryptString, string decryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return decryptString;
            }
        }

        #endregion

        
    }
}

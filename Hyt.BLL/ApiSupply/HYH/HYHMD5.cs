using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace Hyt.BLL.ApiSupply.HYH
{
    public class HYHMD5
    {
        public HYHMD5()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 签名字符串
        /// </summary>
        /// <param name="prestr">需要签名的字符串</param>
        /// <param name="key">密钥</param>
        /// <param name="_input_charset">编码格式</param>
        /// <returns>签名结果</returns>
        public static string Sign(SortedDictionary<string, string> sortedDic, string key, string InputCharset, string SignType)
        {
            System.Text.StringBuilder sign = new System.Text.StringBuilder();
            //拼接字符串,为null和空字符串的不拼接
            System.Text.StringBuilder preSign = new System.Text.StringBuilder();

            //返回的待签名字符串
            foreach (KeyValuePair<string, string> keyValePair in sortedDic)
            {
                if (keyValePair.Value != null && keyValePair.Value != "" && keyValePair.Key.ToLower() != "sign")
                {
                    preSign.Append(keyValePair.Key + "=" + keyValePair.Value + "&");
                }
            }

            //去掉最後一個&字符
            preSign.Remove(preSign.ToString().Length - 1, 1);

            //链接上安全校验码
            preSign.Append(key);

            //生成MD5 sign
            if (SignType.ToUpper() == "MD5")
            {
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] t = md5.ComputeHash(System.Text.Encoding.GetEncoding(InputCharset).GetBytes(preSign.ToString()));
                for (int i = 0; i < t.Length; i++)
                {
                    sign.Append(t[i].ToString("x").PadLeft(2, '0'));
                }
            }

            return sign.ToString();
        }

        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="prestr">需要签名的字符串</param>
        /// <param name="sign">签名结果</param>
        /// <param name="key">密钥</param>
        /// <param name="_input_charset">编码格式</param>
        /// <returns>验证结果</returns>
        public static bool Verify(SortedDictionary<string, string> prestr, string sign, string key, string _input_charset, string SignType)
        {
            string mysign = Sign(prestr, key, _input_charset, SignType);
            if (mysign == sign)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

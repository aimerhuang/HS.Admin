using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Hyt.Util
{
    /// <summary>
    /// 随机数工具类
    /// </summary>
    /// <remarks>2014-1-21 黄波 创建</remarks>
    public sealed class RandomString
    {

        public const string myVersion = "1.2";

        private static readonly int defaultLength = 8;

        /// <summary>
        /// 获取随机基数
        /// </summary>
        /// <returns>基数</returns>
        /// <remarks>2014-1-21 黄波 创建</remarks>
        public static int GetNewSeed()
        {
            byte[] rndBytes = new byte[4];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(rndBytes);
            return BitConverter.ToInt32(rndBytes, 0);
        }

        /// <summary>
        /// 生成随机字符
        /// </summary>
        /// <param name="strLen">字符长度</param>
        /// <returns>随机字符</returns>
        /// <remarks>2014-1-21 黄波 创建</remarks>
        private static string BuildRndCodeAll(int strLen)
        {
            System.Random RandomObj = new System.Random(GetNewSeed());
            string buildRndCodeReturn = null;
            for (int i = 0; i < strLen; i++)
            {
                buildRndCodeReturn += (char)RandomObj.Next(33, 125);
            }
            return buildRndCodeReturn;
        }

        /// <summary>
        /// 获取随机字符
        /// </summary>
        /// <returns>随机字符</returns>
        /// <remarks>2014-1-21 黄波 创建</remarks>
        public static string GetRndStrOfAll()
        {
            return BuildRndCodeAll(defaultLength);
        }

        /// <summary>
        /// 获取随机字符
        /// </summary>
        /// <param name="LenOf">字符长度</param>
        /// <returns>随机字符</returns>
        /// <remarks>2014-1-21 黄波 创建</remarks>
        public static string GetRndStrOfAll(int LenOf)
        {
            return BuildRndCodeAll(LenOf);
        }

        private static string sCharLow = "abcdefghijklmnopqrstuvwxyz";
        private static string sCharUpp = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private static string sNumber = "0123456789";

        /// <summary>
        /// 生成随机字串
        /// </summary>
        /// <param name="StrOf">随机字串来源</param>
        /// <param name="strLen">随机字串长度</param>
        /// <returns>随机字串</returns>
        /// <remarks>2014-1-21 黄波 创建</remarks>
        private static string BuildRndCodeOnly(string StrOf, int strLen)
        {
            System.Random RandomObj = new System.Random(GetNewSeed());
            string buildRndCodeReturn = null;
            for (int i = 0; i < strLen; i++)
            {
                buildRndCodeReturn += StrOf.Substring(RandomObj.Next(0, StrOf.Length - 1), 1);
            }
            return buildRndCodeReturn;
        }

        /// <summary>
        /// 获取随机字串
        /// </summary>
        /// <returns>随机字串</returns>
        /// <remarks>2014-1-21 黄波 创建</remarks>
        public static string GetRndStrOnlyFor()
        {
            return BuildRndCodeOnly(sCharLow + sNumber, defaultLength);
        }

        /// <summary>
        /// 获取随机字串
        /// </summary>
        /// <param name="LenOf">随机字串长度</param>
        /// <returns>随机字串</returns>
        /// <remarks>2014-1-21 黄波 创建</remarks>
        public static string GetRndStrOnlyFor(int LenOf)
        {
            return BuildRndCodeOnly(sCharLow + sNumber, LenOf);
        }

        /// <summary>
        /// 获取默认长度的随机字串
        /// </summary>
        /// <param name="bUseUpper">是否包含大写字母</param>
        /// <param name="bUseNumber">是否包含数字</param>
        /// <returns>默认长度的随机字串</returns>
        /// <remarks>2014-1-21 黄波 创建</remarks>
        public static string GetRndStrOnlyFor(bool bUseUpper, bool bUseNumber)
        {
            string strTmp = sCharLow;
            if (bUseUpper) strTmp += sCharUpp;
            if (bUseNumber) strTmp += sNumber;

            return BuildRndCodeOnly(strTmp, defaultLength);
        }

        /// <summary>
        /// 获取指定长度的随机字串
        /// </summary>
        /// <param name="LenOf">随机字串长度</param>
        /// <param name="bUseUpper">是否包含大写字母</param>
        /// <param name="bUseNumber">是否包含数字</param>
        /// <returns>指定长度的随机字串</returns>
        /// <remarks>2014-1-21 黄波 创建</remarks>
        public static string GetRndStrOnlyFor(int LenOf, bool bUseUpper, bool bUseNumber)
        {
            string strTmp = sCharLow;
            if (bUseUpper) strTmp += sCharUpp;
            if (bUseNumber) strTmp += sNumber;

            return BuildRndCodeOnly(strTmp, LenOf);
        }

        /// <summary>
        /// 获取指定长度的随机字串
        /// </summary>
        /// <param name="LenOf">随机字串长度</param>
        /// <param name="bCharLow">是否包含小写字母</param>
        /// <param name="bUseUpper">是否包含大写字母</param>
        /// <param name="bUseNumber">是否包含数字</param>
        /// <returns>默认长度的随机字串</returns>
        /// <remarks>2014-1-21 黄波 创建</remarks>
        public static string GetRndStrOnlyFor(int LenOf, bool bCharLow, bool bUseUpper, bool bUseNumber)
        {
            string strTmp = "";
            if (bCharLow) strTmp += sCharLow;
            if (bUseUpper) strTmp += sCharUpp;
            if (bUseNumber) strTmp += sNumber;

            return BuildRndCodeOnly(strTmp, LenOf);
        }
    }
}

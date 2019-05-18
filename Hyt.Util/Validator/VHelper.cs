using System;
using System.Collections.Generic;
using System.Linq;
using Hyt.Util.Validator.Rule;
using System.Text.RegularExpressions;

namespace Hyt.Util.Validator
{
    /// <summary>
    /// 验证类型
    /// </summary>
    /// <remarks>2013-12-30 黄志勇 注释</remarks>
    public enum VType
    {
        /// <summary>
        /// 邮箱
        /// </summary>
        Email,

        /// <summary>
        /// 身份证号码
        /// </summary>
        CardNO,

        /// <summary>
        /// 手机号码
        /// </summary>
        Mobile,

        /// <summary>
        /// 电话号码
        /// </summary>
        Telephone,

        /// <summary>
        /// QQ号码
        /// </summary>
        QQ,

        /// <summary>
        /// IP地址
        /// </summary>
        IP,

        /// <summary>
        /// 邮政号码
        /// </summary>
        Postcode,

        /// <summary>
        /// 帐号
        /// </summary>
        Account
    };

    /// <summary>
    /// 验证规则帮助类
    /// </summary>
    /// <remarks>2013-12-30 黄志勇 注释</remarks>
    public static class VHelper
    {
        /// <summary>
        /// 公共验证方法
        /// </summary>
        /// <param name="m">字符串</param>
        /// <param name="v">验证类型</param>
        /// <returns>有效：true 无效：false</returns>
        /// <remarks>2013-12-30 黄志勇 注释</remarks>
        public static bool Do(string m, VType v)
        {
            bool check = false;
            if (!string.IsNullOrWhiteSpace(m))
            {
                switch (v)
                {
                    case VType.Email:
                        check = IsEmail(m);
                        break;
                    case VType.CardNO:
                        check = IsCardNO(m);
                        break;
                    case VType.Mobile:
                        check = IsMobile(m);
                        break;
                    case VType.IP:
                        check = IsIP(m);
                        break;
                    case VType.QQ:
                        check = IsQQ(m);
                        break;
                    case VType.Telephone:
                        check = IsTelephone(m);
                        break;
                    case VType.Postcode:
                        check = IsPostcode(m);
                        break;
                    case VType.Account:
                        check = IsAccount(m);
                        break;
                }
            }
            return check;
        }

        /// <summary>
        /// 验证Email的有效性
        /// </summary>
        /// <param name="m_email">email</param>
        /// <returns>有效：true 无效：false</returns>
        /// <remarks>2013-12-30 黄志勇 注释</remarks>
        private static bool IsEmail(string m_email)
        {
            return Regex.IsMatch(m_email, "^\\w+((-\\w+)|(\\.\\w+))*\\@[A-Za-z0-9]+((\\.|-)[A-Za-z0-9]+)*\\.[A-Za-z0-9]+$");

        }

        /// <summary>
        /// 验证手机
        /// </summary>
        /// <param name="m_mobile">手机号</param>
        /// <returns>有效：true 无效：false</returns>
        /// <remarks>2013-12-30 黄志勇 注释</remarks>
        private static bool IsMobile(string m_mobile)
        {
            return Regex.IsMatch(m_mobile, @"^(1\d{10})$");

        }

        /// <summary>
        /// 验证身份证
        /// </summary>
        /// <param name="cardNo">身份证号</param>
        /// <returns>有效：true 无效：false</returns>
        /// <remarks>2013-12-30 黄志勇 注释</remarks>
        private static bool IsCardNO(string cardNo)
        {
            return Regex.IsMatch(cardNo, "^[1-9]([0-9]{14}|[0-9]{17})$");

        }

        /// <summary>
        /// 验证座机
        /// </summary>
        /// <param name="m_telephone">座机号</param>
        /// <returns>有效：true 无效：false</returns>
        /// <remarks>2013-12-30 黄志勇 注释</remarks>
        private static bool IsTelephone(string m_telephone)
        {
            return Regex.IsMatch(m_telephone, "^(([0\\+]\\d{2,3}-)?(0\\d{2,3})-)?(\\d{7,8})(-(\\d{3,}))?$");

        }

        /// <summary>
        /// 验证QQ
        /// </summary>
        /// <param name="m_qq">qq</param>
        /// <returns>有效：true 无效：false</returns>
        /// <remarks>2013-12-30 黄志勇 注释</remarks>
        private static bool IsQQ(string m_qq)
        {
            return Regex.IsMatch(m_qq, "[1-9][0-9]{4,}");

        }

        /// <summary>
        /// 验证IP
        /// </summary>
        /// <param name="m_ip">IP</param>
        /// <returns>有效：true 无效：false</returns>
        /// <remarks>2013-12-30 黄志勇 注释</remarks>
        private static bool IsIP(string m_ip)
        {
            return Regex.IsMatch(m_ip, "^(25[0-5]|2[0-4]\\d|[0-1]\\d{2}|[1-9]?\\d)\\.(25[0-5]|2[0-4]\\d|[0-1]\\d{2}|[1-9]?\\d)\\.(25[0-5]|2[0-4]\\d|[0-1]\\d{2}|[1-9]?\\d)\\.(25[0-5]|2[0-4]\\d|[0-1]\\d{2}|[1-9]?\\d)$");

        }

        /// <summary>
        /// 验证邮编
        /// </summary>
        /// <param name="m_postcode">邮编</param>
        /// <returns>有效：true 无效：false</returns>
        /// <remarks>2013-12-30 黄志勇 注释</remarks>
        public static bool IsPostcode(string m_postcode)
        {
            return Regex.IsMatch(m_postcode, "^[1-9][0-9]{5}$");

        }

        /// <summary>
        /// 验证输入的用户账号，是否是以字母开头
        /// </summary>
        /// <param name="m_account">用户帐号</param>
        /// <returns>有效：true 无效：false</returns>
        /// <remarks>2013-12-30 黄志勇 注释</remarks>
        private static bool IsAccount(string m_account)
        {
            return Regex.IsMatch(m_account, "^[a-zA-Z][a-zA-Z0-9_]{4,15}$");
        }

        /// <summary>
        /// 使用验证规则进行验证
        /// </summary>
        /// <param name="rules">验证规则</param>
        /// <returns>有效：true 无效：false</returns>
        /// <remarks>2013-12-30 黄志勇 注释</remarks>
        public static VResult ValidatorRule(params IRule[] rules)
        {
            return ValidatorRule(true, rules);
        }

        /// <summary>
        /// 使用验证规则进行验证
        /// </summary>
        /// <param name="one">true：遇到false则中断。false:遇到false继续。默认为true</param>
        /// <param name="rules">验证规则</param>
        /// <returns>验证结果</returns>
        /// <remarks>2013-12-30 黄志勇 注释</remarks>
        public static VResult ValidatorRule(bool one, params IRule[] rules)
        {
            VResult result = new VResult();
            foreach (IRule rule in rules)
            {
                if (rule.Valid() == false)
                {
                    result.IsPass = false;
                    result.Message = result.Message + rule.Message + Environment.NewLine;
                    rule.OnFalse();
                    if (one) break;
                }
            }
            return result;
        }

        /// <summary>
        /// 使用验证规则进行验证
        /// </summary>
        /// <param name="rules">验证规则</param>
        /// <returns>有效：true 无效：false</returns>
        /// <remarks>2013-12-30 黄志勇 注释</remarks>
        public static VResult ValidatorRule(IList<IRule> rules)
        {
            return ValidatorRule(rules.ToArray());
        }

        /// <summary>
        /// 使用验证规则进行验证
        /// </summary>
        /// <param name="one">true：遇到false则中断。false:遇到false继续。默认为true</param>
        /// <param name="rules">验证规则</param>
        /// <returns>有效：true 无效：false</returns>
        /// <remarks>2013-12-30 黄志勇 注释</remarks>
        public static VResult ValidatorRule(bool one, IList<IRule> rules)
        {
            return ValidatorRule(one, rules.ToArray());
        }

    }
}
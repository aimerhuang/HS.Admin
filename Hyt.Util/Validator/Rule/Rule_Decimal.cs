using System;

namespace Hyt.Util.Validator.Rule
{   
    /// <summary>
    /// 验证字符串是否Decimal类型
    /// </summary>
    /// <remarks>2013-12-30 黄志勇 注释</remarks>
    public class Rule_Decimal : IRule
    {
        private string m_decimal;

        /// <summary>
        /// 验证字符串是否Decimal类型
        /// </summary>
        /// <param name="s">Decimal类型的字符串.</param>
        /// <param name="message">验证失败的提示消息.</param>
        /// <remarks>2013-12-30 黄志勇 注释</remarks>
        public Rule_Decimal(string s, string message = "字符串不是有效的Decimal类型")
        {
            m_decimal = s;
            Message = message;
        }

        /// <summary>
        /// 验证是否有效
        /// </summary>
        /// <param></param>
        /// <returns>有效：true 无效：false</returns>
        /// <remarks>2013-12-30 黄志勇 注释</remarks>
        public override bool Valid()
        {
            if (string.IsNullOrWhiteSpace(m_decimal)) return false;
            decimal de;
            return Decimal.TryParse(m_decimal, out de);
        }

    }
}

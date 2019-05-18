using System;
using System.Text.RegularExpressions;

namespace Hyt.Util.Validator.Rule
{
    /// <summary>
    /// 验证规则 身份证
    /// </summary>
    /// <remarks>2013-12-30 黄志勇 注释</remarks>
    public class Rule_IDCard : IRule
    {
        private string m_idCard;

        /// <summary>
        /// 验证规则 身份证
        /// </summary>
        /// <param name="idCard">身份证</param>
        /// <param name="message">验证失败的提示消息</param>
        /// <remarks>2013-12-30 黄志勇 注释</remarks>
        public Rule_IDCard(string idCard, string message = "身份证格式不正确")
        {
            m_idCard = idCard;
            Message = message;
        }

        /// <summary>
        /// 验证是否有效
        /// </summary>
        /// <returns></returns>
        /// <returns>有效：true 无效：false</returns>
        /// <remarks>2013-12-30 黄志勇 注释</remarks>
        public override bool Valid()
        {
            if (String.IsNullOrEmpty(m_idCard)) return false;
            return Regex.IsMatch(m_idCard, "^[1-9]([0-9]{14}|[0-9]{17})$");
        }
    }
}

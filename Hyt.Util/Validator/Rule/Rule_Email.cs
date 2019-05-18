using System.Text.RegularExpressions;

namespace Hyt.Util.Validator.Rule
{
    /// <summary>
    /// 验证规则 email格式的字符串
    /// </summary>
    /// <remarks>2013-12-30 黄志勇 注释</remarks>
    public class Rule_Email :IRule
    {
        private string m_email;

        /// <summary>
        /// 验证规则 email格式的字符串
        /// </summary>
        /// <param name="email">email字符串.</param>
        /// <param name="message">验证失败的提示消息.</param>
        /// <remarks>2013-12-30 黄志勇 注释</remarks>
        public Rule_Email(string email, string message = "电子邮件格式不正确")
        {
            m_email = email;
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
            if (string.IsNullOrWhiteSpace(m_email)) return false;
            return Regex.IsMatch(m_email, "^\\w+((-\\w+)|(\\.\\w+))*\\@[A-Za-z0-9]+((\\.|-)[A-Za-z0-9]+)*\\.[A-Za-z0-9]+$");
        }
    }
}

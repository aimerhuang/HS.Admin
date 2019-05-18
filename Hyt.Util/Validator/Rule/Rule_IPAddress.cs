using System.Text.RegularExpressions;

namespace Hyt.Util.Validator.Rule
{
    /// <summary>
    /// 验证规则 IP地址
    /// </summary>
    /// <remarks>2013-12-30 黄志勇 注释</remarks>
    public class Rule_IPAddress : IRule
    {
        private string m_ip;

        /// <summary>
        /// 验证规则 IP地址
        /// </summary>
        /// <param name="ip">IP地址.</param>
        /// <param name="message">验证失败的提示消息.</param>
        /// <remarks>2013-12-30 黄志勇 注释</remarks>
        public Rule_IPAddress(string ip, string message = "IP地址格式不正确")
        {
            m_ip = ip;
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
            if (string.IsNullOrEmpty(m_ip)) return false;
            return Regex.IsMatch(m_ip, "^(25[0-5]|2[0-4]\\d|[0-1]\\d{2}|[1-9]?\\d)\\.(25[0-5]|2[0-4]\\d|[0-1]\\d{2}|[1-9]?\\d)\\.(25[0-5]|2[0-4]\\d|[0-1]\\d{2}|[1-9]?\\d)\\.(25[0-5]|2[0-4]\\d|[0-1]\\d{2}|[1-9]?\\d)$");
        }
    }
}

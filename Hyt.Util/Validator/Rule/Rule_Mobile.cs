using System.Text.RegularExpressions;

namespace Hyt.Util.Validator.Rule
{
    /// <summary>
    /// 验证规则 手机号码
    /// </summary>
    /// <remarks>2013-12-30 黄志勇 注释</remarks>
    public class Rule_Mobile : IRule 
    {
        private string m_mobile;
        /// <summary>
        /// 验证规则 手机号码
        /// </summary>
        /// <param name="mobile">手机号码.</param>
        /// <param name="message">验证失败的提示消息.</param>
        /// <remarks>2013-12-30 黄志勇 注释</remarks>
        public Rule_Mobile(string mobile, string message = "手机号码格式不正确")
        {
            m_mobile = mobile;
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
            if (string.IsNullOrWhiteSpace(m_mobile)) return false;
            return Regex.IsMatch(m_mobile, "^(\\+86)?1(\\d{10})$");
        }
    }
}

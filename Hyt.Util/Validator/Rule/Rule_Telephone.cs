using System.Text.RegularExpressions;

namespace Hyt.Util.Validator.Rule
{
    /// <summary>
    /// 验证规则 电话号码(包括验证国内区号,国际区号,分机号)
    /// </summary>
    /// <remarks>2013-12-30 黄志勇 注释</remarks>
    public class Rule_Telephone : IRule
    {
        private string m_tel;

        /// <summary>
        /// 验证规则 电话号码(包括验证国内区号,国际区号,分机号)
        /// </summary>
        /// <param name="tel">电话号码字符串.</param>
        /// <param name="message">验证失败的提示消息.</param>
        /// <remarks>2013-12-30 黄志勇 注释</remarks>
        public Rule_Telephone(string tel, string message = "电话号码格式不正确")
        {
            m_tel = tel;
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
            if (string.IsNullOrWhiteSpace(m_tel)) return false;
            return Regex.IsMatch(m_tel, "^(([0\\+]\\d{2,3}-)?(0\\d{2,3})-)?(\\d{7,8})(-(\\d{3,}))?$");
        }
    }
}

using System.Text.RegularExpressions;

namespace Hyt.Util.Validator.Rule
{
    /// <summary>
    /// 验证QQ规则
    /// </summary>
    /// <remarks>2013-12-30 黄志勇 注释</remarks>
    public class Rule_QQ : IRule
    {
        private string qq;

        /// <summary>
        /// 验证规则 qq格式的字符串
        /// </summary>
        /// <param name="qq">qq字符串.</param>
        /// <param name="message">验证失败的提示消息.</param>
        /// <remarks>2013-12-30 黄志勇 注释</remarks>
        public Rule_QQ(string qq, string message = "QQ格式不正确")
        {
            this.qq = qq;
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
            if (string.IsNullOrWhiteSpace(qq)) return false;
            return Regex.IsMatch(qq, "[1-9][0-9]{4,}");
        }
    }
}

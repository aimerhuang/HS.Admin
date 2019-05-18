
using System.Text.RegularExpressions;

namespace Hyt.Util.Validator.Rule
{
    /// <summary>
    /// 验证字母规则
    /// </summary>
    /// <remarks>2013－06-09 黄志勇 创建</remarks>
    public class Rule_Letter : IRule
    {
        private string letter;

        /// <summary>
        /// 验证规则 字母格式的字符串
        /// </summary>
        /// <param name="letter">字母字符串.</param>
        /// <param name="message">验证失败的提示消息.</param>
        /// <remarks>2013-12-30 黄志勇 注释</remarks>
        public Rule_Letter(string letter="", string message = "字母格式不正确")
        {
            this.letter = letter;
            Message = message;
        }

        /// <summary>
        /// 验证是否有效
        /// </summary>
        /// <returns>有效：true 无效：false</returns>
        /// <remarks>2013-12-30 黄志勇 注释</remarks>
        public override bool Valid()
        {
            if (string.IsNullOrWhiteSpace(letter)) return false;
            return Regex.IsMatch(letter, @"^[a-zA-Z]+$", RegexOptions.IgnoreCase);
        }
    }
}

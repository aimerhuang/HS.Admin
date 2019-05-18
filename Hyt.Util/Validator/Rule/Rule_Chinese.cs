
using System.Text.RegularExpressions;

namespace Hyt.Util.Validator.Rule
{
    /// <summary>
    /// 验证中文规则
    /// </summary>
    /// <remarks>2013-06-09 黄志勇 创建</remarks>
    public class Rule_Chinese : IRule
    {
        private string chinese;

        /// <summary>
        /// 验证规则 中文格式的字符串
        /// </summary>
        /// <param name="chinese">中文字符串.</param>
        /// <param name="message">验证失败的提示消息.</param>
        /// <remarks></remarks>
        public Rule_Chinese(string chinese, string message = "中文格式不正确")
        {
            this.chinese = chinese;
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
            if (string.IsNullOrWhiteSpace(chinese)) return false;
            return Regex.IsMatch(chinese, @"^[\u4e00-\u9fa5]+$", RegexOptions.IgnoreCase);
        }
    }
}

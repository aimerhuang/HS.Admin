using System.Text.RegularExpressions;

namespace Hyt.Util.Validator.Rule
{
    /// <summary>
    /// 验证数字
    /// </summary>
    /// <remarks>2013-12-30 黄志勇 注释</remarks>
   public class Rule_Number:IRule
    {
     
        private string number;

        /// <summary>
        /// 验证规则 email格式的字符串
        /// </summary>
        /// <param name="number">数字</param>
        /// <param name="message">验证失败的提示消息.</param>
        /// <remarks>2013-12-30 黄志勇 注释</remarks>
        public Rule_Number(string number, string message = "请输入数字！")
        {
            this.number = number;
            Message = message;
        }

        /// <summary>
        /// 验证是否有效
        /// </summary>
        /// <returns>是否有效</returns>
        /// <param></param>
        /// <returns>有效：true 无效：false</returns>
        /// <remarks>2013-12-30 黄志勇 注释</remarks>
        public override bool Valid()
        {
            if (string.IsNullOrWhiteSpace(number)) return false;
            return Regex.IsMatch(number, "^[0-9]*$");
        }
    }
}

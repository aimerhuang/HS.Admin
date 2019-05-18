using System;

namespace Hyt.Util.Validator.Rule
{
    /// <summary>
    /// 验证字符串是否为日期
    /// </summary>
    /// <remarks>2013-12-30 黄志勇 注释</remarks>
    public class Rule_DateTime : IRule
    {
        private string m_datetime;

        /// <summary>
        /// 验证字符串是否为日期
        /// </summary>
        /// <param name="datetime">日期字符串.</param>
        /// <param name="message">验证失败的提示消息.</param>
        /// <remarks>2013-12-30 黄志勇 注释</remarks>
        public Rule_DateTime(string datetime, string message = "字符串不是有效的日期格式")
        {
            m_datetime = datetime;
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
            if (string.IsNullOrWhiteSpace(m_datetime)) return false;
            DateTime dt;
            return DateTime.TryParse(m_datetime, out dt);
        }
    }
}


namespace Hyt.Util.Validator.Rule
{
    /// <summary>
    /// 验证规则 字符串长度
    /// </summary>
    /// <remarks>2013-12-30 黄志勇 注释</remarks>
    public class Rule_StringLenth : IRule
    {
        private string m_str;
        private int m_min;
        private int m_max;

        /// <summary>
        /// 验证规则 字符串长度
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="min">最小长度</param>
        /// <param name="max">最大长度</param>
        /// <param name="message">验证失败的提示消息</param>
        /// <remarks>2013-12-30 黄志勇 注释</remarks>
        public Rule_StringLenth(string str, int min = -1, int max = int.MaxValue, string message = "字符串长度不符合")
        {
            m_max = max;
            m_min = min;
            m_str = str;
            Message = message;
        }

        /// <summary>
        /// 验证是否有效
        /// </summary>
        /// <returns>有效：true 无效：false</returns>
        /// <remarks>2013-12-30 黄志勇 注释</remarks>
        public override bool Valid()
        {
            if (string.IsNullOrWhiteSpace(m_str)) return false;
            return m_str.Length >= m_min && m_str.Length <= m_max;
        }
    }
}


namespace Hyt.Util.Validator.Rule
{
    /// <summary>
    /// 验证规则 数据范围  <![CDATA[begin <= number <= end]]>
    /// </summary>
    /// <remarks>2013-12-30 黄志勇 注释</remarks>
    public class Rule_Range<T> :IRule where T : struct
    {        
        private dynamic m_min;
        private dynamic m_max;
        private dynamic m_number;

        /// <summary>
        /// 验证规则 数据范围  <![CDATA[min <= number <= max]]>
        /// </summary>
        /// <param name="number">数字.</param>
        /// <param name="min">最小值.</param>
        /// <param name="max">最大值.</param>
        /// <param name="message">验证失败的提示消息.</param>
        /// <remarks>2013-12-30 黄志勇 注释</remarks>
        public Rule_Range(T number, T min, T max, string message)
        {            
            m_number = number;
            m_min = min;
            m_max = max;
            Message = message;
        }

        /// <summary>
        /// 验证是否有效
        /// </summary>
        /// <returns>有效：true 无效：false</returns>
        /// <remarks>2013-12-30 黄志勇 注释</remarks>
        public override bool Valid()
        {
            return m_min <= m_number && m_number <= m_max;
        }
    }
}

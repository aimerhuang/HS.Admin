
namespace Hyt.Util.Validator.Rule
{
    /// <summary>
    /// 相等 规则
    /// </summary>
    /// <remarks>2013-12-30 黄志勇 注释</remarks>
    public class Rule_Equals<T> : IRule
    {
        private T m_t1;
        private T m_t2;

        /// <summary>
        /// 相等判断
        /// </summary>
        /// <param name="t1">泛型1</param>
        /// <param name="t2">泛型2</param>
        /// <param name="message">错误提示</param>
        /// <remarks>2013-12-30 黄志勇 注释</remarks>
        public Rule_Equals(T t1, T t2, string message = "值不相等")
        {
            m_t1 = t1;
            m_t2 = t2;
            Message = message;
        }

        /// <summary>
        /// 泛型相等验证
        /// </summary>
        /// <param></param>
        /// <returns>有效：true 无效：false</returns>
        /// <remarks>2013-12-30 黄志勇 注释</remarks>
        public override bool Valid()
        {
            return m_t1.Equals(m_t2);
        }
    }
}

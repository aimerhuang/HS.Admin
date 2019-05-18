
namespace Hyt.Util.Validator.Rule
{
    /// <summary>
    /// OR 验证规则 
    /// </summary>
    /// <remarks>2013-12-30 黄志勇 注释</remarks>
    public class Rule_Or : IRule
    {
        private IRule m_rule1;
        private IRule m_rule2;

        /// <summary>
        /// OR 验证规则
        /// </summary>
        /// <param name="rule1">验证规则A</param>
        /// <param name="rule2">验证规则B</param>
        /// <param name="message">验证失败的提示消息.</param>
        /// <remarks>2013-12-30 黄志勇 注释</remarks>
        public Rule_Or(IRule rule1, IRule rule2, string message)
        {
            m_rule1 = rule1;
            m_rule2 = rule2;
            Message = message;
        }

        /// <summary>
        /// 验证是否有效
        /// </summary>
        /// <returns></returns>
        /// <returns>有效：true 无效：false</returns>
        /// <remarks>2013-12-30 黄志勇 注释</remarks>
        public override bool Valid()
        {
            return m_rule1.Valid() || m_rule2.Valid();
        }
    }
}

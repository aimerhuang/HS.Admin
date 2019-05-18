
namespace Hyt.Util.Validator.Rule
{
    /// <summary>
    /// Not 验证规则
    /// </summary>
    /// <remarks>2013-12-30 黄志勇 注释</remarks>
    public class Rule_Not : IRule
    {
        private IRule m_rule;
        /// <summary>
        /// Not 验证规则
        /// </summary>
        /// <param name="rule">验证规则</param>
        /// <param name="message">验证失败的提示消息</param>
        /// <remarks>2013-12-30 黄志勇 注释</remarks>
        public Rule_Not(IRule rule,string message)
        {
            m_rule = rule;
            Message = message;
        }

        /// <summary>
        /// 验证是否有效
        /// </summary>
        /// <returns>有效：true 无效：false</returns>
        /// <remarks>2013-12-30 黄志勇 注释</remarks>
        public override bool Valid()
        {
            return !m_rule.Valid();
        }
    }    
}

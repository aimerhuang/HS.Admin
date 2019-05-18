
namespace Hyt.Util.Validator.Rule
{
    /// <summary>
    /// 验证规则 T类型为空
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks>2013-12-30 黄志勇 注释</remarks>
    public class Rule_EqualsNull<T> : IRule
    {
        private T m_obj;

        /// <summary>
        /// 验证规则 T类型允许为空
        /// </summary>
        /// <param name="obj">要验证的对象.</param>
        /// <param name="message">验证失败的提示消息.</param>
        /// <remarks>2013-12-30 黄志勇 注释</remarks>
        public Rule_EqualsNull(T obj, string message = "对象只能为空")
        {            
            Message = message;
            m_obj = obj;
        }

        /// <summary>
        /// 验证是否有效
        /// </summary>
        /// <param></param>
        /// <returns>有效：true 无效：false</returns>
        /// <remarks>2013-12-30 黄志勇 注释</remarks>
        public override bool Valid()
        {
            return m_obj == null;
        }
    }

    /// <summary>
    /// 验证规则 字符串必需为 null、空或者由空白字符组成
    /// </summary>
    /// <remarks>2013-12-30 黄志勇 注释</remarks>
    public class Rule_EqualsNull : IRule
    {
        private string m_text;

        /// <summary>
        /// 验证规则 字符串必需为 null、空或者由空白字符组成
        /// </summary>
        /// <param name="text">要验证的字符串.</param>
        /// <param name="message">验证失败的提示消息.</param>
        /// <remarks>2013-12-30 黄志勇 注释</remarks>
        public Rule_EqualsNull(string text, string message = "字符串允许为空")
        {
            Message = message;
            m_text = text;
        }

        /// <summary>
        /// 验证是否有效
        /// </summary>
        /// <param></param>
        /// <returns>有效：true 无效：false</returns>
        /// <remarks>2013-12-30 黄志勇 注释</remarks>
        public override bool Valid()
        {
            return string.IsNullOrWhiteSpace(m_text);
        }
    }
}

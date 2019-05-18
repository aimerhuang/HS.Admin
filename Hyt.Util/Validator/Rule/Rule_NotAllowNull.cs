
namespace Hyt.Util.Validator.Rule
{
    /// <summary>
    /// 验证规则 T类型不允许为空
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    /// <remarks>2013-12-30 黄志勇 注释</remarks>
    public class Rule_NotAllowNull<T> : IRule
    {
        private T m_obj;

        /// <summary>
        /// 验证规则 T类型不允许为空
        /// </summary>
        /// <param name="obj">要验证的对象.</param>
        /// <param name="message">验证失败的提示消息.</param>
        /// <remarks>2013-12-30 黄志勇 注释</remarks>
        public Rule_NotAllowNull(T obj, string message = "对象不允许为空")
        {
            Message = message;
            m_obj = obj;
        }

        /// <summary>
        /// 验证是否有效
        /// </summary>
        /// <returns>
        /// <c>true:有效</c>
        /// <c>false:无效</c>
        /// </returns>
        /// <remarks>2013-12-30 黄志勇 注释</remarks>
        public override bool Valid()
        {
            return m_obj != null;
        }
    }

    /// <summary>
    /// 验证规则 字符串不允许是 null、空还是仅由空白字符组成
    /// </summary>
    /// <remarks>2013-12-30 黄志勇 注释</remarks>
    public class Rule_NotAllowNull : IRule
    {
        private string m_text;

        /// <summary>
        /// 验证规则 字符串不允许是 null、空还是仅由空白字符组成
        /// </summary>
        /// <param name="text">要验证的字符串.</param>
        /// <param name="message">验证失败的提示消息.</param>
        /// <remarks>2013-12-30 黄志勇 注释</remarks>
        public Rule_NotAllowNull(string text, string message = "字符串不允许为空")
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
            return !string.IsNullOrWhiteSpace(m_text);
        }
    }
}

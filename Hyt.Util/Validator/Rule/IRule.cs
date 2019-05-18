using System;

namespace Hyt.Util.Validator.Rule
{
    /// <summary>
    /// 验证规则
    /// </summary>
    /// <remarks>
    /// 2013-3-1 杨文兵 创建
    /// 所有验证规则类以 “Rule_”开头，配合VS的自动提示功能方便书写代码
    /// </remarks>
    public abstract class IRule
    {
        protected Action m_OnFalse;

        /// <summary>
        /// 验证失败的提示消息
        /// </summary>
        public string Message
        {
            get;
            protected set;
        }

        /// <summary>
        /// 验证是否有效
        /// </summary>
        /// <returns>有效：true 无效：false</returns>
        /// <remarks>2013-12-30 黄志勇 注释</remarks>
        public abstract bool Valid();

        /// <summary>
        /// 当Valid()结果为false时执行
        /// </summary>
        /// <param name="action">无返回值的方法</param>
        /// <returns></returns>
        /// <remarks>2013-12-30 黄志勇 注释</remarks>
        public IRule False(Action action)
        {
            m_OnFalse = action;
            return this;
        }

        /// <summary>
        /// Called when [false].
        /// </summary>
        /// <param></param>
        /// <remarks>2013-12-30 黄志勇 注释</remarks>
        internal void OnFalse() 
        {
            if (m_OnFalse != null)
            {
                m_OnFalse.Invoke();
            }
        }

    }

}

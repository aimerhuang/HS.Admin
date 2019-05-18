
namespace Hyt.Util.Validator.Rule
{
    /// <summary>
    /// 购物车动作检查
    /// </summary>
    /// <remarks>2014-1-21 黄波 创建</remarks>
    public class Rule_CartAction : IRule
    {
        private string actionCode;

        /// <summary>
        /// 验证购物车动作
        /// </summary>
        /// <param name="action">动作代码</param>
        /// <param name="message">错误消息</param>
        /// <remarks>2014-1-21 黄波 创建</remarks>
        public Rule_CartAction(string action, string message = "未知动作")
        {
            actionCode = action;
            Message = message;
        }

        /// <summary>
        /// 验证是否有效
        /// </summary>
        /// <returns>
        /// <c>true:有效</c>
        /// <c>false:无效</c>
        /// </returns>
        /// <remarks>2014-1-21 黄波 创建</remarks>
        public override bool Valid()
        {
            if (string.IsNullOrWhiteSpace(actionCode)) return false;
            actionCode = actionCode.ToLower();
            return actionCode == "addgift"
                || actionCode == "delgift"
                || actionCode == "updatechecked"
                || actionCode == "updateallchecked"
                || actionCode == "delproduct"
                || actionCode == "updatequantity";
        }
    }
}

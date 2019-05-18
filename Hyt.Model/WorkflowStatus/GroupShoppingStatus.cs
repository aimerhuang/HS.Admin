using System.ComponentModel;

namespace Hyt.Model.WorkflowStatus
{
    /// <summary>
    /// 团购状态
    /// </summary>
    /// <remarks>2013-09-10 吴文强 创建</remarks>
    public class GroupShoppingStatus
    {

        /// <summary>
        /// 团购类型
        /// 数据表:GsGroupShopping 字段:GroupType
        /// </summary>
        /// <remarks>2013-08-22 吴文强 创建</remarks>
        public enum 团购类型
        {
            全部 = 0,
            商城 = 10,
            手机App = 20,
        }

        /// <summary>
        /// 团购支持区域类型
        /// 数据表:GsGroupShopping 字段:SupportAreaType
        /// </summary>
        /// <remarks>2013-08-22 吴文强 创建</remarks>
        public enum 团购支持区域类型
        {
            全国 = 0,
            百城当日达范围 = 10,
            指定区域 = 20,
        }

        /// <summary>
        /// 团购状态
        /// 数据表:GsGroupShopping 字段:Status
        /// </summary>
        /// <remarks>2013-08-22 吴文强 创建</remarks>
        public enum 团购状态
        {
            待审 = 10,
            已审 = 20,
            作废 = -10,
        }
    }
}

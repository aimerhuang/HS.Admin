
namespace Hyt.Model
{
    /// <summary>
    /// 调拨单查询显示结果
    /// </summary>
    /// <remarks>
    /// 2016-6-23 杨浩 创建
    /// 2016-06-28 陈海裕 修改
    /// </remarks>
    public class CBAtAllocation : AtAllocation
    {
        /// <summary>
        /// 当前页
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 快速查询条件
        /// </summary>
        public string Condition { get; set; }
    }
}

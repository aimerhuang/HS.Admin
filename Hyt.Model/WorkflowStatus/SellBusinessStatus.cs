using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Hyt.Model.WorkflowStatus
{
    /// <summary>
    /// 分销返利状态
    /// </summary>
    public class SellBusinessStatus
    {
        /// <summary>
        /// 动作类型
        /// 数据表:CrCustomerRebatesRecord 字段:Action
        /// </summary>
        /// <remarks>2015-11-04 王耀发 创建</remarks>
        public enum 动作类型
        {
            注册 = 0,
            关注 = 1,
            购物 = 2
        }

        /// <summary>
        /// 返利类型
        /// 数据表:CrCustomerRebatesRecord 字段:Genre
        /// </summary>
        /// <remarks>
        /// 2015-11-04 王耀发 创建
        /// 2016-1-9 杨浩 添加描述
        /// </remarks>
        public enum 返利类型
        {
            [Description("一级")]
            直接推荐 = 1,
            [Description("二级")]
            间1推荐 = 2,
            [Description("三级")]
            间2推荐 = 3,
            [Description("特殊")]
            特殊返利 = 9,
        }

        /// <summary>
        /// 状态类型
        /// 数据表:CrCustomerRebatesRecord 字段:Status
        /// </summary>
        /// <remarks>
        /// 2015-11-04 王耀发 创建
        /// 2016-1-5 杨浩 添加描述
        /// </remarks>
        public enum 状态类型
        {
            [Description("冻结")]
            未审核 = 0,
            [Description("完结")]
            完结=1,
            [Description("作废")]
            作废 = 2
        }
        /// <summary>
        /// 提现类型
        /// 数据表:CrPredepositCash 字段:PdType
        /// </summary>
        /// <remarks>2016-1-05 王耀发 创建</remarks>
        public enum 提现类型
        {
            会员 = 0,
            分销商 = 1
        }
    }
}

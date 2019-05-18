using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 优惠卡统计报表
    /// </summary>
    /// <remarks>2014-02-26 朱家宏 创建</remarks>
    public class CBRptCouponCard
    {
        /// <summary>
        /// 卡类型系统编号
        /// </summary>
        public int CardTypeSysNo { get; set; }

        /// <summary>
        /// 卡类型名称
        /// </summary>
        public string CardTypeName { get; set; }

        /// <summary>
        /// 优惠卡数量合计（新绑定数量）
        /// </summary>
        public int TotalNumber { get; set; }

        /// <summary>
        /// 优惠卡使用次数合计
        /// </summary>
        public int UsedNumber { get; set; }

        /// <summary>
        /// 统计月份
        /// </summary>
        public string Month { get; set; }

        /// <summary>
        /// 办事处
        /// </summary>
        public string AreaName { get; set; }

        /// <summary>
        /// 仓库
        /// </summary>
        public string WarehouseName { get; set; }

        /// <summary>
        /// 优惠卡金额合计
        /// </summary>
        public decimal TotalAmount { get; set; }
    }
}

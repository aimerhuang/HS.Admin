using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 会员分销详情统计
    /// </summary>
    public class CBCCustomerRebatesRecord : CrCustomer
    {
        /// <summary>
        /// 会员订单总数
        /// </summary>
        public int OrderNums { get; set; }
        /// <summary>
        /// 会员分销订单总数
        /// </summary>
        public int RebagesOrderCount { get; set; }
        /// <summary>
        /// 会员直接推荐分销订单总额
        /// </summary>
        public decimal DirectCount { get; set; }
        /// <summary>
        /// 会员间1分销订单总额
        /// </summary>
        public decimal Indirect1Count { get; set; }
        /// <summary>
        /// 会员间2分销订单总额
        /// </summary>
        public decimal Indirect2Count { get; set; }
        /// <summary>
        /// 分销商等级名称
        /// </summary>
        public string LevelName { get; set; }
        /// <summary>
        /// 等级直接推荐返利比例
        /// </summary>
        public decimal Direct { get; set; }
        /// <summary>
        /// 等级间1返利比例
        /// </summary>
        public decimal Indirect1 { get; set; }
        /// <summary>
        /// 等级间2返利比例
        /// </summary>
        public decimal Indirect2 { get; set; }
        /// <summary>
        /// 所属店铺名称
        /// </summary>
        public string DealerName { get; set; }
        /// <summary>
        /// 代理商
        /// </summary>
        public int DealerCreatedBy { get; set; }
    }
}

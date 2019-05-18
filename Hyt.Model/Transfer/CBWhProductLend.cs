using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Hyt.Model;

namespace Hyt.Model
{
    /// <summary>
    /// 借货单扩展类
    /// </summary>
    /// <remarks>2013-07-11 周唐炬 创建</remarks>
    public class CBWhProductLend
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        [Description("仓库名称")]
        public string WarehouseName { get; set; }

        /// <summary>
        /// 借货人名称
        /// </summary>
        [Description("借货人")]
        public string DeliveryUserName { get; set; }

        /// <summary>
        /// 是否在额度不够时强制放行：是（1）、否（0）
        /// </summary>
        [Description("是否在额度不够时强制放行：是（1）、否（0）")]
        public int IsEnforceAllow { get; set; }

        /// <summary>
        /// 金额(业务员信用价格)
        /// </summary>
        [Description("金额(业务员信用价格)")]
        public decimal Amount { get; set; }

        /// <summary>
        /// 状态:待出库(10),已出库(20),已完成[已补单已还货](30
        /// </summary>
        [Description("状态:待出库(10),已出库(20),已完成[已补单已还货](30")]
        public int Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Description("备注")]
        public string Remarks { get; set; }

        /// <summary>
        /// 创建人名称
        /// </summary>
        [Description("创建人")]
        public string CreatedByName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Description("创建时间")]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// 出库人名称
        /// </summary>
        [Description("出库人")]
        public string StockOutByName { get; set; }

        /// <summary>
        /// 出库时间
        /// </summary>
        [Description("出库时间")]
        public DateTime StockOutDate { get; set; }

        /// <summary>
        /// 最后更新人名称
        /// </summary>
        [Description("最后更新人")]
        public string LastUpdatedByName { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        [Description("最后更新时间")]
        public DateTime LastUpdateDate { get; set; }
    }
}

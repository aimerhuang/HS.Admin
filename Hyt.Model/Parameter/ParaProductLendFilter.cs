using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 借货单过滤类
    /// </summary>
    /// <remarks>2013-07-09 周唐炬 创建</remarks>
    public class ParaProductLendFilter
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int? SysNo { get; set; }

        /// <summary>
        /// 仓库编号
        /// </summary>
        public int? WarehouseSysNo { get; set; }

        /// <summary>
        /// 仓库编号集合
        /// </summary>
        public List<int> WarehouseSysNoList { get; set; }

        /// <summary>
        /// 配送员系统编号
        /// </summary>
        public int? DeliveryUserSysNo { get; set; }

        /// <summary>
        /// 是否在额度不够时强制放行：是（1）、否（0）
        /// </summary>
        public bool? IsEnforceAllow { get; set; }

        /// <summary>
        /// 金额(业务员信用价格)
        /// </summary>
        public decimal? Amount { get; set; }

        /// <summary>
        /// 状态:待出库(10),已出库(20),已完成[已补单已还货](30
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public int? CreatedBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// 出库人
        /// </summary>
        public int? StockOutBy { get; set; }

        /// <summary>
        /// 出库时间
        /// </summary>
        public DateTime? StockOutDate { get; set; }

        /// <summary>
        /// 最后更新人
        /// </summary>
        public int? LastUpdateBy { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime? LastUpdateDate { get; set; }

        /// <summary>
        /// 借货单系统编号
        /// </summary>
        public int? ProductLendSysNo { get; set; }

        /// <summary>
        /// 借货单商品明细
        /// </summary>
        public IList<ParaWhProductLendItemFilter> ItemList { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// 当前页
        /// </summary>
        public int CurrentPage { get; set; }
        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize { get; set; }
    }
}

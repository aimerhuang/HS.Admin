using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 借货单明细过滤类
    /// </summary>
    /// <remarks>2013-07-09 周唐炬 创建</remarks>
    public class ParaWhProductLendItemFilter
    {
        /// <summary>
        /// 借货单明细系统编号
        /// </summary>
        public int? SysNo { get; set; }
        /// <summary>
        /// 借货单系统编号
        /// </summary>
        public int? ProductLendSysNo { get; set; }
        /// <summary>
        /// 借货单状态:待出库(10),已出库(20),已完成[已补单已还货](30
        /// </summary>
        public int? Status { get; set; }
        /// <summary>
        /// 配送员系统编号
        /// </summary>
        public int? DeliveryUserSysNo { get; set; }

        /// <summary>
        /// 商品系统编号
        /// </summary>
        public int? ProductSysNo { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 借货数量
        /// </summary>
        public int? LendQuantity { get; set; }

        /// <summary>
        /// 价格来源：基础价格（0）、会员等级价格（10）、配送
        /// </summary>
        public int? PriceSource { get; set; }

        /// <summary>
        /// 来源编号
        /// </summary>
        public int? SourceSysNo { get; set; }

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

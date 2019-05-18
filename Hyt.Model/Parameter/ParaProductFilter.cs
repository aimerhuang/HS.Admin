using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 商品筛选实体
    /// </summary>
    /// <remarks>2014-1-15 沈强 创建</remarks>
    public class ParaProductFilter : CBPdProductDetail
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 商品创建开始时间范围
        /// </summary>
        public DateTime? CreateStartTime { get; set; }

        /// <summary>
        /// 商品创建结束时间范围
        /// </summary>
        public DateTime? CreateEndTime { get; set; }

        /// <summary>
        /// 分页查询id
        /// </summary>
        public int? id { get; set; }

        /// <summary>
        /// 商品价格来源类型
        /// </summary>
        public Hyt.Model.WorkflowStatus.ProductStatus.产品价格来源 PriceSource { get; set; }

        /// <summary>
        /// 价格来源系统编号（各种等级系统编号）
        /// </summary>
        public int PriceSourceSysNo { get; set; }

        /// <summary>
        /// 是否能在前台下单
        /// </summary>
        public int CanFrontEndOrder { get; set; }
        /// <summary>
        /// 是否绑定经销商
        /// </summary>
        public bool IsBindDealer { get; set; }
        /// <summary>
        /// 是否绑定所有经销商
        /// </summary>
        public bool IsBindAllDealer { get; set; }
        /// <summary>
        /// 经销商创建人
        /// </summary>
        public int DealerCreatedBy { get; set; }
        /// <summary>
        /// 搜索条件选中的代理商
        /// </summary>
        public int SelectedAgentSysNo { get; set; }
        /// <summary>
        /// 搜索条件选中的分销商
        /// </summary>
        public int SelectedDealerSysNo { get; set; }
        /// <summary>
        /// 是否筛选与总部不一致价格
        /// </summary>
        public int? HasChangePrice { get; set; }
        /// <summary>
        /// 是否搜索非主分类商品（0：否，1：是）
        /// </summary>
        public int IsSelectedIsMaster { get; set; }
    }
}
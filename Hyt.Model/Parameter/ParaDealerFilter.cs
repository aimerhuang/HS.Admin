using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 分销商查询
    /// </summary>
    /// <remarks>2014-1-15 沈强 创建</remarks>
    public class ParaDealerFilter
    {
        /// <summary>
        /// 分销商系统编号
        /// </summary>
        public int? SysNo { get; set; }

        /// <summary>
        /// 地区系统编号
        /// </summary>
        public int? AddressSysNo { get; set; }
        /// <summary>
        /// 分销商名称
        /// </summary>
        public string DealerName { get; set; }
        /// <summary>
        /// 状态
        /// 分销商状态:枚举 DistributionStatus.分销商状态
        /// 分销商往来账明细状态:枚举 DistributionStatus.预存款明细状态
        /// </summary>
        public int? Status { get; set; }
        /// <summary>
        /// 预存款明细单据来源:预存款(10),冻结返回(20),退换货(30),订单消
        /// </summary>
        public int? Source { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 结束时期
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 当前页号
        /// </summary>
        public int CurrentPage { get; set; }
        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 分销商
        /// </summary>
        public int DealerSysNo { get; set; }
        /// <summary>
        /// 分销商编号集合
        /// </summary>
        public List<int> DealerSysNoList { get; set; }
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
        /// 搜索条件选中的分销商
        /// </summary>
        public int SelectedDealerSysNo { get; set; }
        /// <summary>
        /// 单据来源编号
        /// </summary>
        public int? SourceSysNo { get; set; }

    }
}

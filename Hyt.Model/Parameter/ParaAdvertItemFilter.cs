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
    public class ParaAdvertItemFilter
    {
        /// <summary>
        /// 广告组编号
        /// </summary>
        public int groupSysNo { get; set; }

        /// <summary>
        /// 索引
        /// </summary>
        public int? id { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int? status { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? beginDate { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? endDate { get; set; }
        /// <summary>
        /// 广告项编号
        /// </summary>
        public int? commentSysNo { get; set; }

        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string linkTitle { get; set; }

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
    }
}

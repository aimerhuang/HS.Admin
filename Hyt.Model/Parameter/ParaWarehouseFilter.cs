using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 仓库查询
    /// </summary>
    /// <remarks>2013-8-15 周瑜 创建</remarks>
    public class ParaWarehouseFilter
    {
        private int _pageSize;

        /// <summary>
        /// 当前页号
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize
        {
            get
            {
                if (_pageSize == 0)
                {
                    _pageSize = 10;
                }
                return _pageSize;
            }
            set { _pageSize = value; }
        }

        /// <summary>
        /// 仓库系统编号
        /// </summary>
        public int? WarehouseSysNo { get; set; }

        /// <summary>
        /// 配送员系统编号
        /// </summary>
        public int? DeliverySysNo { get; set; }

        /// <summary>
        /// 单据来源
        /// </summary>
        public int? Source { get; set; }

        /// <summary>
        /// 单据状态
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 开始日期(起)
        /// </summary>
        public DateTime? BeginDate { get; set; }

        /// <summary>
        /// 结束时间(止)
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 单据来源编号
        /// </summary>
        public int? SourceSysNo { get; set; }
    }
}

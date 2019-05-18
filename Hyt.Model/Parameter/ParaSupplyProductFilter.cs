using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 供应商产品参数
    /// </summary>
    /// <remarks>2016-3-17 杨浩 创建</remarks>
    /// <remarks>2016-3-21 刘伟豪 修改</remarks>
    public class ParaSupplyProductFilter : CBScProduct
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
        /// 每页显示条数
        /// </summary>
        public int? PageSize { get; set; }
        /// <summary>
        /// 模糊查询关键字
        /// </summary>
        public string KeyWord { get; set; }
        /// <summary>
        /// 是否已入库
        /// </summary>
        public int? HasStockIn { get; set; }
    }
}
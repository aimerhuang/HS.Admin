using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 仓库内勤绩效报表，查询过滤参数
    /// </summary>
    /// <remarks>2014-1-15 沈强 创建</remarks>
    public class ParaWarehouseInsideStaffFilter
    {
        /// <summary>
        /// 仓库系统编号数组
        /// </summary>
        public List<int> WarehouseSysNos { get; set; }

        /// <summary>
        /// 统计时间
        /// </summary>
        public string DateCalculated { get; set; }
    }
}

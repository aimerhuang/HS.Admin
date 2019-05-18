using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 用于仓库内勤报表Excel导出
    /// </summary>
    /// <remarks>2014-01-20 沈强 创建</remarks>
    public class CBExportWarehouseInsideStaff
    {
        /// <summary>
        /// 仓库
        /// </summary>
        /// <remarks>2014-01-20 沈强 创建</remarks>
        public string 仓库 { get; set; }

        /// <summary>
        /// 内勤
        /// </summary>
        /// <remarks>2014-01-20 沈强 创建</remarks>
        public string 内勤 { get; set; }
        /// <summary>
        /// 处理单量_百城达
        /// </summary>
        /// <remarks>2014-01-20 沈强 创建</remarks>
        public int 处理单量_百城达 { get; set; }
        /// <summary>
        /// 处理单量_第三方
        /// </summary>
        /// <remarks>2014-01-20 沈强 创建</remarks>
        public int 处理单量_第三方 { get; set; }

    }
}

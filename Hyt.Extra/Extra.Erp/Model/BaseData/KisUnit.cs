using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.Erp.Model.BaseData
{
    /// <summary>
    /// 单位
    /// </summary>
    public class KisUnit
    {
        /// <summary>
        /// 商场数据库名称
        /// </summary>
        public string FAcctDB { get;set; }
        /// <summary>
        /// 计量单位编码
        /// </summary>
        public string FNumber { get; set; }
        /// <summary>
        /// 计量单位名称
        /// </summary>
        public string FName { get; set; }
        /// <summary>
        /// 计量单位组名称
        /// </summary>
        public string FUnitGroupName { get; set; }


    }
}

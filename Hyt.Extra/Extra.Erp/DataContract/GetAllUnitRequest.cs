using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.Erp.DataContract
{
    /// <summary>
    /// 获取全部Kis计量单位
    /// </summary>
    /// <remarks>2016-11-23 杨浩 创建</remarks>
    public class GetAllUnitRequest : BaseRequest
    {
        /// <summary>
        /// 商城数据库名称
        /// </summary>
        public string FAcctDB { get; set; }
        /// <summary>
        /// 计量单位编码
        /// </summary>
        public string FNumber{ get; set; }
        /// <summary>
        /// 计量单位名称
        /// </summary>
        public string FName { get; set; }
        /// <summary>
        /// 计量单位组名称
        /// </summary>
        public string FUnitGroupName { get; set; }
        /// <summary>
        /// 商城标记
        /// </summary>
        public string F { get; set; }
    }
}

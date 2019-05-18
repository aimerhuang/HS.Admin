using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Common
{
    /// <summary>
    /// ERP接口配置
    /// </summary>
    /// <remarks>2016-11-22 杨浩 创建</remarks>
    public class ErpConfig : ConfigBase
    {
        /// <summary>
        /// 身份key值
        /// </summary>
        public string AppKey { get; set; }
        /// <summary>
        /// 身份效验码
        /// </summary>
        public string AppScode { get; set; }
        /// <summary>
        /// 网关
        /// </summary>
        public string Gateway { get; set; }
        /// <summary>
        /// 启用接口（1：是 0：否）
        /// </summary>
        public int Enable { get; set; }
        /// <summary>
        /// 供货单位代码
        /// </summary>
        public string FCustID { get; set; }
        /// <summary>
        /// 单位代码
        /// </summary>
        public string FUnitID { get; set; }
        /// <summary>
        /// 兴业仓库代码例:,01.02,02.03,
        /// </summary>
        public string XinYeWarehouse { get; set; }
    }
}

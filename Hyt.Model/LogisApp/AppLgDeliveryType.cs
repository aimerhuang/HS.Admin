using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.LogisApp
{
    /// <summary>
    /// APP配送方式 
    /// </summary>
    /// <remarks>2013-08-01 周唐炬 创建</remarks>
    public class AppLgDeliveryType : BaseEntity
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 配送方式名称
        /// </summary>
        public string DeliveryTypeName { get; set; }
        /// <summary>
        /// 显示顺序
        /// </summary>
        public int DisplayOrder { get; set; }
        /// <summary>
        /// 是否生成optgroup
        /// </summary>
        public bool OptGroup { get; set; }
        /// <summary>
        /// 地区父级系统编号
        /// </summary>
        public int ParentSysNo { get; set; }
    }
}

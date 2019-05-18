using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 店铺关联广告项表
    /// </summary>
    ///<remarks>2016-07-28 周 创建</remarks>
    [Serializable]
    public partial class DsDealerFeAdvertItem
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }

        /// <summary>
        /// 广告项id
        /// </summary>
        [Description("广告项id")]
        public int FeAdvertItemSysNO { get; set; }

        /// <summary>
        /// 仓库ID
        /// </summary>
       [Description("仓库ID")]
        public int WarehoseSysNo { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
       [Description("创建人")]
        public int CreatedBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Description("创建时间")]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
       [Description("更新人")]
        public int LastUpdateBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [Description("更新时间")]
       public DateTime LastUpdateDate { get; set; }

        /// <summary>
        /// 使用平台
        /// </summary>
        [Description("使用平台")]
        public int PlatformType { get; set; }

        /// <summary>
        /// 广告组代码
        /// </summary>
        [Description("广告组代码")]
        public string FeAdvertGroupCode { get; set; }
    }
}

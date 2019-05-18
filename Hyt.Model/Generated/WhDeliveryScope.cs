using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    ///  2014-10-09 杨浩 T4生成
    /// </remarks>
    public partial class WhDeliveryScope : BaseEntity
    {
        ///<summary>
        ///系统编号
        ///</summary>
        [Description("系统编号")]
        public int SysNo { get; set; }

        ///<summary>
        ///地区编号
        ///</summary>
        [Description("地区编号")]
        public int AreaSysNo { get; set; }
        ///<summary>
        ///仓库系统编号
        ///</summary>
        [Description("仓库系统编号")]
        public int WarehouseSysNo { get; set; }

        ///<summary>
        ///地图坐标
        ///</summary>
        [Description("地图坐标")]
        public string MapScope { get; set; }

        ///<summary>
        ///描述
        ///</summary>
        [Description("描述")]
        public string Description { get; set; }

        ///<summary>
        ///创建人
        ///</summary>
        [Description("创建人")]
        public int CreatedBy { get; set; }

        ///<summary>
        ///创建时间
        ///</summary>
        [Description("创建时间")]
        public DateTime CreatedDate { get; set; }

        ///<summary>
        ///最后更新人
        ///</summary>
        [Description("最后更新人")]
        public int LastUpdateBy { get; set; }

        ///<summary>
        ///最后更新时间
        ///</summary>
        [Description("最后更新时间")]
        public DateTime LastUpdateDate { get; set; }


    }
}

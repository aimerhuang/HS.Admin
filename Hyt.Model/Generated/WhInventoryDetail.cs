using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    
    [Serializable]
    public class WhInventoryDetail
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }

        /// <summary>
        /// 盘点单系统编号
        /// </summary>
        [Description("盘点单系统编号")]
        public int InventorySysNo { get; set; }

        /// <summary>
        /// 仓库产品库存表编号
        /// </summary>
        [Description("仓库产品库存表编号")]
        public int ProductStockSysNo { get; set; }

        /// <summary>
        /// 实盘数
        /// </summary>
        [Description("实盘数")]
        public int RealStock { get; set; }

        /// <summary>
        /// 盘点仓库系统编号
        /// </summary>
        [Description("盘点仓库系统编号")]
        public int WarehouseSysNo { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Description("备注")]
        public string Remarks { get; set; }

        /// <summary>
        /// 商品系统编号
        /// </summary>
        [Description("商品系统编号")]
        public int ProductSysNo { get; set; }
    }
}

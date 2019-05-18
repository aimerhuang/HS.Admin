using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Hyt.Model.InventorySheet
{
    /// <summary>
    /// 盘点仓库详情
    /// </summary>
    public class WhInventoryWhDetail
    {
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }

        /// 盘点单系统编号
        /// </summary>
        [Description("盘点单系统编号")]
        public int InventorySysNo { get; set; }

        /// 盘点仓库系统编号
        /// </summary>
        [Description("盘点仓库系统编号")]
        public int WarehouseSysNo { get; set; }

        /// 备注
        /// </summary>
        [Description("备注")]
        public int Remarks { get; set; }


    }
}

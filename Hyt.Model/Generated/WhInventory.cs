using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    public class WhInventory
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }

        /// <summary>
        /// 盘点状态
        /// </summary>
        [Description("盘点状态")]
        public int Status { get; set; }

        /// <summary>
        /// 盘点单编号
        /// </summary>
        [Description("盘点单编号")]
        public string TransactionSysNo { get; set; }

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
        /// 盘点人
        /// </summary>
        [Description("盘点人")]
        public int CreatedBy { get; set; }

        /// <summary>
        /// 盘点开始时间
        /// </summary>
        [Description("盘点开始时间")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 盘点结束时间
        /// </summary>
        [Description("盘点结束时间")]
        public DateTime EndDate { get; set; }
    }


   
}

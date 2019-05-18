using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Generated
{
    /// <summary>
    /// 库存情况变化记录
    /// </summary>
    /// <remarks>2016-12-26 杨云奕 添加</remarks>
    public class WhWarehouseChangeLog
    {
        /// <summary>
        /// 自动编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 仓库编号
        /// </summary>
        public int WareSysNo { get; set; }
        /// <summary>
        /// 商品编号
        /// </summary>
        public int ProSysNo { get; set; }
        /// <summary>
        /// 变动数量
        /// </summary>
        public int ChangeQuantity { get; set; }
        /// <summary>
        /// 存余数量
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// 业务类型
        /// </summary>
        public string BusinessTypes { get; set; }
        /// <summary>
        /// 信息描述
        /// </summary>
        public string LogData { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ChageDate { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
    }
}

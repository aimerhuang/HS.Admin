using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hyt.Admin.Models
{
    /// <summary>
    /// 批量发货实体
    /// </summary>
    /// <remarks>2017-07-03 杨浩 创建</remarks>
    public class BatchCreateLgDeliveryMdl
    {
        /// <summary>
        /// 仓库系统编号
        /// </summary>
        public int WarehouseSysNo { get; set; }
        /// <summary>
        /// 配送类型系统编号
        /// </summary>
        public int DeliverTypeSysno { get; set; }
        /// <summary>
        /// 单据类型
        /// </summary>
        public int NoteType { get; set; }
        /// <summary>
        /// 单据类型编号
        /// </summary>
        public int NoteSysNo { get; set; }
        /// <summary>
        /// 快递单号
        /// </summary>
        public string ExpressNo { get; set; }
    }
}
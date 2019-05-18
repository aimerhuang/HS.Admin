using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.Erp.Model
{
    /// <summary>
    /// 调拨单导入
    /// </summary>
    /// <remarks>2017-1-4 杨浩 创建</remarks>
    public class TransferStockInfo
    {
        /// <summary>
        /// 仓库系统编号
        /// </summary>
        public int WarehouseSysNo { get; set; }
        /// <summary>
        /// Erp仓库编号
        /// </summary>
        public string WarehouseNumber { get; set; }
        /// <summary>
        /// Erp商品编码
        /// </summary>
        public string ErpCode { get; set; }
        /// <summary>
        /// 出库仓库编码
        /// </summary>
        public string FSCStockID { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string FNote { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        public string FDeptID { get; set; }
        /// <summary>
        /// 业务员
        /// </summary>
        public string FEmpID { get; set; }
        /// <summary>
        /// 保管人
        /// </summary>
        public string FFManagerID { get; set; }
        /// <summary>
        /// 调拨日期
        /// </summary>
        public string FDate { get; set; }
    }

    public class TransferStockInfoWraper
    {
        /// <summary>
        /// 调拨实体列表
        /// </summary>
        public List<TransferStockInfo> Model { get; set; }
        /// <summary>
        /// 调拨状态
        /// </summary>
        public 调拨状态 Type { get; set; }
           
        /// <summary>
        /// 单据摘要
        /// </summary>
        public string Description { get; set; }


    }

}

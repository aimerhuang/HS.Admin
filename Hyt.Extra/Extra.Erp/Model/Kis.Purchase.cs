using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.Erp.Model
{
    /// <summary>
    /// 采购入库、退货
    /// </summary>
    /// <remarks>2017-12-29 杨浩 创建</remarks>
    public  class PurchaseInfo
    {
        /// <summary>
        /// 同步次数
        /// </summary>
        public int SynchronizeCount { get; set; }
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
        /// 入库数量
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// 采购数
        /// </summary>
        public int PurchaseQty { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 总金额（单价*数量）
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string FNote { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public string FDate { get; set; }

        /// <summary>
        /// 供应商编码
        /// </summary>
        public string FSupplyID { get; set; }
        /// <summary>
        /// 业务员编码
        /// </summary>
        public string FEmpID { get; set; }
        /// <summary>
        /// 部门编码
        /// </summary>
        public string FDeptID { get; set; }
        /// <summary>
        /// 保管人编码
        /// </summary>
        public string FMangerID { get; set; }   
        /// <summary>
        /// 结算日期
        /// </summary>
        public string SettleDate { get; set; }
    }

    /// <summary>
    /// 实体参数包装
    /// </summary>
    /// <remarks>2017-12-29 杨浩 创建</remarks>
    public class PurchaseInfoWraper
    {
        /// <summary>
        /// 采购入库、退货实体列表
        /// </summary>
        public List<PurchaseInfo> Model { get; set; }
        /// <summary>
        /// 出库状态
        /// </summary>
        public 采购状态 Type { get; set; }
        /// <summary>
        /// 单据摘要
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 采购单系统编号
        /// </summary>
        public string PurchaseOrderSysNo { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public string TimeSpan
        {
            get { return DateTime.Now.ToString("yyyyMMddHHmmssfff"); }
            set { }
        }
    }
}

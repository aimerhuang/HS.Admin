using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.Erp.Model.BaseData
{
    public class EasSearchSellOutStockItem 
    {
        public string Fauxprice { get; set; }
        public string Famount { get; set; }
        public string FItemID { get; set; }
        public string FROB { get; set; }
        public string FBillNo { get; set; }
        public string FUnitID { get; set; }
        public string FBatchNo { get; set; }
        public string FKFPeriod { get; set; }
        public string FExplanation { get; set; }
        public string FManagerID { get; set; }
        public string FItemName { get; set; }
        public string Fnote { get; set; }
        public string Fdate { get; set; }
        public string FEntryID { get; set; }
        public string OrderSysNo { get; set; }
        public string Fauxqty { get; set; }
        public string FDCStockID { get; set; }
    }

    /// <summary>
    /// erp销售出库单
    /// </summary>
    /// <remarks>2016-09-28 杨云奕 添加</remarks>
    public class EasSellOutStock
    {
        /// <summary>
        /// 序号
        /// </summary>
        public string FEntryID { get; set; }
        /// <summary>
        /// 单据编号
        /// </summary>
        public string FBillNo { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public string Fdate { get; set; }
        /// <summary>
        /// 收 货 方
        /// </summary>
        public string FConsignee { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        public string FDeptID { get; set; }
        /// <summary>
        /// 业务员
        /// </summary>
        public string FEmpID { get; set; }
        /// <summary>
        /// 摘要
        /// </summary>
        public string FExplanation { get; set; }
        /// <summary>
        /// 交货地点
        /// </summary>
        public string FFetchAdd { get; set; }
        /// <summary>
        /// 发货人
        /// </summary>
        public string FFManagerID { get; set; }
        /// <summary>
        /// 销售方式
        /// </summary>
        public string FSaleStyle { get; set; }
        /// <summary>
        /// 保管
        /// </summary>
        public string FSManagerID { get; set; }
        /// <summary>
        /// 购货单位
        /// </summary>
        public string FCustID { get; set; }
        /// <summary>
        /// 红蓝字标记
        /// </summary>
        public string FROB { get; set; }
        /// <summary>
        /// 发货仓库
        /// </summary>
        public string FDCStockID { get; set; }

        public List<EasSellOutStockItem> item = new List<EasSellOutStockItem>();
    }
    /// <summary>
    /// 出库商品明细
    /// </summary>
    public class EasSellOutStockItem
    {
       
        /// <summary>
        /// 商品编码
        /// </summary>
        public string FItemID { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string FItemName { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string FUnitID { get; set; }
        /// <summary>
        /// 销售单价
        /// </summary>
        public string FConsignPrice { get; set; }
        /// <summary>
        /// 销售数量
        /// </summary>
        public string Fauxqty { get; set; }
        /// <summary>
        /// 销售金额
        /// </summary>
        public string FConsignAmount { get; set; }
        /// <summary>
        /// 批号
        /// </summary>
        public string FBatchNo { get; set; }
        /// <summary>
        /// 发货仓库
        /// </summary>
        public string FDCStockID { get; set; }
        /// <summary>
        /// 生产/采购日期
        /// </summary>
        public string FKFDate { get; set; }
        /// <summary>
        /// 保质期(天)
        /// </summary>
        public string FKFPeriod { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Fnote { get; set; }
        /// <summary>
        /// 有效期至
        /// </summary>
        public string FPeriodDate { get; set; }
        /// <summary>
        /// 折扣额
        /// </summary>
        public string FDiscountAmount { get; set; }
        /// <summary>
        /// 折扣率(%)
        /// </summary>
        public string FDiscountRate { get; set; }
    }
}

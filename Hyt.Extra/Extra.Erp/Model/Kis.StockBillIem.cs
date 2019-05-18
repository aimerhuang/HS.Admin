using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.Erp.Model
{
    /// <summary>
    /// 出库单据项
    /// </summary>
    /// <remarks>2016-11-23 杨浩 创建</remarks>
    public class KisStockBillIem
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

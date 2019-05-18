using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.Erp.DataContract
{
    /// <summary>
    /// 销售单查询响应结果
    /// </summary>
    /// <remarks>2016-12-12 杨浩 创建</remarks>
    public class SaleSearchResponse
    {
        /// <summary>
        /// 单据编号
        /// </summary>
        public string FBillNo { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime? Fdate { get; set; }
        /// <summary>
        /// 购货单位
        /// </summary>
        public string FCustID { get; set; }

        /// <summary>
        /// 客户地点
        /// </summary>
        public string FCustAddress { get; set; }
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
        /// 交货方式
        /// </summary>
        public string FFetchStyle { get; set; }
        /// <summary>
        /// 销售方式
        /// </summary>
        public string FSaleStyle { get; set; }
        /// <summary>
        /// 结算日期
        /// </summary>
        public string FSettleDate { get; set; }

        /// <summary>
        /// 结算方式
        /// </summary>
        public string FSettleID { get; set; }

        /// <summary>
        /// 币别
        /// </summary>
        public string FCurrencyID { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        public string FChecker { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? FCheckDate { get; set; }
        /// <summary>
        /// 销售明细
        /// </summary>
        public IList<SaleItem> data{ get; set; }
    }
    /// <summary>
    /// 销售单明细
    /// </summary>
    ///  <remarks>2016-12-12 杨浩 创建</remarks>
    public class SaleItem
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int FEntryID { get; set; }
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
        /// 单价
        /// </summary>
        public decimal Fauxprice { get; set; }
        /// <summary>
        /// 实收数量
        /// </summary>
        public decimal Fauxqty { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Famount { get; set; }
        /// <summary>
        /// 含税单价
        /// </summary>
        public decimal FAuxTaxPrice { get; set; }
        /// <summary>
        /// 税率(%)
        /// </summary>
        public decimal FCess { get; set; }
        /// <summary>
        /// 价税合计
        /// </summary>
        public decimal FAllAmount { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Fnote { get; set; }
    }
}

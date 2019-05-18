using Extra.Erp.Model.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.Erp.Model.BaseData
{
    /// <summary>
    /// 调拨单实体
    /// </summary>
    public class EasTransfers : EasBaseAccount
    {
        /// <summary>
        /// 单据编号
        /// </summary>
        public string FBillNo { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public string Fdate { get; set; }
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
        /// 验收人
        /// </summary>
        public string FFManagerID { get; set; }
        /// <summary>
        /// 保管
        /// </summary>
        public string FSManagerID { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        public string FChecker { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public string CheckDate { get; set; }

        public string FEntryID { get; set; }

        public List<EasTransfersItem> DataList = new List<EasTransfersItem>();
        public List<EasTransfersItem> item = new List<EasTransfersItem>();
    }
    /// <summary>
    /// 调拨单明细
    /// </summary>
    public class EasTransfersItem
    {
        /// <summary>
        /// /序号
        /// </summary>
        public string FEntryID { get; set; }
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
        public decimal FAuxPrice { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public decimal Fauxqty { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal FAmount { get; set; }
        /// <summary>
        /// 批号
        /// </summary>
        public string FBatchNo { get; set; }
        /// <summary>
        /// 调入仓库
        /// </summary>
        public string FDCStockID { get; set; }
        /// <summary>
        /// 调出仓库
        /// </summary>
        public string FSCStockID { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Fnote { get; set; }
    }
}

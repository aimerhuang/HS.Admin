using Extra.Erp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Extra.Erp.DataContract
{
    /// <summary>
    /// 销售出库、退货
    /// </summary>
    /// <remarks>2016-11-23 杨浩  创建</remarks>

    public class SaleIssueBillFacadeRequest : BaseRequest
    {
        /// <summary>
        /// 商城数据库
        /// </summary>
        public string FAcctDB { get; set; }
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
        /// 红蓝字标记(1：正常单据-1：退回单据)
        /// </summary>
        public string FROB { get; set; }
        /// <summary>
        /// 发货仓库
        /// </summary>
        public string FDCStockID { get; set; }

        /// <summary>
        /// 单据明细
        /// </summary>
        [DataMember(Name = "item")]
        public IList<KisStockBillIem> item { get; set; }
    }
}

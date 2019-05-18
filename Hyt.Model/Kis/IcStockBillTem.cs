using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Hyt.Model.Kis
{
    /// <summary>
    /// 主表
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    public class IcStockBillTem
    {
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public string FDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public int FTranType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public string FBillerID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public int FK3BillerID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public string FDeptID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public int FK3DeptID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public string FEmpID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public int FK3EmpID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public string FExplanation { get; set; }
        /// <summary>
        /// ()
        /// </summary>
        [Description("")]
        public string FFManagerID { get; set; }
        /// <summary>
        /// ()
        /// </summary>
        [Description("()")]
        public int FK3FManagerID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public string FManagerID { get; set; }
        /// <summary>
        /// ()
        /// </summary>
        [Description("()")]
        public int FK3ManagerID { get; set; }
        /// <summary>
        /// ()
        /// </summary>
        [Description("()")]
        public string FSManagerID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public int FK3SManagerID { get; set; }
        /// <summary>
        /// ()
        /// </summary>
        [Description("()")]
        public string FSupplyID { get; set; }
        /// <summary>
        /// (，)
        /// </summary>
        [Description("(，)")]
        public int FK3SupplyID { get; set; }
        /// <summary>
        /// ()
        /// </summary>
        [Description("()")]
        public string Fuse { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public string FInBillNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public int FentryCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public int FUpdate { get; set; }
        /// <summary>
        ///
        /// </summary>
        [Description("")]
        public string FCurrencyID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public string FInventoryType { get; set; }
        /// <summary>
        ///
        /// </summary>
        [Description("")]
        public int FK3InventoryType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public string FBillTypeID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public int FK3BillTypeID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public string FRefType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public int FK3RefType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public int FRob { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public int FK3CurrencyID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public string FFetchDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public string FConsignee { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public string FDeliveryPlace { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public string FSettleDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public string FBillNo { get; set; }
    }
}

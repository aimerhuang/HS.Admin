using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Hyt.Model.Kis
{

    public class JsonModel
    {
        /// <summary>
        /// ()
        /// </summary>
        [Description("()")]
        public string FAcctDB { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public string FBillNo { get; set; }
        /// <summary>
        /// ()
        /// </summary>
        [Description("()")]
        public string FConsignee { get; set; }
        /// <summary>
        /// ()
        /// </summary>
        [Description("()")]
        public string FCustID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public string FDCStockID { get; set; }
        /// <summary>
        /// ()
        /// </summary>
        [Description("()")]
        public string FDeptID { get; set; }
        /// <summary>
        /// (，)
        /// </summary>
        [Description("()")]
        public string FEmpID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public string FEntryID { get; set; }
        /// <summary>
        /// ()
        /// </summary>
        [Description("()")]
        public string FExplanation { get; set; }
        /// <summary>
        /// (，)
        /// </summary>
        [Description("()")]
        public string FFManagerID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public string FFetchAdd { get; set; }
        /// <summary>
        /// ()
        /// </summary>
        [Description("()")]
        public string FROB { get; set; }
        /// <summary>
        /// (，)
        /// </summary>
        [Description("()")]
        public string FSManagerID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public string FSaleStyle { get; set; }
        /// <summary>
        /// ()
        /// </summary>
        [Description("()")]
        public string Fdate { get; set; }
        /// <summary>
        /// (，)
        /// </summary>
        [Description("()")]
        public List<JsonItem> item { get; set; }
    }
}

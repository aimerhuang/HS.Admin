using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.Erp.DataContract
{
    /// <summary>
    /// 销售出库退货回执
    /// </summary>
    /// <remarks>2016-12-1 杨浩 创建</remarks>
    public class StockOutAddResponse
    {
        public string  FBillNoFail{get;set;}
        /// <summary>
        /// erp单据编号
        /// </summary>
        public string FBillNo{get;set;}
        /// <summary>
        /// 外部单据编号
        /// </summary>
        public string OutFBillNo{get;set;}

    }
}

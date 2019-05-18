using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.UpGrade
{
    /// <summary>
    /// 订单查询参数
    /// </summary>
    /// <remarks>2017-7-8 罗勤尧 创建</remarks>
   public class HaiOrderParameters
    {
        #region 传入参数
        /// <summary>
        /// 商城类型
        /// </summary>
       public int Type { get; set; }
       /// <summary>
       /// 商城编号
       /// </summary>
       public int SysNo { get; set; }
        /// <summary>
        /// 订单起始时间
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 订单截止时间
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 第三方订单号
        /// </summary>
        public string OrderList { get; set; }
        /// <summary>
        /// 是否选择按订单号查找
        /// </summary>
        public bool Status { set; get; }
        #endregion
    }
}
